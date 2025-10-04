using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Dtos.Review;
using OnlineStore.Models;
using OnlineStore.Repository;
using OnlineStore.Services.Implementations;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {

        private readonly IService<ReviewReadDto, ReviewWriteDto> _reviewService;
        public ReviewsController(IService<ReviewReadDto, ReviewWriteDto> reviewService)
        {
            _reviewService = reviewService;
        }


        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            var result = await _reviewService.GetAllAsync();
            return result == null ? NotFound() : Ok(result);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var result = await _reviewService.GetByIdAsync(id);
            return result == null ? NotFound("Review not found") : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] ReviewWriteDto dtReview)
        {
            var result = await _reviewService.AddAsync(dtReview);

            return !result.Success ? NotFound(result.ErrorMessage) :
                CreatedAtAction(nameof(GetReviewById), new { Id = result?.Data?.Id }, result?.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewWriteDto dtReview)
        {
            var result = await _reviewService.UpdateAsync(id, dtReview);
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
        public async Task<IActionResult> DeleteReview(int id)
        {
            var isDeleted = await _reviewService.DeleteAsync(id);
            return isDeleted ? NoContent() : NotFound("Review not found");
        }

    }
}
