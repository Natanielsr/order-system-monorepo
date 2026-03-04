using OrderSystem.Application.Authorization;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Tests.Application.Authorization;

public class AuthorizationBaseTest
{
    [Fact]
    public void ValidGuid_WithValidGuid_ReturnsSuccess()
    {
        // Arrange
        var userClaim = new UserClaim
        {
            Id = "6f9619ff-8b86-d011-b42d-00c04fc964ff",
            Username = "testuser",
            Email = "test@example.com",
            Role = UserRole.User
        };

        // Act
        var result = AuthorizationBase.ValidGuid(userClaim);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Valid Guid", result.Message);
    }

    [Fact]
    public void ValidGuid_WithEmptyGuid_ReturnsFailure()
    {
        // Arrange
        var userClaim = new UserClaim
        {
            Id = "invalid-guid",
            Username = "testuser",
            Email = "test@example.com",
            Role = UserRole.User
        };

        // Act
        var result = AuthorizationBase.ValidGuid(userClaim);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("the authenticated id is not a valid guid", result.Message);
    }

    [Fact]
    public void ValidUser_WithMatchingUserId_ReturnsSuccess()
    {
        // Arrange
        var userId = Guid.Parse("6f9619ff-8b86-d011-b42d-00c04fc964ff");
        var userClaim = new UserClaim
        {
            Id = userId.ToString(),
            Username = "testuser",
            Email = "test@example.com",
            Role = UserRole.User
        };

        // Act
        var result = AuthorizationBase.ValidUser(userClaim, userId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("User access allowed", result.Message);
    }

    [Fact]
    public void ValidUser_WithNonMatchingUserId_ReturnsFailure()
    {
        // Arrange
        var userId = Guid.Parse("6f9619ff-8b86-d011-b42d-00c04fc964ff");
        var userClaim = new UserClaim
        {
            Id = Guid.NewGuid().ToString(),
            Username = "testuser",
            Email = "test@example.com",
            Role = UserRole.User
        };

        // Act
        var result = AuthorizationBase.ValidUser(userClaim, userId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("User access denied", result.Message);
    }

    [Fact]
    public void ValidUser_WithInvalidUserIdClaim_ReturnsFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userClaim = new UserClaim
        {
            Id = "invalid-guid-string",
            Username = "testuser",
            Email = "test@example.com",
            Role = UserRole.User
        };

        // Act
        var result = AuthorizationBase.ValidUser(userClaim, userId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("the authenticated id is not a valid guid", result.Message);
    }

    [Fact]
    public void GetGuid_WithValidString_ReturnsGuid()
    {
        // Arrange
        var idString = "6f9619ff-8b86-d011-b42d-00c04fc964ff";

        // Act
        var result = AuthorizationBase.getGuid(idString);

        // Assert
        Assert.Equal(Guid.Parse(idString), result);
        Assert.NotEqual(Guid.Empty, result);
    }

    [Fact]
    public void GetGuid_WithInvalidString_ReturnsEmptyGuid()
    {
        // Arrange
        var idString = "invalid-guid";

        // Act
        var result = AuthorizationBase.getGuid(idString);

        // Assert
        Assert.Equal(Guid.Empty, result);
    }
}
