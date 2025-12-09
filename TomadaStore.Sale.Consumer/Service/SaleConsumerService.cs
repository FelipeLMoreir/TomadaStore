using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TomadaStore.Models.DTOs.Payment;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Sale.Consumer.Repository.Interfaces;
using TomadaStore.Sale.Consumer.Service.Interfaces;
using TomadaStore.SaleAPI.Repository.Interfaces;

namespace TomadaStore.Sale.Consumer.Service
{
    public class SaleConsumerService : ISaleConsumerService
    {
        private readonly ILogger<SaleConsumerService> _logger;
        private readonly HttpClient _paymentClient;
        private readonly ConnectionFactory _factory;
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleConsumerRepository _saleConsumerRepository;

        public SaleConsumerService(ILogger<SaleConsumerService> logger, HttpClient paymentClient, 
            ISaleRepository saleRepository, ISaleConsumerRepository saleConsumerRepository)
        {
            _logger = logger;
            _paymentClient = paymentClient;
            _factory = new ConnectionFactory { HostName = "localhost" };
            _saleRepository = saleRepository;
            _saleConsumerRepository = saleConsumerRepository;
        }

        public async Task StartConsumeAsync()
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "sale.requests", durable: false,
                exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueDeclareAsync(queue: "payment.requests", durable: false,
                exclusive: false, autoDelete: false, arguments: null);

            await channel.QueueDeclareAsync(queue: "payment.confirmed", durable: false,
                exclusive: false, autoDelete: false, arguments: null);

            _logger.LogInformation(" [*] SaleConsumer waiting for sale requests...");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Sale request received: {Message}", message);

                var saleRequest = JsonSerializer.Deserialize<SaleRequestDTO>(message);
                var firstProduct = saleRequest.Products.FirstOrDefault();

                var paymentRequest = new PaymentRequestDTO
                {
                    CustomerId = saleRequest.CustomerId,
                    ProductId = firstProduct?.ProductId ?? "unknown",
                    Quantity = firstProduct?.Quantity ?? 1,
                    TotalAmount = 100m,
                    SaleId = Guid.NewGuid().ToString()
                };

                var paymentJson = JsonSerializer.Serialize(paymentRequest);
                var paymentBody = Encoding.UTF8.GetBytes(paymentJson);

                await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "payment.requests", body: paymentBody);
                _logger.LogInformation("Payment request published for sale: {SaleId}", paymentRequest.SaleId);
            };
            await channel.BasicConsumeAsync("sale.requests", autoAck: true, consumer: consumer);

            var confirmedConsumer = new AsyncEventingBasicConsumer(channel);
            confirmedConsumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var paymentConfirmed = JsonSerializer.Deserialize<PaymentResponseDTO>(message);

                if (paymentConfirmed.Status == "Approved")
                {
                    await _saleRepository.CreateConfirmedSaleAsync(paymentConfirmed);
                    _logger.LogInformation("Sale persisted for PaymentId: {PaymentId}",
                        paymentConfirmed.PaymentId);
                }
                else
                {
                    _logger.LogWarning("Payment rejected: {PaymentId}", paymentConfirmed.PaymentId);
                }

                await Task.CompletedTask;
            };

            await channel.BasicConsumeAsync("payment.confirmed", autoAck: true, 
                consumer: confirmedConsumer);
        }
    }
    
}
