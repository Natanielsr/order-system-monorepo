using System;

namespace OrderSystem.Domain.Services;

public interface IPasswordService
{

    public string HashPassword(string password);
    public bool VerifyPassowrd(string password, string hashedPassword);

}
