using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Dtos.Review;
using OnlineStore.Dtos.Role;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;
using OnlineStore.Services.Results;

namespace OnlineStore.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        private string? ValidateResult(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return string.Join(", ", errors);
            }

            return null;
        }

        public async Task<ServiceResult<IdentityResult?>> AddRoleAsync(RoleDto roleDto)
        {

            if (await _roleManager.Roles.AnyAsync(r => r.Name == roleDto.Name))
            {
                return ServiceResult<IdentityResult?>.Fail($"Role '{roleDto.Name}' already exists!");
            }

            var role = new IdentityRole()
            {
                Name = roleDto.Name
            };

            var result = await _roleManager.CreateAsync(role);

            var validation = ValidateResult(result);
            if (validation != null) return ServiceResult<IdentityResult?>.Fail(validation);

            return ServiceResult<IdentityResult?>.Ok(result);
        }

        public async Task<ServiceResult<IdentityResult?>> SetUserRolesAsync(UserRoleDto usRolDto)
        {

            var user = await _userManager.FindByIdAsync(usRolDto.UserId);

            if (user == null)
            {
                return ServiceResult<IdentityResult?>.Fail("User not found");
            }

            var stringRolesList = usRolDto.Roles.Select(x => x.ToString()).ToList();

            var existingRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var invalidRoles = stringRolesList.Where(r => !existingRoles.Contains(r)).ToList();

            if (invalidRoles.Any())
            {
                return ServiceResult<IdentityResult?>.Fail($"Invalid Role: {string.Join(", ", invalidRoles)}");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var newRoles = stringRolesList.Except(currentRoles).ToList();
            if (!newRoles.Any())
            {
                return ServiceResult<IdentityResult?>.Fail("User already has all specified roles");
            }

            var result = await _userManager.AddToRolesAsync(user, stringRolesList);

            var validation = ValidateResult(result);
            if (validation != null) return ServiceResult<IdentityResult?>.Fail(validation);

            return ServiceResult<IdentityResult?>.Ok(result);
        }
    }
}
