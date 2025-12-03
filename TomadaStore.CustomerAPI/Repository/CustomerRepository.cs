using Dapper;
using Microsoft.Data.SqlClient;
using TomadaStore.CustomerAPI.Data;
using TomadaStore.CustomerAPI.Repository.Interfaces;
using TomadaStore.Models.DTOs;
using TomadaStore.Models.Models;

namespace TomadaStore.CustomerAPI.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ILogger<CustomerRepository> _logger;
        private readonly SqlConnection _connection;

        public CustomerRepository(ILogger<CustomerRepository> logger, 
            ConnectionDB connectionDB)
        {
            _logger = logger;
            _connection = connectionDB.GetConnection();
        }

        public Task<List<Customer>> GetAllCustomersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetCustomerByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task InsertCustomerAsync(CustomerRequestDTO customer)
        {
            try
            {
                var insertSql = "INSERT INTO Customers (FirstName, LastName, Email, PhoneNumber) " +
                                "VALUES (@FirstName, @LastName, @Email, @PhoneNumber)";
                await _connection.ExecuteAsync(insertSql, new { customer.FirstName,
                                                                customer.LastName,
                                                                customer.Email,
                                                                customer.PhoneNumber});
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while inserting customer: " + sqlEx.StackTrace);
                throw new Exception(sqlEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while inserting customer: " + ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }
    }
}
