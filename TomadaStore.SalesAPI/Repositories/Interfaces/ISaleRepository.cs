using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Models.Models;

namespace TomadaStore.SalesAPI.Repositories.Interfaces
{
    public interface ISaleRepository
    {
        Task CreateSaleAsync(CustomerResponseDTO customerDTO,
                                  List<ProductResponseDTO> productDTOs,
                                  decimal totalPrice);
    }
}
