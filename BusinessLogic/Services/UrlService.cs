using AutoMapper;
using BusinessLogic.Dtos;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;

namespace BusinessLogic.Services
{
    public class UrlService : IUrlShorteningService
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IMapper _mapper; // Inject IMapper

        public UrlService(IUrlRepository urlRepository, IMapper mapper)
        {
            _urlRepository = urlRepository;
            _mapper = mapper; // Initialize the mapper
        }

        public async Task<bool> UrlExists(string originalUrl)
        {
            return await _urlRepository.UrlExists(originalUrl);
        }

        public async Task<ShortUrl> CreateShortUrl(CreateShortUrlDto dto)
        {
            // Use AutoMapper to map the DTO to an entity
            var shortUrl = _mapper.Map<ShortUrl>(dto);
            shortUrl.ShortenedUrl = GenerateShortUrl();
            shortUrl.CreatedAt = DateTime.UtcNow;

            // Save the entity and return the mapped DTO
            var createdShortUrl = await _urlRepository.CreateShortUrl(shortUrl);
            return _mapper.Map<ShortUrl>(createdShortUrl); // Return the mapped DTO
        }

        public async Task<IEnumerable<ShortUrl>> GetAllUrls()
        {
            // Retrieve all URLs from the repository
            var urls = await _urlRepository.GetAllUrls();

            // Map entities to DTOs
            return _mapper.Map<IEnumerable<ShortUrl>>(urls);
        }

        public async Task<ShortUrl> GetShortUrlById(int id)
        {
            // Retrieve the URL by ID and map to DTO
            var shortUrl = await _urlRepository.GetShortUrlById(id);
            return _mapper.Map<ShortUrl>(shortUrl); // Return the mapped DTO
        }

        public async Task<bool> DeleteUrl(int id, string userId)
        {
            return await _urlRepository.DeleteUrl(id, userId);
        }

        public async Task<bool> DeleteUrlAsync(Guid shortUrlId, Guid userId, bool isAdmin)
        {
            return await _urlRepository.DeleteUrlAsync(shortUrlId, userId, isAdmin);
        }

        // Implementing GetOriginalUrlAsync method from the interface
        public async Task<string?> GetOriginalUrlAsync(string shortUrl)
        {
            return await _urlRepository.GetOriginalUrlAsync(shortUrl);
        }

        public async Task<string> ShortenUrlAsync(string originalUrl, Guid userId)
        {
            // Check if the URL already exists in the repository
            if (await UrlExists(originalUrl))
            {
                // Retrieve the existing shortened URL associated with the original URL
                var existingShortUrl = (await _urlRepository.GetAllUrls()) // Get all URLs
                    .FirstOrDefault(url => url.OriginalUrl == originalUrl); // Perform the filtering in memory

                // Return the existing shortened URL if found
                return existingShortUrl?.ShortenedUrl ?? throw new Exception("Short URL not found.");
            }

            // If URL doesn't exist, create a new short URL
            var dto = new CreateShortUrlDto
            {
                OriginalUrl = originalUrl,
                UserId = userId.ToString()
            };

            var shortUrl = await CreateShortUrl(dto);
            return shortUrl.ShortenedUrl;
        }

        private string GenerateShortUrl()
        {
            return Guid.NewGuid().ToString().Substring(0, 6); // Generates a short URL
        }
    }
}

