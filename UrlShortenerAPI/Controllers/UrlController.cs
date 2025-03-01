using BusinessLogic.Dtos;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortenerApp.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UrlController : ControllerBase
{
    private readonly IUrlShorteningService _urlService;

    public UrlController(IUrlShorteningService urlService)
    {
        _urlService = urlService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateShortUrl([FromBody] CreateShortUrlDto dto)
    {
        if (await _urlService.UrlExists(dto.OriginalUrl))
        {
            return BadRequest("This URL already exists.");
        }

        var shortUrl = await _urlService.CreateShortUrl(dto);
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUrl(int id)
    {
        var result = await _urlService.DeleteUrl(id, User.Identity.Name);
        if (!result)
        {
            return Unauthorized();
        }
        return NoContent();
    }
}
