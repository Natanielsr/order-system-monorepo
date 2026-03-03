using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Services;

namespace OrderSystem.Infrastructure.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext context, IPasswordService passwordService)
    {
        context.Database.EnsureCreated();

        if (context.Users.Any()) return; // Banco já tem dados

        var user = new User()
        {
            Id = Guid.Empty,
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Active = true,
            Phone = "(15)99730-7629",
            Username = "UserTest",
            Email = "usertest@email.com",
            HashedPassword = passwordService.HashPassword("password123"),
            Role = UserRole.User
        };

        var admin = new User()
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Active = true,
            Username = "admin",
            Email = "admin@email.com",
            HashedPassword = passwordService.HashPassword("admin"),
            Role = UserRole.Admin,
            Phone = "(15)99730-7629"
        };


        context.Users.Add(user);
        context.Users.Add(admin);
        context.SaveChanges();
        Console.WriteLine("UserTest and admin created in database.");
    }
}
