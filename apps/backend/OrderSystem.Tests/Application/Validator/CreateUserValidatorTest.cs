#nullable disable

using FluentValidation.TestHelper;
using OrderSystem.Application.Validator;
using OrderSystem.Application.Users.Commands.CreateUser;

namespace OrderSystem.Tests.Application.Validator;

public class CreateUserValidatorTest
{
    private readonly CreateUserValidator _validator;

    public CreateUserValidatorTest()
    {
        _validator = new CreateUserValidator();
    }

    [Fact]
    public void Validate_ValidCommand_ShouldPass()
    {
        // Arrange
        var command = new CreateUserCommand(
            "validuser",
            "valid@email.com",
            "Password123!",
            "Password123!"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyUsername_ShouldHaveError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "",
            "valid@email.com",
            "Password123!",
            "Password123!"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Username);
    }

    [Fact]
    public void Validate_UsernameTooShort_ShouldHaveError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "ab",
            "valid@email.com",
            "Password123!",
            "Password123!"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Username);
    }

    [Fact]
    public void Validate_UsernameTooLong_ShouldHaveError()
    {
        // Arrange
        var command = new CreateUserCommand(
            new string('a', 21),
            "valid@email.com",
            "Password123!",
            "Password123!"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Username);
    }

    [Fact]
    public void Validate_UsernameStartsWithSpecialChar_ShouldHaveError()
    {
        // Arrange
        var command = new CreateUserCommand(
            ".username",
            "valid@email.com",
            "Password123!",
            "Password123!"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Username);
    }

    [Fact]
    public void Validate_UsernameEndsWithSpecialChar_ShouldHaveError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "username.",
            "valid@email.com",
            "Password123!",
            "Password123!"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Username);
    }

    [Fact]
    public void Validate_UsernameWithInvalidChars_ShouldHaveError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "user@name",
            "valid@email.com",
            "Password123!",
            "Password123!"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Username);
    }

    [Fact]
    public void Validate_EmptyEmail_ShouldHaveError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "validuser",
            "",
            "Password123!",
            "Password123!"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void Validate_InvalidEmail_ShouldHaveError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "validuser",
            "invalid-email",
            "Password123!",
            "Password123!"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void Validate_EmptyPassword_ShouldHaveError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "validuser",
            "valid@email.com",
            "",
            ""
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Password);
    }

    [Fact]
    public void Validate_PasswordTooShort_ShouldHaveError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "validuser",
            "valid@email.com",
            "Pass1!",
            "Pass1!"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Password);
    }

    [Fact]
    public void Validate_PasswordWithoutUppercase_ShouldHaveError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "validuser",
            "valid@email.com",
            "password123!",
            "password123!"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Password);
    }

    [Fact]
    public void Validate_PasswordWithoutLowercase_ShouldHaveError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "validuser",
            "valid@email.com",
            "PASSWORD123!",
            "PASSWORD123!"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Password);
    }

    [Fact]
    public void Validate_PasswordWithoutNumber_ShouldHaveError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "validuser",
            "valid@email.com",
            "Password!",
            "Password!"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Password);
    }

    [Fact]
    public void Validate_PasswordWithoutSpecialChar_ShouldHaveError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "validuser",
            "valid@email.com",
            "Password123",
            "Password123"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Password);
    }

    [Fact]
    public void Validate_PasswordMismatch_ShouldHaveError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "validuser",
            "valid@email.com",
            "Password123!",
            "Password123"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.ConfirmPassword);
    }
}
