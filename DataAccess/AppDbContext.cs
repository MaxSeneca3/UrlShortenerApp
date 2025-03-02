using DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Add DbSet for ShortUrl
        public DbSet<ShortUrl> ShortUrls { get; set; }
        public DbSet<Url> OriginalUrls { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ShortUrl entity mapping if necessary
            modelBuilder.Entity<ShortUrl>()
                .ToTable("ShortUrls") // Ensures it maps to the correct table name
                .HasKey(s => s.Id);  // Ensures id is used as primary key if not automatically detected
        }
    }
}