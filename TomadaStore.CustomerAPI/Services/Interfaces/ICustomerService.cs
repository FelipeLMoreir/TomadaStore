using TomadaStore.Models.DTOs;
using TomadaStore.Models.Models;

namespace TomadaStore.CustomerAPI.Services.Interfaces
{
    public interface ICustomerService
    {
        Task InsertCustomerAsync(CustomerRequestDTO customer);
        Task<List<CustomerResponseDTO>> GetAllCustomersAsync();
        Task<CustomerResponseDTO> GetCustomerByIdAsync(int id);
    }
}