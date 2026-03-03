using System;

namespace OrderSystem.Domain.Exceptions;

public class AddProductOrderException : BadRequest
{
    public AddProductOrderException(string message) : base(message)
    {
    }
}
