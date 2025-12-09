using System.Linq;
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
                var firstProduct = saleDTO.Products.FirstOrDefault();
                var quantity = firstProduct?.Quantity ?? 1;

                var saleEvent = new
                {
                    CustomerId = idCustomer,
                    ProductId = idProduct,
                    Quantity = quantity,  
                    SaleDate = DateTime.UtcNow
                };

                await _producerRepository.PublishSaleAsync(saleEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao publicar venda no RabbitMQ");
                throw;
            }
        }
    }
}