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

        public async Task<List<CustomerResponseDTO>> GetAllCustomersAsync()
        {
            try
            {
                var sqlSelect = @"SELECT Id, FirstName, LastName, Email, PhoneNumber, IsActive
                                  FROM Customers";
                var customers = await _connection.QueryAsync<CustomerResponseDTO>(sqlSelect);
                return customers.ToList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError("Sql error" + sqlEx.Message);
                throw new Exception(sqlEx.StackTrace);
            }
            catch (Exception ex)
            {
                _logger.LogError("Sql error" + ex.Message);
                throw new Exception(ex.StackTrace);
            }
        }

        public async Task<CustomerResponseDTO> GetCustomerByIdAsync(int id)
        {
            try
            {
                var sqlSelect = @"SELECT Id, FirstName, LastName, Email, PhoneNumber
                          FROM Customers 
                          WHERE Id = @Id";
                var customer = await _connection.QuerySingleOrDefaultAsync<CustomerResponseDTO>(sqlSelect, new { Id = id });

                if (customer == null)
                    return null;

                return customer;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while getting customer by id {Id}: {Message}", id, sqlEx.Message);
                throw new Exception(sqlEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting customer by id {Id}: {Message}", id, ex.Message);
                throw new Exception(ex.Message);
            }
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
        public async Task<bool> CustomerStatusAsync(int id)
        {
            try
            {
                var sqlToggle = @"UPDATE Customers 
                          SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END 
                          WHERE Id = @Id";

                var rowsAffected = await _connection.ExecuteAsync(sqlToggle, new { Id = id });
                return rowsAffected > 0;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while toggling customer status {Id}: {Message}", id, sqlEx.Message);
                throw new Exception(sqlEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while toggling customer status {Id}: {Message}", id, ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
