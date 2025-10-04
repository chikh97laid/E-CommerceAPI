using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Dtos.Shipping;
using OnlineStore.Models;
using OnlineStore.Repository;
using OnlineStore.Services.Implementations;
using OnlineStore.Services.Interfaces;
using System.Threading;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShippingsController : ControllerBase
    {
        private readonly IShippingService _shippingService;

        public ShippingsController(IShippingService shippingService)
        { 
            _shippingService = shippingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetShippings()
        {
            var result = await _shippingService.GetAllAsync();
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShippingById(int id)
        {
            var result = await _shippingService.GetByIdAsync(id);
            return result == null ? NotFound("Shipping not found") : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddShipping([FromBody] ShippingWriteDto dtShipping)
        {
            var result = await _shippingService.AddAsync(dtShipping);
            return !result.Success ? NotFound(result.ErrorMessage) :
                CreatedAtAction(nameof(GetShippingById), new { id = result?.Data?.Id }, result?.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShipping(int id, ShippingWriteDto dtShipping)
        {
            var result = await _shippingService.UpdateAsync(id, dtShipping);
            return !result.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }

        [HttpPatch("{id}/Status")]
        public async Task<IActionResult> UpdateShippingStatus(int id, enShippingStatus Status)
        {
            var result = await _shippingService.UpdateShipStatusAsync(id, Status);
            return !result.Success ? NotFound(result.ErrorMessage) :
                Ok(new {Message = "Status updated successfully", NewStatus = result?.Data?.ShippingStatus});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipping(int id)
        {
            var isDeleted = await _shippingService.DeleteAsync(id);
            return isDeleted ? NoContent() : NotFound("Shipping not found");
        }

    }
}
