using System;

namespace OrderSystem.Domain.Exceptions;

public class UsernameAlreadyExistsException : ConflictException
{
    public UsernameAlreadyExistsException() : base("Username Already exists")
    {
    }
}
