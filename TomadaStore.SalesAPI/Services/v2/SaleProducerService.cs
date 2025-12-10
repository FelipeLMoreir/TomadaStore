using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.SaleAPI.Services.Interfaces;
using TomadaStore.SalesAPI.Repositories.Interfaces;
using TomadaStore.SalesAPI.Services.Interfaces;

namespace TomadaStore.SalesAPI.Services.v2
{
    public class SaleProducerService : ISaleProducerService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SaleProducerService> _logger;
        private readonly ISaleService _saleService;
        private readonly ConnectionFactory _rabbitFactory;

        public SaleProducerService(
            IHttpClientFactory httpClientFactory,
            ISaleService saleService,
            ILogger<SaleProducerService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _saleService = saleService;
            _logger = logger;
            _rabbitFactory = new ConnectionFactory { HostName = "localhost" };
        }

        public async Task CreateSaleRabbitAsync(int idCustomer, string idProduct, SaleRequestDTO saleDTO)
        {
            try
            {
                var httpClientCustomer = _httpClientFactory.CreateClient("CustomerAPI");
                var httpClientProduct = _httpClientFactory.CreateClient("ProductAPI");

                var customerResp = await httpClientCustomer.GetAsync($"/api/v1/customer/{idCustomer}");
                var customer = await customerResp.Content.ReadFromJsonAsync<CustomerResponseDTO>();

                decimal totalAmount = 0;
                foreach (var item in saleDTO.Products)
                {
                    var productResp = await httpClientProduct.GetAsync($"/api/v1/product/{item.ProductId}");
                    var product = await productResp.Content.ReadFromJsonAsync<ProductResponseDTO>();
                    totalAmount += product.Price * item.Quantity;
                }

                var saleEvent = new
                {
                    CustomerId = customer.Id,
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    Products = saleDTO.Products,
                    TotalAmount = totalAmount,
                    SaleDate = DateTime.UtcNow,
                    SaleId = Guid.NewGuid().ToString()
                };

                await PublishSaleDirectAsync(saleEvent);

                _logger.LogInformation("Sale published to queue. Total: {TotalAmount}", totalAmount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao publicar venda");
                throw;
            }
        }

        public async Task PublishSaleDirectAsync(object saleEvent)
        {
            using var connection = await _rabbitFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "sale.requests", durable: false, exclusive: false, autoDelete: false);

            var json = JsonSerializer.Serialize(saleEvent);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: "sale.requests",
                body: body);

            _logger.LogInformation("Message sent to sale.requests");
        }

    }
}
