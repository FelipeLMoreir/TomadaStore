using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.SalesAPI.Services.Interfaces;

namespace TomadaStore.SalesAPI.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ILogger<SaleController> _logger;
        private readonly ISaleService _saleService;

        public SaleController(ILogger<SaleController> logger, ISaleService saleService)
        {
            _logger = logger;
            _saleService = saleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSaleAsync(int idCustomer, string idProduct, [FromBody] SaleRequestDTO saleDto)
        {
            _logger.LogInformation("Creating a new sale...");

            await _saleService.CreateSaleAsync(idCustomer, idProduct, saleDto);
            return Ok();
        }
    }
}
