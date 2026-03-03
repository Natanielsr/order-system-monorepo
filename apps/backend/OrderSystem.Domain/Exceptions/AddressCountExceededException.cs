using System;

namespace OrderSystem.Domain.Exceptions;

public class AddressCountExceededException : BadRequest
{
    public AddressCountExceededException() : base("The maximum number of addresses registered by the user is 5.")
    {
    }
}
