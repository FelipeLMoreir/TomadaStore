using Microsoft.AspNetCore.Mvc;

namespace TomadaStore.SalesAPI.Services.Interfaces
{
    public interface ISaleProducerService
    {
        Task CreateSaleRabbitAsync();
    }
}
