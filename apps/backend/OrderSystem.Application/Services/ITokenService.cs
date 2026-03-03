using System;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Application.Services;

public interface ITokenService
{
    public string GenerateToken(User user);
}
