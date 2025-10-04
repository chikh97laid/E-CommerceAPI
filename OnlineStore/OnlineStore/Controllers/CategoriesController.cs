using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using OnlineStore.Repository;
using OnlineStore.Dtos;
using OnlineStore.Dtos.Category;
using AutoMapper;
using OnlineStore.Services.Interfaces;
using OnlineStore.Services.Implementations;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IService<CategoryReadDto, CategoryWriteDto> _categoryService;
        
        public CategoriesController(IService<CategoryReadDto, CategoryWriteDto> catService)
        {
            _categoryService = catService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _categoryService.GetAllAsync();
            return result == null ? NotFound() : Ok(result);              
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCateggoryById(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            return result == null ? NotFound($"Category with id = {id} is not found") : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryWriteDto dtCategory)
        {
            var result = await _categoryService.AddAsync(dtCategory);

            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetCateggoryById), new { Id = result!.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryWriteDto dtCategory)
        {
            var result = await _categoryService.UpdateAsync(id, dtCategory);

            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var isDeleted = await _categoryService.DeleteAsync(id);

            return isDeleted ? NoContent() : NotFound($"Category with id = {id} is not found");
        }

    }
}
