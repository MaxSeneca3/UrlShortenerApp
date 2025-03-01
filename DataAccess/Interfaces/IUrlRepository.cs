using DataAccess.Entities;

namespace DataAccess.Interfaces
{
    public interface IUrlRepository
    {
        Task<bool> UrlExists(string originalUrl);
        Task<ShortUrl> CreateShortUrl(ShortUrl shortUrl);
        Task<IEnumerable<ShortUrl>> GetAllUrls();
        Task<ShortUrl> GetShortUrlById(int id);
        Task<bool> DeleteUrl(int id, string userId);
        Task<bool> DeleteUrlAsync(Guid shortUrlId, Guid userId, bool isAdmin);
        Task<string?> GetOriginalUrlAsync(string shortUrl);
    }
}
