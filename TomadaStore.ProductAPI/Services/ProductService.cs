using TomadaStore.Models.DTOs.Product;
using TomadaStore.ProductAPI.Repositories.Interfaces;
using TomadaStore.ProductAPI.Services.Interfaces;

namespace TomadaStore.ProductAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IProductRepository _productRepository;
        public ProductService(ILogger<ProductService> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }
        public async Task CreateProductAsync(ProductRequestDTO productDto)
        {
            try
            {
                await _productRepository.CreateProductAsync(productDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating productDto: {ex.Message}");
                throw;
            }
        }

        public Task DeleteProductAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductResponseDTO>> GetProductAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProductResponseDTO> GetProductByIDAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(string id, ProductRequestDTO productDto)
        {
            throw new NotImplementedException();
        }
    }
}
