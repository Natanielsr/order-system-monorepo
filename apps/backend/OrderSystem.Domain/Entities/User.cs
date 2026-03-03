using System;
using OrderSystem.Domain.Services;

namespace OrderSystem.Domain.Entities;

public static class UserRole
{
    public const string User = "User";
    public const string Admin = "Admin";
}

public class User : Entity
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string HashedPassword { get; init; }

    public required string Role { get; init; }

    public List<Address>? Addresses { get; init; }

    public string Phone { get; set; } = string.Empty;

    public User() { }

    public static User CreateUser(
        string username,
        string email,
        string hashedPassword,
        string role,
        string phone
    )
    {
        return new User()
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Active = true,
            Username = username,
            Email = email,
            HashedPassword = hashedPassword,
            Role = role,
            Phone = phone
        };
    }

}
