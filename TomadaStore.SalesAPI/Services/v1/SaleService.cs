using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.SaleAPI.Repository.Interfaces;
using TomadaStore.SaleAPI.Services.Interfaces;

namespace TomadaStore.SaleAPI.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;

        private readonly ILogger<SaleService> _logger;

        private readonly HttpClient _httpClientProduct;

        private readonly HttpClient _httpClientCustomer;

        public SaleService(
        IHttpClientFactory httpClientFactory,  
        ISaleRepository saleRepository,
        ILogger<SaleService> logger)
        {
            _httpClientCustomer = httpClientFactory.CreateClient("CustomerAPI");
            _httpClientProduct = httpClientFactory.CreateClient("ProductAPI");
            _saleRepository = saleRepository;
            _logger = logger;
        }

        public async Task CreateSaleAsync(int idCustomer,
                                            string idProduct,
                                            SaleRequestDTO saleDTO)
        {
            try
            {
                var customer = await _httpClientCustomer
                                    .GetFromJsonAsync<CustomerResponseDTO>
                                    ($"/api/v1/customer/{idCustomer}");

                var product = await _httpClientProduct
                                    .GetFromJsonAsync<ProductResponseDTO>
                                    ($"/api/v1/product/{idProduct}");

                await _saleRepository.CreateSaleAsync(customer, product, saleDTO);

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating a sale: {ex.Message}");
                throw;
            }
        }
    }
}