using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.Dtos.User;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterNewUser([FromBody] RegisterDto dtoUser)
        {
            var result = await _accountService.RegisterAsync(dtoUser);
            return !result.Success ? BadRequest(result.ErrorMessage) :Ok(new { Message = "User Registered Successfully"});
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dtoLogin)
        {
            var result = await _accountService.LoginAsync(dtoLogin);
            return !result.Success ? BadRequest(result.ErrorMessage) : Ok(result.Data);
        }

    }
}
