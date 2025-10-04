using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Dtos.Role;
using OnlineStore.Services.Results;

namespace OnlineStore.Services.Interfaces
{
    public interface IRoleService
    {
        Task<ServiceResult<IdentityResult?>> AddRoleAsync(RoleDto roleDto);
        Task<ServiceResult<IdentityResult?>> SetUserRolesAsync(UserRoleDto usRolDto);
    }
}
