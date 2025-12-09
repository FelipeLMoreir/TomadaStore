using TomadaStore.Models.DTOs.Sale;

namespace TomadaStore.SalesAPI.Services.Interfaces
{
    public interface ISaleProducerService
    {
        Task CreateSaleRabbitAsync(int idCustomer, string idProduct, SaleRequestDTO saleDTO);
    }
}
