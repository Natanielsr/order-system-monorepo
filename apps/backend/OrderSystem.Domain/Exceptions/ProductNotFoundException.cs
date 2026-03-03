using System;

namespace OrderSystem.Domain.Exceptions;

public class ProductNotFoundException : BadRequest
{
    public ProductNotFoundException() : base("Product Id in order doesn't exist") { }
}
