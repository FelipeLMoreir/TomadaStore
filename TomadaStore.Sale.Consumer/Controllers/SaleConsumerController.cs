using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TomadaStore.Sale.Consumer.Service;
using TomadaStore.Sale.Consumer.Service.Interfaces;

namespace TomadaStore.Sale.Consumer.Controllers
{
    [Route("api/v1[controller]")]
    [ApiController]
    public class SaleConsumerController : ControllerBase
    {
        private readonly ILogger<SaleConsumerController> _logger;
        private readonly ISaleConsumerService _saleService;
        private readonly ConnectionFactory _factory;


        public SaleConsumerController(ILogger<SaleConsumerController> logger)
        {
            _logger = logger;
            _saleService = new SaleConsumerService();
            _factory = new ConnectionFactory() { HostName = "localhost" };
        }

        [HttpPost]
        public async Task<IActionResult> ConsumeSale()
        {
            try
            {
                using var connection = await _factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();
                await channel.QueueDeclareAsync(queue: "saleList",
                                                durable: false,
                                                exclusive: false,
                                                autoDelete: false,
                                                arguments: null);
                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = System.Text.Encoding.UTF8.GetString(body);
                    _logger.LogInformation($" [x] Received {message}");
                    await Task.CompletedTask;
                };
                await channel.BasicConsumeAsync("saleList", autoAck: true,
                                                         consumer: consumer);
                return Ok("Sale consumption started.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while consuming sales: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error consuming sales.");
            }
        }
    }
}
