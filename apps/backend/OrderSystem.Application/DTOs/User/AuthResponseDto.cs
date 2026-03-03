namespace OrderSystem.Application.DTOs.User;

public record class AuthResponseDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }

    public string? Token { get; set; }
}
