using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderSystem.Application.Services;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Infrastructure.Services;

public class JWTTokenService(IConfiguration config) : ITokenService
{
    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        if (user == null)
            throw new NullReferenceException("User is null");

        if (string.IsNullOrEmpty(user.Username))
            throw new NullReferenceException("Username is null or empty");

        if (string.IsNullOrEmpty(user.Email))
            throw new NullReferenceException("Email is null or empty");

        if (string.IsNullOrEmpty(user.Role))
            throw new NullReferenceException("Role is null or empty");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = config["Jwt:Issuer"],
            Audience = config["Jwt:Audience"],
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var stringToken = tokenHandler.WriteToken(securityToken);

        return stringToken;
    }
}

