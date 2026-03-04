using System;
using FluentValidation.TestHelper;
using OrderSystem.Application.Addresses.Commands.UpdateAddress;
using OrderSystem.Application.Validator;

namespace OrderSystem.Tests.Application.Validator;

public class UpdateAddressValidatorTest
{
    private readonly UpdateAddressValidator _validator;

    public UpdateAddressValidatorTest()
    {
        _validator = new UpdateAddressValidator();
    }

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateAddressCommand(
            Guid.NewGuid(),
            "João Silva",
            "12345678901",
            "Rua das Flores",
            "123",
            "Apto 10",
            "Centro",
            "São Paulo",
            "SP",
            "12345678",
            "Brasil",
            Guid.NewGuid(),
            false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyFullName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAddressCommand(
            Guid.NewGuid(),
            "",
            "12345678901",
            "Rua das Flores",
            "123",
            null,
            "Centro",
            "São Paulo",
            "SP",
            "12345678",
            "Brasil",
            Guid.NewGuid(),
            false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName)
            .WithErrorMessage("O nome completo é obrigatório.");
    }

    [Fact]
    public void Validate_FullNameTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var longName = new string('A', 101);
        var command = new UpdateAddressCommand(
            Guid.NewGuid(),
            longName,
            "12345678901",
            "Rua das Flores",
            "123",
            null,
            "Centro",
            "São Paulo",
            "SP",
            "12345678",
            "Brasil",
            Guid.NewGuid(),
            false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName)
            .WithErrorMessage("O nome deve ter no máximo 100 caracteres.");
    }

    [Fact]
    public void Validate_EmptyCpf_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAddressCommand(
            Guid.NewGuid(),
            "João Silva",
            "",
            "Rua das Flores",
            "123",
            null,
            "Centro",
            "São Paulo",
            "SP",
            "12345678",
            "Brasil",
            Guid.NewGuid(),
            false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Cpf)
            .WithErrorMessage("O CPF é obrigatório.");
    }

    [Fact]
    public void Validate_InvalidCpf_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAddressCommand(
            Guid.NewGuid(),
            "João Silva",
            "123", // CPF inválido (muito curto)
            "Rua das Flores",
            "123",
            null,
            "Centro",
            "São Paulo",
            "SP",
            "12345678",
            "Brasil",
            Guid.NewGuid(),
            false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Cpf)
            .WithErrorMessage("O CPF informado é inválido.");
    }

    [Fact]
    public void Validate_EmptyStreet_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAddressCommand(
            Guid.NewGuid(),
            "João Silva",
            "12345678901",
            "",
            "123",
            null,
            "Centro",
            "São Paulo",
            "SP",
            "12345678",
            "Brasil",
            Guid.NewGuid(),
            false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Street)
            .WithErrorMessage("A rua é obrigatória.");
    }

    [Fact]
    public void Validate_StreetTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var longStreet = new string('R', 151);
        var command = new UpdateAddressCommand(
            Guid.NewGuid(),
            "João Silva",
            "12345678901",
            longStreet,
            "123",
            null,
            "Centro",
            "São Paulo",
            "SP",
            "12345678",
            "Brasil",
            Guid.NewGuid(),
            false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Street)
            .WithErrorMessage("A rua deve ter no máximo 150 caracteres.");
    }

    [Fact]
    public void Validate_EmptyNumber_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAddressCommand(
            Guid.NewGuid(),
            "João Silva",
            "12345678901",
            "Rua das Flores",
            "",
            null,
            "Centro",
            "São Paulo",
            "SP",
            "12345678",
            "Brasil",
            Guid.NewGuid(),
            false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Number)
            .WithErrorMessage("O número é obrigatório.");
    }

    [Fact]
    public void Validate_EmptyNeighborhood_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAddressCommand(
            Guid.NewGuid(),
            "João Silva",
            "12345678901",
            "Rua das Flores",
            "123",
            null,
            "",
            "São Paulo",
            "SP",
            "12345678",
            "Brasil",
            Guid.NewGuid(),
            false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Neighborhood)
            .WithErrorMessage("O bairro é obrigatório.");
    }

    [Fact]
    public void Validate_EmptyCity_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAddressCommand(
            Guid.NewGuid(),
            "João Silva",
            "12345678901",
            "Rua das Flores",
            "123",
            null,
            "Centro",
            "",
            "SP",
            "12345678",
            "Brasil",
            Guid.NewGuid(),
            false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.City)
            .WithErrorMessage("A cidade é obrigatória.");
    }

    [Fact]
    public void Validate_EmptyState_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAddressCommand(
            Guid.NewGuid(),
            "João Silva",
            "12345678901",
            "Rua das Flores",
            "123",
            null,
            "Centro",
            "São Paulo",
            "",
            "12345678",
            "Brasil",
            Guid.NewGuid(),
            false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.State)
            .WithErrorMessage("O estado é obrigatório.");
    }

    [Fact]
    public void Validate_StateTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAddressCommand(
            Guid.NewGuid(),
            "João Silva",
            "12345678901",
            "Rua das Flores",
            "123",
            null,
            "Centro",
            "São Paulo",
            "SPA",
            "12345678",
            "Brasil",
            Guid.NewGuid(),
            false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.State)
            .WithErrorMessage("Use a sigla do estado (ex: SP).");
    }

    [Fact]
    public void Validate_EmptyZipCode_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAddressCommand(
            Guid.NewGuid(),
            "João Silva",
            "12345678901",
            "Rua das Flores",
            "123",
            null,
            "Centro",
            "São Paulo",
            "SP",
            "",
            "Brasil",
            Guid.NewGuid(),
            false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ZipCode)
            .WithErrorMessage("O CEP é obrigatório.");
    }

    [Fact]
    public void Validate_InvalidZipCodeFormat_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAddressCommand(
            Guid.NewGuid(),
            "João Silva",
            "12345678901",
            "Rua das Flores",
            "123",
            null,
            "Centro",
            "São Paulo",
            "SP",
            "1234567", // inválido
            "Brasil",
            Guid.NewGuid(),
            false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ZipCode)
            .WithErrorMessage("CEP em formato inválido (use 00000-000 ou 00000000).");
    }

    [Fact]
    public void Validate_EmptyUserId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAddressCommand(
            Guid.NewGuid(),
            "João Silva",
            "12345678901",
            "Rua das Flores",
            "123",
            null,
            "Centro",
            "São Paulo",
            "SP",
            "12345678",
            "Brasil",
            Guid.Empty,
            false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage("O ID do usuário é obrigatório.");
    }

    [Fact]
    public void Validate_EmptyAddressId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAddressCommand(
            Guid.Empty,
            "João Silva",
            "12345678901",
            "Rua das Flores",
            "123",
            null,
            "Centro",
            "São Paulo",
            "SP",
            "12345678",
            "Brasil",
            Guid.NewGuid(),
            false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("O ID do endereço é obrigatório.");
    }

    [Theory]
    [InlineData("12345678901", true)] // CPF válido (11 dígitos)
    [InlineData("1234567890", false)] // 10 dígitos
    [InlineData("123456789012", false)] // 12 dígitos
    [InlineData("abcdefghijk", false)] // letras
    public void Validate_CpfLength_IsValidOrNot(string cpf, bool expected)
    {
        // Arrange
        var command = new UpdateAddressCommand(
            Guid.NewGuid(),
            "João Silva",
            cpf,
            "Rua das Flores",
            "123",
            null,
            "Centro",
            "São Paulo",
            "SP",
            "12345678",
            "Brasil",
            Guid.NewGuid(),
            false
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        if (expected)
            result.ShouldNotHaveValidationErrorFor(x => x.Cpf);
        else
            result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }
}
