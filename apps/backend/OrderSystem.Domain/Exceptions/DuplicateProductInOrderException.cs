using System;

namespace OrderSystem.Domain.Exceptions;

public class DuplicateProductInOrderException : BadRequest
{
    public DuplicateProductInOrderException() : base("Duplicate Product In Order") { }
}
