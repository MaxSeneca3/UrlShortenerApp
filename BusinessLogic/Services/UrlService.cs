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
        public async Task<Url> CreateOriginalUrlAsync(string originalUrl, string userId)
        {
            // Use the repository to create the original URL
            return await _urlRepository.CreateOriginalUrlAsync(originalUrl, userId);
        }

        public async Task<bool> UrlExists(string originalUrl)
        {
            return await _urlRepository.UrlExists(originalUrl);
        }

        public async Task<ShortUrl> CreateShortUrl(CreateShortUrlDto dto, string userId, string userName, string role)
        {
            // Generate the shortened URL (this can be done using the GenerateShortenedUrl method)
            var shortenedUrl = GenerateShortenedUrl(dto.OriginalUrl);

            var shortUrl = new ShortUrl
            {
                OriginalUrl = dto.OriginalUrl,
                ShortenedUrl = shortenedUrl, // Set the generated shortened URL
                UserId = userId,
                Username = userName, // Add username
                Role = role, // Add role
                CreatedAt = DateTime.UtcNow
            };

            // Save the short URL to the database
            await _urlRepository.CreateShortUrl(shortUrl);
            return shortUrl;
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

        public async Task<bool> DeleteUrlAsync(int id, string userId, bool isAdmin)
        {
            // Call the repository to delete the URL, passing the user ID and admin status
            return await _urlRepository.DeleteUrlAsync(id, userId, isAdmin);
        }

        // Implementing GetOriginalUrlAsync method from the interface
        public async Task<string?> GetOriginalUrlAsync(string shortUrl)
        {
            return await _urlRepository.GetOriginalUrlAsync(shortUrl);
        }

        public async Task<string> ShortenUrlAsync(string originalUrl, Guid userId, string username, string role)
        {
            // Check if the URL already exists in the repository
            if (await UrlExists(originalUrl))
            {
                // Retrieve existing shortened URL associated with the original URL
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

            var shortUrl = await CreateShortUrl(dto, userId.ToString(), username, role); // Pass user info to service
            return shortUrl.ShortenedUrl;
        }


        private string GenerateShortenedUrl(string originalUrl)
        {
            // Implement the logic to generate a shortened URL (e.g., hashing the URL or using a base62 encoding)
            var shortenedUrl = Guid.NewGuid().ToString("N").Substring(0, 8); // Example shortened URL logic
            return shortenedUrl;
        }
    }
}


