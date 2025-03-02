namespace DataAccess.Entities;

public class Url
{
    public int Id { get; set; }
    public string OriginalUrl { get; set; }
    public string UserId { get; set; } 
    public DateTime CreatedDate { get; set; }
}