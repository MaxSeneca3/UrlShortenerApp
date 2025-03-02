using System.Security.Claims;
using BusinessLogic.Dtos;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortenerApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlController : ControllerBase
{
    private readonly IUrlShorteningService _urlService;

    public UrlController(IUrlShorteningService urlService)
    {
        _urlService = urlService;
    }

    [HttpPost("add-original-url")]
    public async Task<IActionResult> CreateOriginalUrl([FromBody] CreateOriginalUrlDto dto)
    {
        // Validate the original URL
        if (await _urlService.UrlExists(dto.OriginalUrl))
        {
            return BadRequest("This URL already exists.");
        }

        // Add the original URL
        var originalUrl = await _urlService.CreateOriginalUrlAsync(dto.OriginalUrl, dto.UserId);

        // Convert to response DTO
        var responseDto = new OriginalUrlResponse
        {
            Id = originalUrl.Id,
            OriginalUrl = originalUrl.OriginalUrl,
            CreatedDate = originalUrl.CreatedDate,
            UserId = originalUrl.UserId
        };

        return CreatedAtAction(nameof(CreateOriginalUrl), new { id = responseDto.Id }, responseDto);
    }
    
    //[Authorize] // Ensure user is authenticated
    [HttpPost]
    public async Task<IActionResult> CreateShortUrl([FromBody] CreateShortUrlDto dto)
    {
        // Check if the URL already exists
        if (await _urlService.UrlExists(dto.OriginalUrl))
        {
            return BadRequest("This URL already exists.");
        }

        // Create the shortened URL
        var shortUrl = await _urlService.CreateShortUrl(dto, dto.UserId, dto.UserName, dto.Role); // Pass user info to service
        return CreatedAtAction(nameof(GetShortUrlById), new { id = shortUrl.Id }, shortUrl);
    }


    
    [HttpGet]
    public async Task<IActionResult> GetAllUrls()
    {
        var urls = await _urlService.GetAllUrls();
        return Ok(urls);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetShortUrlById(int id)
    {
        var shortUrl = await _urlService.GetShortUrlById(id);
        if (shortUrl == null)
        {
            return NotFound();
        }
        return Ok(shortUrl);
    }
    
   // [Authorize] // Ensure user is authenticated
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUrlAsync(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Get logged-in user ID
        var isAdmin = User.IsInRole("Admin"); // Check if user is Admin

        // Retrieve the URL to delete
        var shortUrl = await _urlService.GetShortUrlById(id);
        if (shortUrl == null)
        {
            return NotFound("URL not found.");
        }

        // Admins can delete any URL, normal users can only delete their own URLs
        if (!isAdmin && shortUrl.UserId != userId)
        {
            return Forbid("You can only delete your own URLs.");
        }

        // Delete the URL through the service
        var result = await _urlService.DeleteUrlAsync(id, userId, isAdmin); // userId and isAdmin here
        if (!result)
        {
            return BadRequest("Failed to delete URL.");
        }

        return NoContent(); // 204 No Content
    }

}
