using MongoDB.Driver;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Models.Models;
using TomadaStore.SalesAPI.Data;
using TomadaStore.SalesAPI.Repositories.Interfaces;
using TomadaStore.SalesAPI.Services.Interfaces;

namespace TomadaStore.SalesAPI.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ILogger<SaleRepository> _logger;
        private readonly IMongoCollection<Sale> _mongoCollection;
        private readonly ConnectionDB _connection;

        public SaleRepository(ILogger<SaleRepository> logger,
                                ConnectionDB connection)
        {
            _logger = logger;
            _connection = connection;
            _mongoCollection = _connection.GetMongoCollection();
        }
        public async Task CreateSaleAsync(CustomerResponseDTO customerDTO,
                                  List<ProductResponseDTO> productDTOs,
                                  decimal totalPrice)
        {
            try
            {
                var products = new List<Product>();

                foreach (var productDTO in productDTOs)
                {
                    var category = new Category(
                        productDTO.Category.Id,
                        productDTO.Category.Name,
                        productDTO.Category.Description
                    );

                    var product = new Product(
                        productDTO.Id,
                        productDTO.Name,
                        productDTO.Description,
                        productDTO.Price,
                        category
                    );
                    products.Add(product);
                }

                var customer = new Customer(
                    customerDTO.Id,
                    customerDTO.FirstName,
                    customerDTO.LastName,
                    customerDTO.Email,
                    customerDTO.PhoneNumber
                );

                await _mongoCollection.InsertOneAsync(new Sale(customer, products, totalPrice));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating sale: {ex.Message}");
                throw;
            }
        }
    }
}
