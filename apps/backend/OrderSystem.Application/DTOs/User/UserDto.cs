namespace OrderSystem.Application.DTOs.User;

public record class UserDto
{
    public required Guid Id { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Phone { get; init; }
    public required string Role { get; set; }

}
