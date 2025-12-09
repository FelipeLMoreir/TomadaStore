using Microsoft.AspNetCore.Mvc;
using TomadaStore.SaleConsumer.Service.Interfaces;

namespace TomadaStore.SaleConsumer.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SaleConsumerController : ControllerBase
    {
        private readonly ILogger<SaleConsumerController> _logger;
        private readonly ISaleConsumerService _saleService;

        public SaleConsumerController(
            ILogger<SaleConsumerController> logger,
            ISaleConsumerService saleService)
        {
            _logger = logger;
            _saleService = saleService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> ConsumeSale()
        {
            try
            {
                await _saleService.StartConsumeAsync();
                return Ok("Sale consumption started.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming sales");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error consuming sales.");
            }
        }
    }
}
