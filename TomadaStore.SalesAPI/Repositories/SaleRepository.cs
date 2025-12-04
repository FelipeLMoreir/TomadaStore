using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.SalesAPI.Repositories.Interfaces;
using TomadaStore.SalesAPI.Services.Interfaces;

namespace TomadaStore.SalesAPI.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        public async Task CreateSaleAsync(CustomerResponseDTO customer, ProductResponseDTO product, SaleRequestDTO saleDto)
        {
            throw new NotImplementedException();
        }
    }
}
