using TomadaStore.Models.DTOs.Sale;

namespace TomadaStore.SalesAPI.Services.Interfaces
{
    public interface ISaleService
    {
        Task CreateSaleAsync(int customerId, SaleRequestDTO saleDto);
    }
}
