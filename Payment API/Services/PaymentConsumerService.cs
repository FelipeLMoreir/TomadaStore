using Payment_API.Repository.Interfaces;
using Payment_API.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TomadaStore.Models.DTOs.Payment;

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
                arguments: null);

            _logger.LogInformation(" [*] PaymentAPI waiting for payment requests...");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Payment request received: {Message}", message);

                var paymentRequest = JsonSerializer.Deserialize<PaymentRequestDTO>(message);

                var paymentResponse = new PaymentResponseDTO
                {
                    PaymentId = Guid.NewGuid().ToString(),
                    SaleId = paymentRequest.SaleId,
                    Status = "Approved", 
                    Amount = paymentRequest.TotalAmount
                };

                await _producerRepository.PublishPaymentConfirmedAsync(paymentResponse);

                await Task.CompletedTask;
            };

            await channel.BasicConsumeAsync("payment.requests", autoAck: true, consumer: consumer);
        }
    }
}