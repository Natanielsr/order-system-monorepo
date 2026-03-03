namespace OrderSystem.Application.Authorization;

public record class UserClaim
{
    public required string Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }

}
