using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Dtos.Role;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService; 

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] RoleDto roleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _roleService.AddRoleAsync(roleDto);
            return !result.Success ? BadRequest(result.ErrorMessage) : Ok(new { Message = "Role added successfully", roleDto.Name});
        }

        [HttpPost("SetUserRoles")]
        [Authorize(Roles = Role.RoleAdmin)]
        public async Task<IActionResult> SetUserRoles([FromBody] UserRoleDto usRolDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roleService.SetUserRolesAsync(usRolDto);
            if(!result.Success)
            {
                if(result.ErrorMessage.Contains("not found"))
                {
                    return NotFound(result.ErrorMessage);
                }

                return NotFound(result.ErrorMessage);
            }

            return Ok("Role(s) Added to user successfully");

        }

    }
}
