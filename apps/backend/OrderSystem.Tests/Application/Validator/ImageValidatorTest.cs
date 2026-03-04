#nullable disable

using OrderSystem.Application.Validator;
using OrderSystem.Domain.Exceptions;

namespace OrderSystem.Tests.Application.Validator;

public class ImageValidatorTest
{
    [Fact]
    public void IsValidImage_ValidJpeg_ReturnsTrue()
    {
        // Arrange
        var stream = new MemoryStream(new byte[100]); // arquivo de 100 bytes < 5MB

        // Act
        var result = ImageValidator.IsValidImage(stream, "test.jpg", "image/jpeg");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidImage_ValidPng_ReturnsTrue()
    {
        // Arrange
        var stream = new MemoryStream(new byte[100]);

        // Act
        var result = ImageValidator.IsValidImage(stream, "test.png", "image/png");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidImage_ValidGif_ReturnsTrue()
    {
        // Arrange
        var stream = new MemoryStream(new byte[100]);

        // Act
        var result = ImageValidator.IsValidImage(stream, "test.gif", "image/gif");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidImage_ValidWebp_ReturnsTrue()
    {
        // Arrange
        var stream = new MemoryStream(new byte[100]);

        // Act
        var result = ImageValidator.IsValidImage(stream, "test.webp", "image/webp");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidImage_InvalidExtension_ReturnsFalse()
    {
        // Arrange
        var stream = new MemoryStream(new byte[100]);

        // Act
        var result = ImageValidator.IsValidImage(stream, "test.exe", "application/x-msdownload");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidImage_InvalidContentType_ReturnsFalse()
    {
        // Arrange
        var stream = new MemoryStream(new byte[100]);

        // Act
        var result = ImageValidator.IsValidImage(stream, "test.jpg", "text/plain");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidImage_FileTooLarge_ReturnsFalse()
    {
        // Arrange
        var largeData = new byte[6 * 1024 * 1024]; // 6MB
        var stream = new MemoryStream(largeData);

        // Act
        var result = ImageValidator.IsValidImage(stream, "test.jpg", "image/jpeg");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidImage_ExtensionCaseInsensitive()
    {
        // Arrange
        var stream = new MemoryStream(new byte[100]);

        // Act
        var result = ImageValidator.IsValidImage(stream, "test.JPG", "IMAGE/JPEG");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidImage_WithMultipleValidExtensions()
    {
        // Arrange
        var stream = new MemoryStream(new byte[100]);

        // Act & Assert - testar todas as extensões válidas
        Assert.True(ImageValidator.IsValidImage(stream, "test.jpg", "image/jpeg"));
        Assert.True(ImageValidator.IsValidImage(stream, "test.jpeg", "image/jpeg"));
        Assert.True(ImageValidator.IsValidImage(stream, "test.png", "image/png"));
        Assert.True(ImageValidator.IsValidImage(stream, "test.gif", "image/gif"));
        Assert.True(ImageValidator.IsValidImage(stream, "test.webp", "image/webp"));
    }

    [Fact]
    public void IsValidImage_WithAllInvalidExtensions()
    {
        // Arrange
        var stream = new MemoryStream(new byte[100]);

        // Act & Assert - testar extensões inválidas
        Assert.False(ImageValidator.IsValidImage(stream, "test.txt", "text/plain"));
        Assert.False(ImageValidator.IsValidImage(stream, "test.pdf", "application/pdf"));
        Assert.False(ImageValidator.IsValidImage(stream, "test.exe", "application/x-msdownload"));
    }

    [Fact]
    public void IsValidImage_WithAllExtensionsAndMimeTypes()
    {
        // Arrange
        var stream = new MemoryStream(new byte[100]);

        // Act & Assert - teste que cada extensão com seu mime type correto
        Assert.True(ImageValidator.IsValidImage(stream, "test.jpg", "image/jpeg"));
        Assert.True(ImageValidator.IsValidImage(stream, "test.jpeg", "image/jpeg"));
        Assert.True(ImageValidator.IsValidImage(stream, "test.png", "image/png"));
        Assert.True(ImageValidator.IsValidImage(stream, "test.gif", "image/gif"));
        Assert.True(ImageValidator.IsValidImage(stream, "test.webp", "image/webp"));
    }
}
