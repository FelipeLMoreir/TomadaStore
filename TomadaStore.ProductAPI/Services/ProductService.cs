using MongoDB.Bson;
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

        public Task DeleteProductAsync(ObjectId id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductResponseDTO>> GetProductAsync()
        {
            try
            {
                return await _productRepository.GetAllProductsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving products: {ex.Message}");
                throw;
            }
        }

        public async Task<ProductResponseDTO> GetProductByIDAsync(ObjectId id)
        {
            try
            {
                return await _productRepository.GetProductByIDAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving product by ID: {ex.Message}");
                throw;
            }
        }

        public Task UpdateProductAsync(ObjectId id, ProductRequestDTO productDto)
        {
            throw new NotImplementedException();
        }
    }
}
