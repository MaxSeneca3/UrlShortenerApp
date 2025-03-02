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
    
    // Short Urls Table View
    [HttpPost("Add-short-url")]
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
    
    // Short Url Info
    [HttpGet("ShortUrlInfo/{shortUrlId}")]
    public async Task<IActionResult> GetShortUrlInfo(int shortUrlId)
    {
        var shortUrl = await _urlService.GetShortUrlById(shortUrlId);

        if (shortUrl == null)
        {
            return NotFound("Short URL not found.");
        }

        return Ok(shortUrl); // Return the Short URL details
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUrlAsync(int id)
    {
        // Retrieve the URL to delete
        var shortUrl = await _urlService.GetShortUrlById(id);
        if (shortUrl == null)
        {
            return NotFound("URL not found.");
        }

        // If we don't need role-based checks, proceed to delete the URL directly
        var result = await _urlService.DeleteUrlAsync(id);
        if (!result)
        {
            return BadRequest("Failed to delete URL.");
        }

        return NoContent(); // 204 No Content
    }
}
