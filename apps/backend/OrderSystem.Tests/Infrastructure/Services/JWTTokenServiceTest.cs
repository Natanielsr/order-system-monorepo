using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using OrderSystem.Application.Services;
using OrderSystem.Domain.Entities;
using OrderSystem.Infrastructure.Services;

namespace OrderSystem.Tests.Infrastructure.Services;

public class JWTTokenServiceTest
{
    private readonly Mock<IConfiguration> _configMock;
    private readonly JWTTokenService _tokenService;

    private readonly string _jwtKey = "super_secret_test_key_12345!@#$%";
    private readonly string _jwtIssuer = "OrderSystem.Tests";
    private readonly string _jwtAudience = "OrderSystem.Client";

    public JWTTokenServiceTest()
    {
        _configMock = new Mock<IConfiguration>();
        _configMock.Setup(c => c["Jwt:Key"]).Returns(_jwtKey);
        _configMock.Setup(c => c["Jwt:Issuer"]).Returns(_jwtIssuer);
        _configMock.Setup(c => c["Jwt:Audience"]).Returns(_jwtAudience);

        _tokenService = new JWTTokenService(_configMock.Object);
    }

    private User CreateValidUser()
    {
        return User.CreateUser(
            id: Guid.NewGuid(),
            username: "testuser",
            email: "test@example.com",
            hashedPassword: "hashed123",
            role: "User",
            phone: "1111111111"
        );
    }

    [Fact]
    public void GenerateToken_WithValidUser_ReturnsValidJwtToken()
    {
        // Arrange
        var user = CreateValidUser();

        // Act
        var token = _tokenService.GenerateToken(user);

        // Assert
        Assert.NotNull(token);
        Assert.False(string.IsNullOrWhiteSpace(token));

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)),
            ValidateIssuer = true,
            ValidIssuer = _jwtIssuer,
            ValidateAudience = true,
            ValidAudience = _jwtAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        Assert.Equal(_jwtIssuer, jwtToken.Issuer);
        Assert.Contains(_jwtAudience, jwtToken.Audiences);

        var claims = jwtToken.Claims.ToList();
        Assert.Contains(claims, c => c.Type == "nameid" && c.Value == user.Id.ToString());
        Assert.Contains(claims, c => c.Type == "unique_name" && c.Value == user.Username);
        Assert.Contains(claims, c => c.Type == "email" && c.Value == user.Email);
        Assert.Contains(claims, c => c.Type == "role" && c.Value == user.Role);

        Assert.True(jwtToken.ValidTo > DateTime.UtcNow);
        Assert.True(jwtToken.ValidTo <= DateTime.UtcNow.AddHours(2).AddSeconds(1));
    }

    [Fact]
    public void GenerateToken_WithNullUser_ThrowsNullReferenceException()
    {
        // Arrange
        User user = null!;

        // Act & Assert
        var exception = Assert.Throws<NullReferenceException>(() => _tokenService.GenerateToken(user));
        Assert.Equal("User is null", exception.Message);
    }

    [Fact]
    public void GenerateToken_WithEmptyUsername_ThrowsNullReferenceException()
    {
        // Arrange
        var user = User.CreateUser(
            id: Guid.NewGuid(),
            username: "",
            email: "test@example.com",
            hashedPassword: "hashed123",
            role: "User",
            phone: "1111111111"
        );

        // Act & Assert
        var exception = Assert.Throws<NullReferenceException>(() => _tokenService.GenerateToken(user));
        Assert.Equal("Username is null or empty", exception.Message);
    }

    [Fact]
    public void GenerateToken_WithNullEmail_ThrowsNullReferenceException()
    {
        // Arrange
        var user = User.CreateUser(
            id: Guid.NewGuid(),
            username: "testuser",
            email: "",
            hashedPassword: "hashed123",
            role: "User",
            phone: "1111111111"
        );

        // Act & Assert
        var exception = Assert.Throws<NullReferenceException>(() => _tokenService.GenerateToken(user));
        Assert.Equal("Email is null or empty", exception.Message);
    }

    [Fact]
    public void GenerateToken_WithEmptyRole_ThrowsNullReferenceException()
    {
        // Arrange
        var user = User.CreateUser(
            id: Guid.NewGuid(),
            username: "testuser",
            email: "test@example.com",
            hashedPassword: "hashed123",
            role: "",
            phone: "1111111111"
        );

        // Act & Assert
        var exception = Assert.Throws<NullReferenceException>(() => _tokenService.GenerateToken(user));
        Assert.Equal("Role is null or empty", exception.Message);
    }

    [Fact]
    public async Task GenerateToken_ProducesUniqueTokensForSameUser()
    {
        // Arrange
        var user = CreateValidUser();

        // Act
        var token1 = _tokenService.GenerateToken(user);
        await Task.Delay(1000); // Ensure iat differs
        var token2 = _tokenService.GenerateToken(user);

        // Assert
        Assert.NotEqual(token1, token2);
    }

    [Fact]
    public void GenerateToken_TokenHasCorrectStructure()
    {
        // Arrange
        var user = CreateValidUser();

        // Act
        var token = _tokenService.GenerateToken(user);

        // Assert
        var parts = token.Split('.');
        Assert.Equal(3, parts.Length);

        var header = JwtBase64UrlDecode<JwtHeader>(parts[0]);
        var payload = JwtBase64UrlDecode<JwtPayload>(parts[1]);

        Assert.Equal("HS256", header.Alg); // HmacSha256 algorithm identifier
        Assert.Equal("JWT", header.Typ);
        Assert.NotNull(payload.Iss);
        Assert.NotNull(payload.Aud);
    }

    private static T JwtBase64UrlDecode<T>(string base64Url) where T : class
    {
        var base64 = base64Url.Replace('-', '+').Replace('_', '/');
        var padded = base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
        var bytes = Convert.FromBase64String(padded);
        var json = Encoding.UTF8.GetString(bytes);
        return System.Text.Json.JsonSerializer.Deserialize<T>(json)!;
    }
}
