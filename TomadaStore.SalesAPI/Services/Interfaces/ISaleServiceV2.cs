using Microsoft.AspNetCore.Mvc;

namespace TomadaStore.SalesAPI.Services.Interfaces
{
    public interface ISaleServiceV2
    {
        Task CreateSaleRabbitAsync();
    }
}
