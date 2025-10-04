using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Dtos.Payment;
using OnlineStore.Models;
using OnlineStore.Repository;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IConfirmPayment _paymentService;
        public PaymentsController(IConfirmPayment paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPayments()
        {
            var result = await _paymentService.GetAllAsync();
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var result = await _paymentService.GetByIdAsync(id);
            return result == null ? NotFound("Payment not found") : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment([FromBody] PaymentWriteDto dtpayment)
        {
            var result = await _paymentService.AddAsync(dtpayment);
            return !result.Success ? NotFound(result.ErrorMessage) :
                CreatedAtAction(nameof(GetPaymentById), new { Id = result?.Data?.Id }, result?.Data);

        }

        [HttpPost("{id}/Confirm")]
        public async Task<IActionResult> ConfirmPayment(int id)
        {
            var result = await _paymentService.ConfirmPaymentAsync(id);
            if (!result.Success)
            {
                if (result.ErrorMessage.Contains("not found"))
                {
                    return NotFound(result.ErrorMessage);
                }
                else
                {
                    return BadRequest(result.ErrorMessage);
                }
            }

            return Ok(new { Message = "Payment successful and stock updated", Payment = result.Data });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] PaymentWriteDto dtpayment)
        {
            var result = await _paymentService.UpdateAsync(id, dtpayment);
            if (!result.Success)
            {
                if (result.ErrorMessage.Contains("not found"))
                {
                    return NotFound(result.ErrorMessage);
                }
                else
                {
                    return BadRequest(result.ErrorMessage);
                }
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var isDeleted = await _paymentService.DeleteAsync(id);
            return isDeleted ? NoContent() : NotFound("Payment not found");
        }

    }
}
