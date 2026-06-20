using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Dtos.Category;
using OnlineStore.Dtos.Item;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly IitemService _itemService;
        private readonly IService<CategoryReadDto, CategoryWriteDto> _categoryService;

        public ItemsController(IitemService itemService, IService<CategoryReadDto, CategoryWriteDto> categoryService)
        {
            _itemService = itemService;
            _categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetItems()
        {
            var result = await _itemService.GetAllAsync();
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetItemById(int id)
        {
            var result = await _itemService.GetByIdAsync(id);
            return result == null ? NotFound($"Item with id = {id} doesn't exist") : Ok(result);
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetItemImage(int id)
        {
            var result = await _itemService.GetItemImageAsync(id);

            return result != null ? File(result, "image/png") : NotFound("Image Not Found!");
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([FromForm] ItemWriteDto dtitem)
        {
            var result = await _itemService.AddAsync(dtitem);

            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetItemById), new { Id = result!.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem([FromRoute] int id, [FromForm] ItemWriteDto dtitem)
        {
            var result = await _itemService.UpdateAsync(id, dtitem);

            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int id)
        {
            var isDeleted = await _itemService.DeleteAsync(id);
            return isDeleted ? NoContent() : NotFound($"Item with id = {id} is not found");
        }
    }
}
