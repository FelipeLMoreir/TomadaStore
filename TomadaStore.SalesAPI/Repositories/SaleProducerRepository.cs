using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TomadaStore.SalesAPI.Repositories.Interfaces;

namespace TomadaStore.SalesAPI.Repositories
{
    public class SaleProducerRepository : ISaleProducerRepository
    {
        private readonly ILogger<SaleProducerRepository> _logger;
        private readonly ConnectionFactory _factory;

        public SaleProducerRepository(ILogger<SaleProducerRepository> logger)
        {
            _logger = logger;
            _factory = new ConnectionFactory { HostName = "localhost" };
        }

        public async Task PublishSaleAsync(object saleEvent)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "sale.requests",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var json = JsonSerializer.Serialize(saleEvent);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: "sale.requests",
                body: body);

            _logger.LogInformation("Sent sale request: {Message}", json);
        }
    }
}