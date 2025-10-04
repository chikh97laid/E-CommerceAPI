using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Dtos.Customer;
using OnlineStore.Models;
using OnlineStore.Repository;
using OnlineStore.Services.Implementations;
using OnlineStore.Services.Interfaces;
using System.Diagnostics.Metrics;
using System.Net;
using System.Numerics;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly IService<CustomerReadDto, CustomerWriteDto> _customerService;  

        public CustomersController(IService<CustomerReadDto, CustomerWriteDto> customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var result = await _customerService.GetAllAsync();
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var result = await _customerService.GetByIdAsync(id);
            return result == null ? NotFound($"Customer with id = {id} doesn't exist") : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerWriteDto dtCustomer)
        {
            var result = await _customerService.AddAsync(dtCustomer);

            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetCustomerById), new { Id = result!.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerWriteDto dtCustomer)
        {
            var result = await _customerService.UpdateAsync(id, dtCustomer);

            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var isDeleted = await _customerService.DeleteAsync(id);
            return isDeleted ? NoContent() : NotFound($"Customer with id = {id} is not found");
        }


    }
}
