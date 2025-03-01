namespace BusinessLogic.Interfaces;

public interface IUrlShorteningService
{
    Task<string> ShortenUrlAsync(string originalUrl, Guid userId);
    Task<string?> GetOriginalUrlAsync(string shortCode);
    //Task<IEnumerable<ShortUrlDto>> GetAllUrlsAsync();
    Task<bool> DeleteUrlAsync(Guid urlId, Guid userId, bool isAdmin);
}
