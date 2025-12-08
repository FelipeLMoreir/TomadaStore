using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TomadaStore.SaleAPI.Controllers.v1;
using TomadaStore.SaleAPI.Services.Interfaces;
using TomadaStore.SalesAPI.Services.Interfaces;
using TomadaStore.SalesAPI.Services.v2;

namespace TomadaStore.SalesAPI.Controllers.v2
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleProducerController : ControllerBase
    {
        
    }
}
