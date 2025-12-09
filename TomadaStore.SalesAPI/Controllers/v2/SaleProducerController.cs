using Microsoft.AspNetCore.Mvc;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.SalesAPI.Services.Interfaces;

namespace TomadaStore.SalesAPI.Controllers.v2
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleProducerController : ControllerBase
    {
        private readonly ISaleProducerService _producerService;
        private readonly ILogger<SaleProducerController> _logger;

        public SaleProducerController(
            ISaleProducerService producerService,
            ILogger<SaleProducerController> logger)
        {
            _producerService = producerService;
            _logger = logger;
        }

        [HttpPost("customer/{idCustomer}/product/{idProduct}")]
        public async Task<IActionResult> PublishSaleAsync(
            int idCustomer,
            string idProduct,
            [FromBody] SaleRequestDTO saleDTO)
        {
            _logger.LogInformation("Publicando venda na fila...");

            await _producerService.CreateSaleRabbitAsync(idCustomer, idProduct, saleDTO);

            return Accepted("Sale request published to queue.");
        }
    }
}