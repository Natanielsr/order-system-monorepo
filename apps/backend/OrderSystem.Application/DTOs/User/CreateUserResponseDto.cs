namespace OrderSystem.Application.DTOs.User;

public record class CreateUserResponseDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
}
