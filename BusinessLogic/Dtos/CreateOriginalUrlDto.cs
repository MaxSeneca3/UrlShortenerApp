namespace BusinessLogic.Dtos;

public record CreateOriginalUrlDto
{
    public string OriginalUrl { get; set; }
    public string UserId { get; set; }
}