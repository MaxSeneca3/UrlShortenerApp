using BusinessLogic.Dtos;
using DataAccess.Entities;

namespace BusinessLogic.Interfaces
{
    public interface IUrlShorteningService
    {
        Task<bool> UrlExists(string originalUrl);
        Task<ShortUrl> CreateShortUrl(CreateShortUrlDto dto);
        Task<IEnumerable<ShortUrl>> GetAllUrls();
        Task<ShortUrl> GetShortUrlById(int id);
        Task<bool> DeleteUrl(int id, string userId);
    }
}



