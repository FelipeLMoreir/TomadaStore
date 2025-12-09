using Payment_API.Repository.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TomadaStore.Models.DTOs.Payment;

namespace TomadaStore.PaymentAPI.Repositories
{
    public class PaymentProducerRepository : IPaymentProducerRepository
    {
        private readonly ILogger<PaymentProducerRepository> _logger;
        private readonly ConnectionFactory _factory;

        public PaymentProducerRepository(ILogger<PaymentProducerRepository> logger)
        {
            _logger = logger;
            _factory = new ConnectionFactory { HostName = "localhost" };
        }

        public async Task PublishPaymentConfirmedAsync(PaymentResponseDTO payment)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "payment.confirmed",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var json = JsonSerializer.Serialize(payment);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: "payment.confirmed",
                body: body);

            _logger.LogInformation("Payment confirmed published: {PaymentId}", payment.PaymentId);
        }
    }
}
