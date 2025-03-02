namespace DataAccess.Entities;

public class ShortUrl
{
    public int Id { get; set; }
    public string OriginalUrl { get; set; }
    public string ShortenedUrl { get; set; }
    public string UserId { get; set; }  // User identifier
    public string Username { get; set; }  // Username of the creator
    public string Role { get; set; }  // Role of the user (Admin or User)
    public DateTime CreatedAt { get; set; }
}

