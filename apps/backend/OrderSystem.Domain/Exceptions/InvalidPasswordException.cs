using System;

namespace OrderSystem.Domain.Exceptions;

public class InvalidPasswordException : BadRequest
{
    public InvalidPasswordException() : base("Invalid Password")
    {
    }
}
