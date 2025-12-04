using Microsoft.AspNetCore.Mvc;
using TomadaStore.CustomerAPI.Services;
using TomadaStore.CustomerAPI.Services.Interfaces;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.Models;

namespace TomadaStore.CustomerAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;
        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            this._logger = logger;
            this._customerService = customerService;
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer([FromBody] CustomerRequestDTO customer)
        {
            try
            {
                await _customerService.InsertCustomerAsync(customer);

                return Created();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while creating customer" + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerResponseDTO>>> GetAllCustomers()
        {
            try
            {
                var customers = await _customerService.GetAllCustomersAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while getting all customers" + ex.Message);
                return Problem(ex.StackTrace);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponseDTO>> GetCustomerById(int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
                if (customer == null)
                    return NotFound($"Customer with id {id} not found");

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting customer by id {Id}", id);
                return Problem(ex.Message);
            }
        }

        [HttpPatch("{id}/modify-status")]
        public async Task<ActionResult> CustomerStatus(int id)
        {
            try
            {
                var success = await _customerService.CustomerStatusAsync(id);
                if (!success)
                    return NotFound($"Customer with id {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while toggling customer status {Id}", id);
                return Problem(ex.Message);
            }
        }
    }
}
