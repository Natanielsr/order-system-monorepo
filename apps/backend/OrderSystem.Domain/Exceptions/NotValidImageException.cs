using System;

namespace OrderSystem.Domain.Exceptions;

public class NotValidImageException : BadRequest
{
    public NotValidImageException() : base("Not valid image file")
    {
    }
}
