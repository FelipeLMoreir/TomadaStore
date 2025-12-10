using Payment_API.Repository.Interfaces;
using Payment_API.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TomadaStore.Models.DTOs.Payment;
using TomadaStore.Models.DTOs.Sales;

namespace TomadaStore.PaymentAPI.Services
{
    public class PaymentConsumerService : IPaymentConsumerService
    {
        private readonly ILogger<PaymentConsumerService> _logger;
        private readonly IPaymentProducerRepository _producerRepository;
        private readonly ConnectionFactory _factory;

        public PaymentConsumerService(
            ILogger<PaymentConsumerService> logger,
            IPaymentProducerRepository producerRepository)
        {
            _logger = logger;
            _producerRepository = producerRepository;
            _factory = new ConnectionFactory { HostName = "localhost" };
        }

        public async Task StartConsumeAsync()
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "payment.requests",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            await channel.QueueDeclareAsync(
                queue: "approved-sales.queue",
                durable: false, 
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            _logger.LogInformation("PaymentAPI waiting for payment requests...");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation("Payment request received: {Message}", message);

                var paymentRequest = JsonSerializer.Deserialize<SaleEventDTO>(message); 

                if (paymentRequest.TotalAmount > 1000)
                {
                    _logger.LogWarning("Sale REJECTED - Wallet limit exceeded: {TotalAmount} > 1000", paymentRequest.TotalAmount);
                    await Task.CompletedTask; 
                    return;
                }

                var approvedEvent = new ApprovedSaleEventDTO
                {
                    SaleId = paymentRequest.SaleId,
                    CustomerId = paymentRequest.CustomerId,
                    CustomerName = paymentRequest.CustomerName,
                    Products = paymentRequest.Products,
                    TotalAmount = paymentRequest.TotalAmount,
                    ApprovedAt = DateTime.UtcNow
                };

                var approvedJson = JsonSerializer.Serialize(approvedEvent);
                var approvedBody = Encoding.UTF8.GetBytes(approvedJson);

                await channel.BasicPublishAsync(
                      exchange: string.Empty,
                      routingKey: "approved-sales.queue",
                      body: approvedBody);

                _logger.LogInformation("Sale APPROVED: {SaleId} | Total: {TotalAmount}",
                    approvedEvent.SaleId, approvedEvent.TotalAmount);

                await Task.CompletedTask;
            };

            await channel.BasicConsumeAsync("payment.requests", autoAck: true, consumer: consumer);
        }
    }
}