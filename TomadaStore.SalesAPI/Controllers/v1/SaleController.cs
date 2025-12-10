using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Models.DTOs.Sales;
using TomadaStore.SaleAPI.Services.Interfaces;
using TomadaStore.SalesAPI.Services.Interfaces;
using TomadaStore.SalesAPI.Services.v2;

namespace TomadaStore.SaleAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ILogger<SaleController> _logger;
        private readonly ISaleService _saleService;


        public SaleController(ILogger<SaleController> logger,
                                ISaleService saleService)
        {
            _logger = logger;
            _saleService = saleService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSaleAsync([FromBody] SaleRequestDTO request)
        {
            _logger.LogInformation("Creating sale for CustomerId: {CustomerId}", request.CustomerId);

            await _saleService.CreateSaleAsync(request.CustomerId, request.ProductId, request);

            return Ok("Sale created successfully!");
        }


    }
}