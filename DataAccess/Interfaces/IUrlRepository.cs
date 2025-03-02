using DataAccess.Entities;

namespace DataAccess.Interfaces
{
    public interface IUrlRepository
    {
        Task<bool> UrlExists(string originalUrl);
        Task<ShortUrl> CreateShortUrl(ShortUrl shortUrl);
        Task<IEnumerable<ShortUrl>> GetAllUrls();
        Task<ShortUrl> GetShortUrlById(int id);
        Task<bool> DeleteUrlAsync(int shortUrlId); 
        Task<string?> GetOriginalUrlAsync(string shortUrl);
    }
}
