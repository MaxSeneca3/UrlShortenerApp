using AutoMapper;
using BusinessLogic.Services;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Moq;
using Shouldly;
using Xunit;

namespace UnitTests.ServiceTests
{
    public class UrlServiceTests
    {
        private readonly Mock<IUrlRepository> _urlRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UrlService _urlService;

        public UrlServiceTests()
        {
            // Set up mocks for dependencies
            _urlRepositoryMock = new Mock<IUrlRepository>();
            _mapperMock = new Mock<IMapper>();

            // Create UrlService instance
            _urlService = new UrlService(_urlRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task UrlExists_ShouldReturnTrue_WhenUrlExists()
        {
            // Arrange
            var originalUrl = "http://example.com";
            _urlRepositoryMock.Setup(repo => repo.UrlExists(originalUrl)).ReturnsAsync(true);

            // Act
            var result = await _urlService.UrlExists(originalUrl);

            // Assert
            result.ShouldBe(true);
        }

        [Fact]
        public async Task UrlExists_ShouldReturnFalse_WhenUrlDoesNotExist()
        {
            // Arrange
            var originalUrl = "http://example.com";
            _urlRepositoryMock.Setup(repo => repo.UrlExists(originalUrl)).ReturnsAsync(false);

            // Act
            var result = await _urlService.UrlExists(originalUrl);

            // Assert
            result.ShouldBe(false);
        }
        

        [Fact]
        public async Task GetAllUrls_ShouldReturnUrls_WhenUrlsExist()
        {
            // Arrange
            var urls = new List<ShortUrl>
            {
                new ShortUrl { Id = 1, OriginalUrl = "http://example.com", ShortenedUrl = "abc123" }
            };
            _urlRepositoryMock.Setup(repo => repo.GetAllUrls()).ReturnsAsync(urls);
            _mapperMock.Setup(m => m.Map<IEnumerable<ShortUrl>>(It.IsAny<IEnumerable<ShortUrl>>()))
                       .Returns(urls);

            // Act
            var result = await _urlService.GetAllUrls();

            // Assert
            result.ShouldNotBeNull();
            result.ShouldHaveSingleItem();
            result.First().OriginalUrl.ShouldBe("http://example.com");
        }

        [Fact]
        public async Task GetShortUrlById_ShouldReturnShortUrl_WhenUrlExists()
        {
            // Arrange
            var id = 1;
            var shortUrl = new ShortUrl { Id = id, OriginalUrl = "http://example.com", ShortenedUrl = "abc123" };
            _urlRepositoryMock.Setup(repo => repo.GetShortUrlById(id)).ReturnsAsync(shortUrl);
            _mapperMock.Setup(m => m.Map<ShortUrl>(It.IsAny<ShortUrl>())).Returns(shortUrl);

            // Act
            var result = await _urlService.GetShortUrlById(id);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(id);
            result.OriginalUrl.ShouldBe("http://example.com");
        }

        [Fact]
        public async Task DeleteUrlAsync_ShouldReturnTrue_WhenUrlIsDeleted()
        {
            // Arrange
            var id = 1;
            _urlRepositoryMock.Setup(repo => repo.DeleteUrlAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _urlService.DeleteUrlAsync(id);

            // Assert
            result.ShouldBe(true);
        }

        [Fact]
        public async Task DeleteUrlAsync_ShouldReturnFalse_WhenUrlDeletionFails()
        {
            // Arrange
            var id = 1;
            _urlRepositoryMock.Setup(repo => repo.DeleteUrlAsync(id)).ReturnsAsync(false);

            // Act
            var result = await _urlService.DeleteUrlAsync(id);

            // Assert
            result.ShouldBe(false);
        }

        [Fact]
        public async Task GetOriginalUrlAsync_ShouldReturnOriginalUrl_WhenShortUrlExists()
        {
            // Arrange
            var shortUrl = "abc123";
            var originalUrl = "http://example.com";
            _urlRepositoryMock.Setup(repo => repo.GetOriginalUrlAsync(shortUrl)).ReturnsAsync(originalUrl);

            // Act
            var result = await _urlService.GetOriginalUrlAsync(shortUrl);

            // Assert
            result.ShouldBe(originalUrl);
        }
        

        [Fact]
        public async Task ShortenUrlAsync_ShouldReturnExistingShortenedUrl_WhenUrlExists()
        {
            // Arrange
            var originalUrl = "http://existing-url.com";
            var userId = Guid.NewGuid();
            var userName = "user";
            var role = "User";
            var existingShortUrl = "existing123";

            _urlRepositoryMock.Setup(repo => repo.UrlExists(originalUrl)).ReturnsAsync(true);
            _urlRepositoryMock.Setup(repo => repo.GetAllUrls()).ReturnsAsync(new List<ShortUrl>
            {
                new ShortUrl { OriginalUrl = originalUrl, ShortenedUrl = existingShortUrl }
            });

            // Act
            var result = await _urlService.ShortenUrlAsync(originalUrl, userId, userName, role);

            // Assert
            result.ShouldBe(existingShortUrl);
        }
    }
}
