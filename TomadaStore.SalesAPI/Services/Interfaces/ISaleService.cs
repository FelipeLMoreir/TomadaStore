using TomadaStore.Models.DTOs.Sale;

namespace TomadaStore.SalesAPI.Services.Interfaces
{
    public interface ISaleService
    {
        Task CreateSaleAsync(int idCustomer, string idProduct, SaleRequestDTO saleDto);
    }
}
