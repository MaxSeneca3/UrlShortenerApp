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
            return await _context.OriginalUrls.AnyAsync(url => url.OriginalUrl == originalUrl); // Use _context.OriginalUrls
        }

        public async Task<ShortUrl> CreateShortUrl(ShortUrl shortUrl)
        {
            _context.ShortUrls.Add(shortUrl);  // Add the ShortUrl to the context
            await _context.SaveChangesAsync();  // Save the changes to the database
            return shortUrl;  // Return the created ShortUrl
        }

        public async Task<IEnumerable<ShortUrl>> GetAllUrls()
        {
            return await _context.ShortUrls.ToListAsync();
        }

        public async Task<ShortUrl> GetShortUrlById(int id)
        {
            return await _context.ShortUrls.FindAsync(id);
        }
        public async Task<bool> DeleteUrlAsync(int shortUrlId)
        {
            var rowsAffected = await _context.ShortUrls
                .Where(url => url.Id == shortUrlId) 
                .ExecuteDeleteAsync();

            return rowsAffected > 0;
        }

        public async Task<string?> GetOriginalUrlAsync(string shortUrl)
        {
            var url = await _context.ShortUrls.FirstOrDefaultAsync(u => u.ShortenedUrl == shortUrl);
            return url?.OriginalUrl; // Returns the original URL if found, otherwise null
        }
        
    }
}




