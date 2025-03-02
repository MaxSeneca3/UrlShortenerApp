using BusinessLogic.Dtos;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Interfaces;

public interface IAuthService
{
    Task<string> AuthenticateAsync(LoginDto loginDto);
    Task<IdentityResult> RegisterAsync(RegisterDto registerDto);
}