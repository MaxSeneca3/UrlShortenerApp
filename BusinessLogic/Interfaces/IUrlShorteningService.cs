using BusinessLogic.Dtos;
using DataAccess.Entities;

namespace BusinessLogic.Interfaces
{
    public interface IUrlShorteningService
    {
        Task<Url> CreateOriginalUrlAsync(string originalUrl, string userId);
        Task<bool> UrlExists(string originalUrl);
        Task<ShortUrl> CreateShortUrl(CreateShortUrlDto dto, string userId, string username, string role);
        Task<IEnumerable<ShortUrl>> GetAllUrls();
        Task<ShortUrl> GetShortUrlById(int id);
        Task<bool> DeleteUrlAsync(int shortUrlId, string userId, bool isAdmin);
    }
}



