using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Dtos.User;
using OnlineStore.Services.Results;

namespace OnlineStore.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResult<IdentityResult?>> RegisterAsync(RegisterDto dto);
        Task<ServiceResult<TokenResponse?>> LoginAsync(LoginDto dto);
    }
}
