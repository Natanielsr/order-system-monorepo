using System;
using OrderSystem.Domain.Services;

namespace OrderSystem.Infrastructure.Services;

public class PasswordService : IPasswordService
{
    // O 'workFactor' define quão lento/seguro o hash será. 12 é um bom padrão.
    private const int WorkFactor = 12;

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    public bool VerifyPassowrd(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
