using BusinessLogic.Dtos;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenService _tokenService;

        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, TokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                UserName = registerDto.Username.Trim(),
                Email = registerDto.Email,
                UserId = registerDto.UserId  // Add userId 
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                return result;

            if (!await _roleManager.RoleExistsAsync(registerDto.Role))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(registerDto.Role));
                if (!roleResult.Succeeded)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Failed to create role" });
                }
            }

            await _userManager.AddToRoleAsync(user, registerDto.Role);

            return IdentityResult.Success;
        }

        public async Task<string?> AuthenticateAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return null;

            var userRoles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateJwtToken(user, userRoles);

            return token;
        }
    }
}




