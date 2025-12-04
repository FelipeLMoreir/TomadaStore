using MongoDB.Bson;
using MongoDB.Driver;
using TomadaStore.CustomerAPI.Data;
using TomadaStore.Models.DTOs.Category;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.Models;
using TomadaStore.ProductAPI.Repositories.Interfaces;

namespace TomadaStore.ProductAPI.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;
        private readonly IMongoCollection<Product> _mongoCollection;
        private readonly ConnectionDB _connection;
        public ProductRepository(ILogger<ProductRepository> logger, ConnectionDB connectionDB)
        {
            _logger = logger;
            _connection = connectionDB;
            _mongoCollection = _connection.GetMongoCollection();
        }

        public async Task CreateProductAsync(ProductRequestDTO productDTO)
        {
            try
            {
                await _mongoCollection.InsertOneAsync(new Product(
                    productDTO.Name,
                    productDTO.Description,
                    productDTO.Price,
                    new Category(
                        productDTO.Category.Name,
                        productDTO.Category.Description
                        )
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError("erro" + ex.Message);
                throw;
            }
        }

        public async Task<List<ProductResponseDTO>> GetAllProductsAsync()
        {
            try
            {
                var products = await _mongoCollection.Find(_ => true, null).ToListAsync();
                return products.Select(product => new ProductResponseDTO
                {
                    Id = product.Id.ToString(),
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Category = new CategoryResponseDTO
                    {
                        Id = product.Category.Id.ToString(),
                        Name = product.Category.Name,
                        Description = product.Category.Description
                    }
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("erro" + ex.Message);
                throw;
            }
        }

        public async Task<ProductResponseDTO> GetProductByIDAsync(ObjectId id)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
                var product = await _mongoCollection.Find(filter).FirstOrDefaultAsync();
                if (product == null)
                    return null;
                return new ProductResponseDTO
                {
                    Id = product.Id.ToString(),
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Category = new CategoryResponseDTO
                    {
                        Name = product.Category.Name,
                        Description = product.Category.Description
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("erro" + ex.Message);
                throw;
            }
        }

        public Task<ProductResponseDTO> GetProductByIDAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
