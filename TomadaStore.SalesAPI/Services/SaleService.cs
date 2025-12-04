using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.SalesAPI.Repositories.Interfaces;
using TomadaStore.SalesAPI.Services.Interfaces;

namespace TomadaStore.SalesAPI.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ILogger<SaleService> _logger;
        private readonly HttpClient _httpClientProduct;
        private readonly HttpClient _httpClientCustomer;
        public SaleService(ISaleRepository saleRepository, ILogger<SaleService> logger, HttpClient httpProduct, HttpClient httpCustomer)
        {
            _saleRepository = saleRepository;
            _logger = logger;
            _httpClientProduct = httpProduct;
            _httpClientCustomer = httpCustomer;
        }

        public async Task CreateSaleAsync(int customerId, SaleRequestDTO saleDto)
        {
            try
            {
                var customerResponse = await _httpClientCustomer.GetAsync($"v1/customers/{customerId}");
                customerResponse.EnsureSuccessStatusCode();
                var customerDTO = 
                    await customerResponse.Content.ReadFromJsonAsync<CustomerResponseDTO>();

                var productTasks = saleDto.Items.Select(item =>
                    _httpClientProduct.GetAsync($"v1/products/{item.ProductId}"));

                var productResponses = await Task.WhenAll(productTasks);
                var productsDTO = new List<ProductResponseDTO>();

                foreach (var response in productResponses)
                {
                    response.EnsureSuccessStatusCode();
                    productsDTO.Add(await response.Content.ReadFromJsonAsync<ProductResponseDTO>());
                }

                decimal totalPrice = productsDTO.Sum(p => p.Price *
                    saleDto.Items.First(i => i.ProductId == p.Id).Quantity);

                await _saleRepository.CreateSaleAsync(customerDTO, productsDTO, totalPrice);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating sale: {ex.Message}");
                throw;
            }
        }
    }
}
