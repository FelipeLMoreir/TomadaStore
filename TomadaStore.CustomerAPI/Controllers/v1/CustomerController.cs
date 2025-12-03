using Microsoft.AspNetCore.Mvc;
using TomadaStore.CustomerAPI.Services;
using TomadaStore.CustomerAPI.Services.Interfaces;
using TomadaStore.Models.DTOs;
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
    }
}
