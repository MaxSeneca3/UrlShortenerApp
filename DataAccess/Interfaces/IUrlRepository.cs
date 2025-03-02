using DataAccess.Entities;

namespace DataAccess.Interfaces
{
    public interface IUrlRepository
    {
        Task<Url> CreateOriginalUrlAsync(string originalUrl, string userId);
        Task<bool> UrlExists(string originalUrl);
        Task<ShortUrl> CreateShortUrl(ShortUrl shortUrl);
        Task<IEnumerable<ShortUrl>> GetAllUrls();
        Task<ShortUrl> GetShortUrlById(int id);
        //Task<ShortUrl> GetUserByIdAsync(string userId);
        Task<bool> DeleteUrlAsync(int shortUrlId, string userId, bool isAdmin); // Keep only this one
        Task<string?> GetOriginalUrlAsync(string shortUrl);
    }
}
