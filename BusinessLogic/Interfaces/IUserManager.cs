using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Interfaces;

public interface IUserManager
{
    Task<IdentityResult> CreateUserAsync(AppUser user, string password);
}