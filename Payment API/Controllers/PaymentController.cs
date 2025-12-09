using Microsoft.AspNetCore.Mvc;
using Payment_API.Services.Interfaces;

[ApiController]
[Route("api/v1/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentConsumerService _consumerService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(IPaymentConsumerService consumerService, ILogger<PaymentController> logger)
    {
        _consumerService = consumerService;
        _logger = logger;
    }

    [HttpPost("start-consumer")]
    public async Task<IActionResult> StartConsumer()
    {
        await _consumerService.StartConsumeAsync();
        return Ok("Payment consumer started.");
    }
}