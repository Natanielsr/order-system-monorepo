#nullable disable

using OrderSystem.Domain.Exceptions;

namespace OrderSystem.Tests.Domain.Exceptions;

public class AddProductOrderExceptionTest
{
    [Fact]
    public void Constructor_WithMessage_SetsMessageCorrectly()
    {
        // Arrange
        var expectedMessage = "productOrder cant be null";

        // Act
        var exception = new AddProductOrderException(expectedMessage);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithMessage_InheritsFromBadRequest()
    {
        // Arrange & Act
        var exception = new AddProductOrderException("test message");

        // Assert
        Assert.IsAssignableFrom<BadRequest>(exception);
    }

    [Fact]
    public void Constructor_WithMessage_InheritsFromException()
    {
        // Arrange & Act
        var exception = new AddProductOrderException("test message");

        // Assert
        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Theory]
    [InlineData("productOrder cant be null")]
    [InlineData("productOrder quantity must be bigger then zero")]
    [InlineData("productOrder UnitPrice must be bigger then zero")]
    [InlineData("ProductId already exists in productOrder")]
    public void Constructor_WithDifferentMessages_SetsMessageCorrectly(string message)
    {
        // Arrange & Act
        var exception = new AddProductOrderException(message);

        // Assert
        Assert.Equal(message, exception.Message);
    }
}
