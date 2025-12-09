using MongoDB.Driver;
using TomadaStore.Models.DTOs.Category;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Payment;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Models.Models;
using TomadaStore.SaleAPI.Repository.Interfaces;
using TomadaStore.SalesAPI.Data;
using TomadaStore.SalesAPI.Repositories.Interfaces;

namespace TomadaStore.SaleAPI.Repository
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
                                            ProductResponseDTO productDTO,
                                            SaleRequestDTO sale)
        {
            try
            {
                var products = new List<Product>();

                var category = new Category
                (
                    productDTO.Category.Id,
                    productDTO.Category.Name,
                    productDTO.Category.Description
                );

                var product = new Product
                (
                    productDTO.Id,
                    productDTO.Name,
                    productDTO.Description,
                    productDTO.Price,
                    category
                );

                products.Add(product);

                var customer = new Customer
                (
                    customerDTO.Id,
                    customerDTO.FirstName,
                    customerDTO.LastName,
                    customerDTO.Email,
                    customerDTO.PhoneNumber
                );
                await _mongoCollection.InsertOneAsync(new Sale
                (
                    customer,
                    products,
                    productDTO.Price
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating sale: {ex.Message}");
                throw;
            }
        }

        public async Task<SaleResponseDTO> GetSaleByIdAsync(string id)
        {
            var sale = await _mongoCollection.Find(s => s.Id == id).FirstOrDefaultAsync();
            return sale?.ToResponseDTO(); 
        }

        public async Task CreateConfirmedSaleAsync(PaymentResponseDTO paymentConfirmed)
        {
            try
            {
                var mockCustomer = new CustomerResponseDTO
                {
                    Id = 1, 
                    FirstName = "Cliente",
                    LastName = "Pago",
                    Email = "cliente@pago.com",
                    PhoneNumber = "999999999"
                };

                var mockProduct = new ProductResponseDTO
                {
                    Id = "prod-pago",
                    Name = "Produto Confirmado",
                    Description = $"Produto pago via {paymentConfirmed.PaymentId}",
                    Price = paymentConfirmed.Amount,
                    Category = new CategoryResponseDTO 
                    {
                        Id = "1",
                        Name = "Geral",
                        Description = "Categoria padrão"
                    }
                };

                var saleRequest = new SaleRequestDTO
                {
                    CustomerId = 1,
                    Products = new List<SaleProductItemDTO>
            {
                new SaleProductItemDTO
                {
                    ProductId = "produto-pago",
                    Quantity = 1
                }
            }
                };

                await CreateSaleAsync(mockCustomer, mockProduct, saleRequest);

                _logger.LogInformation("Venda persistida no Mongo para PaymentId: {PaymentId}", 
                    paymentConfirmed.PaymentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao persistir venda confirmada: {PaymentId}", 
                    paymentConfirmed.PaymentId);
                throw;
            }
        }


    }
}