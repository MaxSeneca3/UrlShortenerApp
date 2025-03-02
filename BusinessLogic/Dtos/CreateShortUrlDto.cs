namespace BusinessLogic.Dtos;

public record CreateShortUrlDto
{
    public string OriginalUrl { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Role { get; set; }
}
