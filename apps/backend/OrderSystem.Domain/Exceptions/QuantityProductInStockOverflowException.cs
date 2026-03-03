using System;

namespace OrderSystem.Domain.Exceptions;

public class QuantityProductInStockOverflowException : BadRequest
{
    public QuantityProductInStockOverflowException() : base("quantity required greater than available")
    {

    }
}
