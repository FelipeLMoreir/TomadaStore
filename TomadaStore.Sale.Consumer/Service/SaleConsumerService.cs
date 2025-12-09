using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Payment;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Models.DTOs.Sales;
using TomadaStore.SaleAPI.Repository.Interfaces;
using TomadaStore.SaleConsumer.Service.Interfaces;

namespace TomadaStore.SaleConsumer.Service
{
    public class SaleConsumerService : ISaleConsumerService
    {
        private readonly ILogger<SaleConsumerService> _logger;
        private readonly ConnectionFactory _factory;
        private readonly ISaleRepository _saleRepository;
        private readonly HttpClient _httpClientCustomer;
        private readonly HttpClient _httpClientProduct;

        public SaleConsumerService(
            ILogger<SaleConsumerService> logger,
            IHttpClientFactory httpClientFactory,  
            ISaleRepository saleRepository)
        {
            _logger = logger;
            _httpClientCustomer = httpClientFactory.CreateClient("CustomerAPI");
            _httpClientProduct = httpClientFactory.CreateClient("ProductAPI");
            _saleRepository = saleRepository;
            _factory = new ConnectionFactory { HostName = "localhost" };
        }

        public async Task StartConsumeAsync()
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "sale.requests", durable: false, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueDeclareAsync(queue: "payment.requests", durable: false, exclusive: false, autoDelete: false, arguments: null);

            await channel.QueueDeclareAsync(queue: "approved-sales.queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            _logger.LogInformation("🚀 SaleConsumer: Listening sale.requests + approved-sales.queue");

            var saleConsumer = new AsyncEventingBasicConsumer(channel);
            saleConsumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("📥 Sale request received: {Message}", message);

                var saleRequest = JsonSerializer.Deserialize<SaleRequestDTO>(message);
                var firstProduct = saleRequest.Products.FirstOrDefault();

                var paymentRequest = new PaymentRequestDTO
                {
                    CustomerId = saleRequest.CustomerId,
                    ProductId = firstProduct?.ProductId ?? "unknown",
                    Quantity = firstProduct?.Quantity ?? 1,
                    TotalAmount = 1000, 
                    SaleId = Guid.NewGuid().ToString()
                };

                var paymentJson = JsonSerializer.Serialize(paymentRequest);
                var paymentBody = Encoding.UTF8.GetBytes(paymentJson);

                await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "payment.requests", body: paymentBody);
                _logger.LogInformation("➡️ Payment request published: {SaleId}", paymentRequest.SaleId);
            };
            await channel.BasicConsumeAsync("sale.requests", autoAck: true, consumer: saleConsumer);

            var approvedConsumer = new AsyncEventingBasicConsumer(channel);
            approvedConsumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("✅ Approved sale received: {Message}", message);

                var approvedSale = JsonSerializer.Deserialize<ApprovedSaleEventDTO>(message);

                try
                {
                    var customerResp = await _httpClientCustomer.GetAsync($"/api/v1/customer/{approvedSale.CustomerId}");
                    if (!customerResp.IsSuccessStatusCode)
                    {
                        _logger.LogError("❌ CustomerAPI error for ID: {CustomerId}", approvedSale.CustomerId);
                        return;
                    }
                    var customer = await customerResp.Content.ReadFromJsonAsync<CustomerResponseDTO>();

                    var products = new List<ProductResponseDTO>();
                    foreach (var item in approvedSale.Products)
                    {
                        var productResp = await _httpClientProduct.GetAsync($"/api/v1/product/{item.ProductId}");
                        if (productResp.IsSuccessStatusCode)
                        {
                            var product = await productResp.Content.ReadFromJsonAsync<ProductResponseDTO>();
                            products.Add(product);
                        }
                    }

                    var saleRequest = new SaleRequestDTO
                    {
                        CustomerId = approvedSale.CustomerId,
                        Products = approvedSale.Products
                    };

                    await _saleRepository.CreateSaleAsync(customer, products.FirstOrDefault(), saleRequest);

                    _logger.LogInformation("💾 Sale SAVED to MongoDB: {SaleId} | Total: {TotalAmount}",
                        approvedSale.SaleId, approvedSale.TotalAmount);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Error saving approved sale: {SaleId}", approvedSale.SaleId);
                }
            };

            await channel.BasicConsumeAsync("approved-sales.queue", autoAck: true, consumer: approvedConsumer);

            _logger.LogInformation("🎉 All consumers started!");
        }

    }

}
