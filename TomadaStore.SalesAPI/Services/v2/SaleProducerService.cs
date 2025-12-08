using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TomadaStore.SaleAPI.Repository.Interfaces;
using TomadaStore.SaleAPI.Services;
using TomadaStore.SalesAPI.Services.Interfaces;

namespace TomadaStore.SalesAPI.Services.v2
{
    public class SaleProducerService : ISaleProducerService
    {
        private readonly ISaleRepository _saleRepository;

        private readonly ILogger<SaleProducerService> _logger;

        private readonly HttpClient _httpClientProduct;

        private readonly HttpClient _httpClientCustomer;

        public SaleProducerService(ISaleRepository saleRepository,
                            ILogger<SaleProducerService> logger,
                            HttpClient httpProduct,
                            HttpClient httpCustomer)
        {
            _saleRepository = saleRepository;
            _logger = logger;
            _httpClientProduct = httpProduct;
            _httpClientCustomer = httpCustomer;
        }

        public async Task CreateSaleRabbitAsync()
        {
            try
            {
                var factory = new ConnectionFactory { HostName = "localhost" };
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();
                await channel.QueueDeclareAsync(queue: "saleList",
                                                durable: false,
                                                exclusive: false,
                                                autoDelete: false,
                                                arguments: null);
                _logger.LogInformation(" [*] Waiting for messages.");
                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = System.Text.Encoding.UTF8.GetString(body);
                    _logger.LogInformation($" [x] Received {message}");
                    return Task.CompletedTask;
                };
                await channel.BasicConsumeAsync("saleList", autoAck: true,
                                                         consumer: consumer);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating a sale: {ex.Message}");
                throw;
            }
        }
    }
}
