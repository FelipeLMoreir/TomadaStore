using TomadaStore.Models.DTOs;
using TomadaStore.Models.Models;

namespace TomadaStore.CustomerAPI.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        Task InsertCustomerAsync(CustomerRequestDTO customer);
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
    }
}