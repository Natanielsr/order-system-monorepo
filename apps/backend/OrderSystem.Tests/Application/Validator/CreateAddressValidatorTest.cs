#nullable disable

using FluentValidation.TestHelper;
using OrderSystem.Application.Addresses.Commands.CreateAddress;
using OrderSystem.Application.Validator;

namespace OrderSystem.Tests.Application.Validator;

public class CreateAddressValidatorTest
{
    private readonly CreateAddressValidator _validator;

    public CreateAddressValidatorTest()
    {
        _validator = new CreateAddressValidator();
    }

    [Fact]
    public void Validate_EmptyFullName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateAddressCommand(
            string.Empty, "12345678901", "Rua Test", "123", "", "Bairro", "Cidade", "SP", "12345678", Guid.NewGuid(), false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Validate_FullNameTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var longName = new string('A', 101);
        var command = new CreateAddressCommand(
            longName, "12345678901", "Rua Test", "123", "", "Bairro", "Cidade", "SP", "12345678", Guid.NewGuid(), false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Validate_EmptyCpf_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateAddressCommand(
            "Test User", string.Empty, "Rua Test", "123", "", "Bairro", "Cidade", "SP", "12345678", Guid.NewGuid(), false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    [Fact]
    public void Validate_InvalidCpf_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateAddressCommand(
            "Test User", "123", "Rua Test", "123", "", "Bairro", "Cidade", "SP", "12345678", Guid.NewGuid(), false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    [Fact]
    public void Validate_EmptyStreet_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateAddressCommand(
            "Test User", "12345678901", string.Empty, "123", "", "Bairro", "Cidade", "SP", "12345678", Guid.NewGuid(), false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Street);
    }

    [Fact]
    public void Validate_EmptyNumber_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateAddressCommand(
            "Test User", "12345678901", "Rua Test", string.Empty, "", "Bairro", "Cidade", "SP", "12345678", Guid.NewGuid(), false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Number);
    }

    [Fact]
    public void Validate_EmptyNeighborhood_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateAddressCommand(
            "Test User", "12345678901", "Rua Test", "123", "", string.Empty, "Cidade", "SP", "12345678", Guid.NewGuid(), false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Neighborhood);
    }

    [Fact]
    public void Validate_EmptyCity_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateAddressCommand(
            "Test User", "12345678901", "Rua Test", "123", "", "Bairro", string.Empty, "SP", "12345678", Guid.NewGuid(), false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.City);
    }

    [Fact]
    public void Validate_EmptyState_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateAddressCommand(
            "Test User", "12345678901", "Rua Test", "123", "", "Bairro", "Cidade", string.Empty, "12345678", Guid.NewGuid(), false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.State);
    }

    [Fact]
    public void Validate_StateTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateAddressCommand(
            "Test User", "12345678901", "Rua Test", "123", "", "Bairro", "Cidade", "SPA", "12345678", Guid.NewGuid(), false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.State);
    }

    [Fact]
    public void Validate_InvalidZipCode_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateAddressCommand(
            "Test User", "12345678901", "Rua Test", "123", "", "Bairro", "Cidade", "SP", "1234567", Guid.NewGuid(), false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ZipCode);
    }

    [Fact]
    public void Validate_EmptyUserId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateAddressCommand(
            "Test User", "12345678901", "Rua Test", "123", "", "Bairro", "Cidade", "SP", "12345678", Guid.Empty, false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateAddressCommand(
            "Test User", "12345678901", "Rua Test", "123", "Apto 45", "Bairro", "Cidade", "SP", "12345678", Guid.NewGuid(), false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_ValidCpfFormats_ShouldPass()
    {
        // Arrange
        var command1 = new CreateAddressCommand(
            "Test User", "123.456.789-01", "Rua Test", "123", "", "Bairro", "Cidade", "SP", "12345678", Guid.NewGuid(), false
        );

        var command2 = new CreateAddressCommand(
            "Test User", "12345678901", "Rua Test", "123", "", "Bairro", "Cidade", "SP", "12345678", Guid.NewGuid(), false
        );

        // Act
        var result1 = _validator.TestValidate(command1);
        var result2 = _validator.TestValidate(command2);

        // Assert
        result1.ShouldNotHaveAnyValidationErrors();
        result2.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_ValidZipCodeFormats_ShouldPass()
    {
        // Arrange
        var command1 = new CreateAddressCommand(
            "Test User", "12345678901", "Rua Test", "123", "", "Bairro", "Cidade", "SP", "12345678", Guid.NewGuid(), false
        );

        var command2 = new CreateAddressCommand(
            "Test User", "12345678901", "Rua Test", "123", "", "Bairro", "Cidade", "SP", "12345-678", Guid.NewGuid(), false
        );

        // Act
        var result1 = _validator.TestValidate(command1);
        var result2 = _validator.TestValidate(command2);

        // Assert
        result1.ShouldNotHaveAnyValidationErrors();
        result2.ShouldNotHaveAnyValidationErrors();
    }
}
