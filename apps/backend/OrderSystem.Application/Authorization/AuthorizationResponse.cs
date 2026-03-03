namespace OrderSystem.Application.Authorization;

public record class AuthorizationResponse
{
    public bool Success { get; set; }
    public required string Message { get; set; }
}
