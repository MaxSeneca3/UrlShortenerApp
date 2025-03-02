namespace BusinessLogic.Dtos;

public class OriginalUrlResponse
{
    public int Id { get; set; }
    public string OriginalUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Role { get; set; }
}