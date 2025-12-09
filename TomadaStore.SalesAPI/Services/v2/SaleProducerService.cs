using System.Linq;
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
        private readonly ISaleProducerRepository _producerRepository;
        private readonly ILogger<SaleProducerService> _logger;
        private readonly ISaleService _saleService;
        private static readonly HttpClient _httpClientCustomer;
        private static readonly HttpClient _httpClientProduct;

        public SaleProducerService(
            ISaleProducerRepository producerRepository,
            ISaleService saleService,
            ILogger<SaleProducerService> logger)
        {
            _producerRepository = producerRepository;
            _saleService = saleService;
            _logger = logger;
        }

        public async Task CreateSaleRabbitAsync(int idCustomer, string idProduct, SaleRequestDTO saleDTO)
        {
            try
            {
                var customerResp = await _httpClientCustomer.GetAsync($"/api/v1/customer/{idCustomer}");
                var customer = await customerResp.Content.ReadFromJsonAsync<CustomerResponseDTO>();

                decimal totalAmount = 0;
                foreach (var item in saleDTO.Products)
                {
                    var productResp = await _httpClientProduct.GetAsync($"/api/v1/product/{item.ProductId}");
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

                await _producerRepository.PublishSaleAsync(saleEvent);
                _logger.LogInformation("Sale published to queue. Total: {TotalAmount}", totalAmount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar venda");
                throw;
            }
        }
    }
}