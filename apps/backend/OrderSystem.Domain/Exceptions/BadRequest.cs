using System;

namespace OrderSystem.Domain.Exceptions;

public class BadRequest : Exception
{
    public BadRequest(string message) : base(message) { }
}
