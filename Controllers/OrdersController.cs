using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Dtos.Order;
using OnlineStore.Models;
using OnlineStore.Repository;
using OnlineStore.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var result = await _orderService.GetAllAsync();
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _orderService.GetByIdAsync(id);
            return result == null ? NotFound("Order not found") : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderWriteDto dtOrder)
        {
            var result = await _orderService.AddAsync(dtOrder);

            return !result.Success ? NotFound(result.ErrorMessage) :
                CreatedAtAction(nameof(GetOrderById), new {Id = result?.Data?.Id}, result?.Data);
        }

        [HttpPost("{id}/Cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var result = await _orderService.CancelOrder(id);
           
            if(!result.Success)
            {
                if(result.ErrorMessage == "Order not found")
                {
                    return NotFound(result.ErrorMessage);
                }

                return BadRequest(result.ErrorMessage);
            }

            return Ok(new { Message = "Order cancelled successfully", result.Data });        
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderWriteDto dtOrder)
        {
            var result = await _orderService.UpdateAsync(id, dtOrder);

            return !result.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var isDeleted = await _orderService.DeleteAsync(id);
            return isDeleted ? NoContent() : NotFound("Order not found");
        }

    }
}
