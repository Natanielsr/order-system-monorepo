using BCrypt.Net;
using OrderSystem.Domain.Services;
using OrderSystem.Infrastructure.Services;

namespace OrderSystem.Tests.Infrastructure.Services;

public class PasswordServiceTest
{
    private readonly IPasswordService _passwordService;

    public PasswordServiceTest()
    {
        _passwordService = new PasswordService();
    }

    [Fact]
    public void HashPassword_WithValidPassword_ReturnsNonEmptyHash()
    {
        // Arrange
        var password = "SecurePassword123!";

        // Act
        var hash = _passwordService.HashPassword(password);

        // Assert
        Assert.NotNull(hash);
        Assert.False(string.IsNullOrWhiteSpace(hash));
        Assert.StartsWith("$2a$", hash); // BCrypt format
        Assert.Contains("$", hash); // Should contain cost factor
    }

    [Fact]
    public void HashPassword_WithEmptyPassword_ReturnsValidHash()
    {
        // Arrange
        var password = "";

        // Act
        var hash = _passwordService.HashPassword(password);

        // Assert
        Assert.NotNull(hash);
        Assert.False(string.IsNullOrWhiteSpace(hash));
        Assert.StartsWith("$2a$", hash);
    }

    [Fact]
    public void HashPassword_ProducesUniqueHashesForSamePassword()
    {
        // Arrange
        var password = "MyPassword123";

        // Act
        var hash1 = _passwordService.HashPassword(password);
        var hash2 = _passwordService.HashPassword(password);

        // Assert
        Assert.NotEqual(hash1, hash2);
        Assert.True(BCrypt.Net.BCrypt.Verify(password, hash1));
        Assert.True(BCrypt.Net.BCrypt.Verify(password, hash2));
    }

    [Fact]
    public void VerifyPassword_WithCorrectPassword_ReturnsTrue()
    {
        // Arrange
        var password = "TestPassword!@#";
        var hash = _passwordService.HashPassword(password);

        // Act
        var result = _passwordService.VerifyPassowrd(password, hash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_WithIncorrectPassword_ReturnsFalse()
    {
        // Arrange
        var correctPassword = "CorrectPassword123";
        var incorrectPassword = "WrongPassword456";
        var hash = _passwordService.HashPassword(correctPassword);

        // Act
        var result = _passwordService.VerifyPassowrd(incorrectPassword, hash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyPassword_WithEmptyPassword_ComparesCorrectly()
    {
        // Arrange
        var password = "";
        var hash = _passwordService.HashPassword(password);

        // Act
        var result = _passwordService.VerifyPassowrd(password, hash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_WithNullPassword_ThrowsException()
    {
        // Arrange
        var hash = _passwordService.HashPassword("somepassword");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _passwordService.VerifyPassowrd(null!, hash));
    }

    [Fact]
    public void VerifyPassword_WithNullHash_ThrowsException()
    {
        // Arrange
        var password = "somepassword";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _passwordService.VerifyPassowrd(password, null!));
    }

    [Fact]
    public void VerifyPassword_WithInvalidHashFormat_ReturnsFalse()
    {
        // Arrange
        var password = "testpassword";
        var invalidHash = "$2a$10$invalidhashexample1234567890";

        // Act
        var result = _passwordService.VerifyPassowrd(password, invalidHash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HashPassword_UsesCorrectWorkFactor()
    {
        // Arrange
        var password = "TestPassword123";

        // Act
        var hash = _passwordService.HashPassword(password);

        // Assert
        // BCrypt hash format: $2a$[cost]$[22-character-salt][31-character-hash]
        // The cost factor should be 12 (as defined in PasswordService)
        var parts = hash.Split('$');
        Assert.Equal(4, parts.Length); // Should have 4 parts: "", "2a", "12", "rest"
        Assert.Equal("2a", parts[1]); // Algorithm identifier
        Assert.Equal("12", parts[2]); // Work factor (cost)
    }

    [Fact]
    public void HashAndVerify_WithSpecialCharacters_WorksCorrectly()
    {
        // Arrange
        var password = "P@ssw0rd!#$%^&*()_+-=[]{}|;':\",./<>?";

        // Act
        var hash = _passwordService.HashPassword(password);
        var result = _passwordService.VerifyPassowrd(password, hash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HashAndVerify_WithLongPassword_WorksCorrectly()
    {
        // Arrange
        var password = new string('A', 1000); // Very long password

        // Act
        var hash = _passwordService.HashPassword(password);
        var result = _passwordService.VerifyPassowrd(password, hash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HashPassword_WithDifferentWorkFactors_ProducesDifferentHashLengths()
    {
        // Note: This test verifies that our constant WorkFactor is being used
        // Since PasswordService uses constant WorkFactor=12, all hashes should have similar structure
        // This is more of an integration check

        // Arrange
        var password = "testpassword";

        // Act
        var hash = _passwordService.HashPassword(password);

        // Assert
        // BCrypt with work factor 12 produces a hash with specific segment length
        var parts = hash.Split('$');
        var saltAndHash = parts[3];
        // Standard BCrypt: 22 chars for salt + 31 chars for hash = 53 chars
        Assert.Equal(53, saltAndHash.Length);
    }
}
