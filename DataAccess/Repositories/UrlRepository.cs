using AutoMapper;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper; // Add the IMapper instance

        public UrlRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper; // Initialize the mapper
        }

        public async Task<bool> UrlExists(string originalUrl)
        {
            return await _context.ShortUrls.AnyAsync(url => url.OriginalUrl == originalUrl);
        }

        public async Task<ShortUrl> CreateShortUrl(ShortUrl shortUrl)
        {
            _context.ShortUrls.Add(shortUrl);
            await _context.SaveChangesAsync();
            return shortUrl;
        }

        public async Task<IEnumerable<ShortUrl>> GetAllUrls()
        {
            return await _context.ShortUrls.ToListAsync();
        }

        public async Task<ShortUrl> GetShortUrlById(int id)
        {
            return await _context.ShortUrls.FindAsync(id);
        }

        public async Task<bool> DeleteUrl(int id, string userId)
        {
            var rowsAffected = await _context.ShortUrls
                .Where(url => url.Id == id && (url.UserId == userId || IsAdmin(userId)))
                .ExecuteDeleteAsync(); // Executes delete on the database level directly

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteUrlAsync(Guid shortUrlId, Guid userId, bool isAdmin)
        {
            var rowsAffected = await _context.ShortUrls
                .Where(url => url.ShortenedUrl == shortUrlId.ToString() && (url.UserId == userId.ToString() || isAdmin))
                .ExecuteDeleteAsync();

            return rowsAffected > 0;
        }

        public async Task<string?> GetOriginalUrlAsync(string shortUrl)
        {
            var url = await _context.ShortUrls.FirstOrDefaultAsync(u => u.ShortenedUrl == shortUrl);
            return url?.OriginalUrl; // Returns the original URL if found, otherwise null
        }

        // Method to map ShortUrl entity to DTO
        public async Task<ShortUrl> GetShortUrlDtoById(int id)
        {
            var shortUrl = await _context.ShortUrls.FindAsync(id);
            return _mapper.Map<ShortUrl>(shortUrl); // Map entity to DTO
        }

        private bool IsAdmin(string userId)
        {
            return true; // Example: admin logic could be based on roles or user ID
        }
    }
}

