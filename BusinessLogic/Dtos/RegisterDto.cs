namespace BusinessLogic.Dtos;

public record RegisterDto
{
    public string Username { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "User"; // Default role is "User"
}
