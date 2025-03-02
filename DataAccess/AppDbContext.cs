using DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<ShortUrl> ShortUrls { get; set; }
        public DbSet<Url> OriginalUrls { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure ShortUrl entity is properly configured
            modelBuilder.Entity<ShortUrl>()
                .ToTable("ShortUrls")
                .HasKey(s => s.Id);
            
            // Url entity
            modelBuilder.Entity<Url>()
                .HasKey(u => u.Id);  // Define the primary key explicitly
        }
    }
}