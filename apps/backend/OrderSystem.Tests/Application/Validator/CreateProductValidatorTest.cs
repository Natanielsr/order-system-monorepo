#nullable disable

using FluentValidation.TestHelper;
using OrderSystem.Application.Validator;
using OrderSystem.Application.Products.Commands.CreateProduct;

namespace OrderSystem.Tests.Application.Validator;

public class CreateProductValidatorTest
{
    private readonly CreateProductValidator _validator;

    public CreateProductValidatorTest()
    {
        _validator = new CreateProductValidator();
    }

    [Fact]
    public void Validate_ValidCommand_ShouldPass()
    {
        // Arrange
        var fileStream = new MemoryStream(new byte[100]);
        var command = new CreateProductCommand(
            "Valid Product",
            29.99m,
            50,
            fileStream,
            "product.jpg",
            "image/jpeg"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyName_ShouldHaveError()
    {
        // Arrange
        var fileStream = new MemoryStream(new byte[100]);
        var command = new CreateProductCommand(
            "",
            29.99m,
            50,
            fileStream,
            "product.jpg",
            "image/jpeg"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public void Validate_NameTooShort_ShouldHaveError()
    {
        // Arrange
        var fileStream = new MemoryStream(new byte[100]);
        var command = new CreateProductCommand(
            "A",
            29.99m,
            50,
            fileStream,
            "product.jpg",
            "image/jpeg"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public void Validate_NameTooLong_ShouldHaveError()
    {
        // Arrange
        var fileStream = new MemoryStream(new byte[100]);
        var longName = new string('a', 101);
        var command = new CreateProductCommand(
            longName,
            29.99m,
            50,
            fileStream,
            "product.jpg",
            "image/jpeg"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public void Validate_ZeroPrice_ShouldHaveError()
    {
        // Arrange
        var fileStream = new MemoryStream(new byte[100]);
        var command = new CreateProductCommand(
            "Valid Product",
            0,
            50,
            fileStream,
            "product.jpg",
            "image/jpeg"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Price);
    }

    [Fact]
    public void Validate_NegativePrice_ShouldHaveError()
    {
        // Arrange
        var fileStream = new MemoryStream(new byte[100]);
        var command = new CreateProductCommand(
            "Valid Product",
            -10,
            50,
            fileStream,
            "product.jpg",
            "image/jpeg"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Price);
    }

    [Fact]
    public void Validate_NegativeQuantity_ShouldHaveError()
    {
        // Arrange
        var fileStream = new MemoryStream(new byte[100]);
        var command = new CreateProductCommand(
            "Valid Product",
            29.99m,
            -1,
            fileStream,
            "product.jpg",
            "image/jpeg"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.AvailableQuantity);
    }

    [Fact]
    public void Validate_EmptyFileName_ShouldHaveError()
    {
        // Arrange
        var fileStream = new MemoryStream(new byte[100]);
        var command = new CreateProductCommand(
            "Valid Product",
            29.99m,
            50,
            fileStream,
            "",
            "image/jpeg"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.FileName);
    }

    [Fact]
    public void Validate_InvalidFileNameExtension_ShouldHaveError()
    {
        // Arrange
        var fileStream = new MemoryStream(new byte[100]);
        var command = new CreateProductCommand(
            "Valid Product",
            29.99m,
            50,
            fileStream,
            "product.txt",
            "text/plain"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.FileName);
    }

    [Fact]
    public void Validate_NullFileStream_ThrowsNullReferenceException()
    {
        // Arrange
        var command = new CreateProductCommand(
            "Valid Product",
            29.99m,
            50,
            null,
            "product.jpg",
            "image/jpeg"
        );

        // Act & Assert - NullReferenceException é lançada ao acessar Length
        Assert.Throws<NullReferenceException>(
            () => _validator.TestValidate(command)
        );
    }

    [Fact]
    public void Validate_EmptyFileStream_ShouldHaveError()
    {
        // Arrange
        var fileStream = new MemoryStream(new byte[0]); // empty stream
        var command = new CreateProductCommand(
            "Valid Product",
            29.99m,
            50,
            fileStream,
            "product.jpg",
            "image/jpeg"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.FileStream);
    }

    [Fact]
    public void Validate_FileTooLarge_ShouldHaveError()
    {
        // Arrange
        var largeData = new byte[6 * 1024 * 1024]; // 6MB
        var fileStream = new MemoryStream(largeData);
        var command = new CreateProductCommand(
            "Valid Product",
            29.99m,
            50,
            fileStream,
            "product.jpg",
            "image/jpeg"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.FileStream);
    }

    [Fact]
    public void Validate_EmptyContentType_ShouldHaveError()
    {
        // Arrange
        var fileStream = new MemoryStream(new byte[100]);
        var command = new CreateProductCommand(
            "Valid Product",
            29.99m,
            50,
            fileStream,
            "product.jpg",
            ""
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.ContentType);
    }

    [Fact]
    public void Validate_ContentTypeNotImage_ShouldHaveError()
    {
        // Arrange
        var fileStream = new MemoryStream(new byte[100]);
        var command = new CreateProductCommand(
            "Valid Product",
            29.99m,
            50,
            fileStream,
            "product.jpg",
            "text/plain"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.ContentType);
    }

    [Fact]
    public void Validate_ContentTypeWithCharset_PassesBecauseStartsWithImage()
    {
        // Arrange
        var fileStream = new MemoryStream(new byte[100]);
        var command = new CreateProductCommand(
            "Valid Product",
            29.99m,
            50,
            fileStream,
            "product.jpg",
            "image/jpeg; charset=utf-8"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert - Passa porque começa com "image/"
        result.ShouldNotHaveValidationErrorFor(c => c.ContentType);
    }
}
