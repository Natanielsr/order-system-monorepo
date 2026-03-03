using System;

namespace OrderSystem.Domain.Exceptions;

public class UserNotFoundException : BadRequest
{
    public UserNotFoundException() : base("User Not Found")
    {
    }
}
