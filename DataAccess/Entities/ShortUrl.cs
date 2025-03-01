namespace DataAccess.Entities;

public class ShortUrl
{
    public int Id { get; set; }
    public string OriginalUrl { get; set; }
    public string ShortenedUrl { get; set; }
    public string UserId { get; set; } // FK to AppUser
    public DateTime CreatedAt { get; set; }
}
