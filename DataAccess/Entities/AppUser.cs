using DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Entities;

    public class AppUser : IdentityUser
    {
        public Guid UserId { get; set; }
    }