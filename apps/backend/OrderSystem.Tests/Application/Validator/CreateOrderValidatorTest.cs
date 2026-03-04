#nullable disable

using FluentValidation.TestHelper;
using OrderSystem.Application.DTOs.Order;
using OrderSystem.Application.Orders.Commands.CreateOrder;
using OrderSystem.Application.Validator;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Tests.Application.Validator;

public class CreateOrderValidatorTest
{
    private readonly CreateOrderValidator _validator;

    public CreateOrderValidatorTest()
    {
        _validator = new CreateOrderValidator();
    }

    [Fact]
    public void Validate_OrderItemsIsNull_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateOrderCommand(null, Guid.NewGuid(), PaymentMethod.CreditCard, Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.OrderItems);
    }

    [Fact]
    public void Validate_OrderItemsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateOrderCommand(new List<CreateOrderItemDto>(), Guid.NewGuid(), PaymentMethod.CreditCard, Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.OrderItems);
    }

    [Fact]
    public void Validate_OrderItemsWithDuplicateProductIds_ShouldHaveValidationError()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new CreateOrderCommand(
            new List<CreateOrderItemDto>
            {
                new CreateOrderItemDto { ProductId = productId, Quantity = 2 },
                new CreateOrderItemDto { ProductId = productId, Quantity = 1 }
            },
            Guid.NewGuid(),
            PaymentMethod.CreditCard,
            Guid.NewGuid()
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.OrderItems)
            .WithErrorMessage("the list contains duplicate product IDs.");
    }

    [Fact]
    public void Validate_OrderItemWithZeroQuantity_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateOrderCommand(
            new List<CreateOrderItemDto>
            {
                new CreateOrderItemDto { ProductId = Guid.NewGuid(), Quantity = 0 }
            },
            Guid.NewGuid(),
            PaymentMethod.CreditCard,
            Guid.NewGuid()
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("OrderItems[0].Quantity")
            .WithErrorMessage("Product quantity must be bigger then zero");
    }

    [Fact]
    public void Validate_OrderItemWithNegativeQuantity_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateOrderCommand(
            new List<CreateOrderItemDto>
            {
                new CreateOrderItemDto { ProductId = Guid.NewGuid(), Quantity = -1 }
            },
            Guid.NewGuid(),
            PaymentMethod.CreditCard,
            Guid.NewGuid()
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("OrderItems[0].Quantity")
            .WithErrorMessage("Product quantity must be bigger then zero");
    }

    [Fact]
    public void Validate_OrderItemWithEmptyProductId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateOrderCommand(
            new List<CreateOrderItemDto>
            {
                new CreateOrderItemDto { ProductId = Guid.Empty, Quantity = 1 }
            },
            Guid.NewGuid(),
            PaymentMethod.CreditCard,
            Guid.NewGuid()
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("OrderItems[0].ProductId")
            .WithErrorMessage("ProductId can´t be empty");
    }

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateOrderCommand(
            new List<CreateOrderItemDto>
            {
                new CreateOrderItemDto { ProductId = Guid.NewGuid(), Quantity = 2 },
                new CreateOrderItemDto { ProductId = Guid.NewGuid(), Quantity = 1 }
            },
            Guid.NewGuid(),
            PaymentMethod.CreditCard,
            Guid.NewGuid()
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
