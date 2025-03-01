using DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Entities;

    public class AppUser : IdentityUser, IEntity<string>
    {
        public Guid UserId { get; set; }
    }