using System;

namespace OrderSystem.Domain.Exceptions;

public class EmailAlreadyExistsException : ConflictException
{
    public EmailAlreadyExistsException() : base("Email Already Exists")
    {
    }
}
