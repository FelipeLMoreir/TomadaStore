using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.SalesAPI.Services.Interfaces;

namespace TomadaStore.SalesAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
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
        public async Task<ActionResult> CreateSaleAsync([FromBody] SaleRequestDTO saleDto)
        {
            _logger.LogInformation("Creating a new sale with {Count} products...", saleDto.Items.Count);

            try
            {
                await _saleService.CreateSaleAsync(saleDto.CustomerId, saleDto);
                return Ok("Venda criada com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating sale");
                return BadRequest(ex.Message);
            }
        }
    }
}
