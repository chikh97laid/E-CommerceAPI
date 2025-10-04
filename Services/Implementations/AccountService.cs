using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.Dtos.User;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;
using OnlineStore.Services.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineStore.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountService(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager; 
            _configuration = configuration;
        }

        public async Task<ServiceResult<IdentityResult?>> RegisterAsync(RegisterDto dto)
        {
            var appUser = new AppUser()
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(appUser, dto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return ServiceResult<IdentityResult?>.Fail(string.Join(", ", errors));
            }

            return ServiceResult<IdentityResult?>.Ok(result);
        }

        public async Task<ServiceResult<TokenResponse?>> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserNameOrEmail);
            user ??= await _userManager.FindByEmailAsync(dto.UserNameOrEmail);

            if (user == null)
            {
                return ServiceResult<TokenResponse?>.Fail("UserName/Email is invalid!");
            }

            if (await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user!.UserName!));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!));
                var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    claims: claims,
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: sc
                );

                var _token = new TokenResponse()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    UserId = user.Id
                };
                
                return ServiceResult<TokenResponse?>.Ok(_token);

            }
            else
            {
                return ServiceResult<TokenResponse?>.Fail("Unauthorized");
            }

        }

    }
}
