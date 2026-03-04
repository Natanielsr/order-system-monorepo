using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using OrderSystem.Application.Behaviors;
using Xunit;

#nullable disable

namespace OrderSystem.Tests.Application.Behaviors;

public class ValidatorBehaviorTest
{
    private readonly Mock<IValidator<TestRequest>> _validatorMock;
    private readonly ValidationBehavior<TestRequest, TestResponse> _behavior;

    public ValidatorBehaviorTest()
    {
        _validatorMock = new Mock<IValidator<TestRequest>>();
        _behavior = new ValidationBehavior<TestRequest, TestResponse>(new[] { _validatorMock.Object });
    }

    [Fact]
    public async Task Handle_NoValidators_ShouldCallNext()
    {
        // Arrange
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(Enumerable.Empty<IValidator<TestRequest>>());
        var request = new TestRequest { Name = "Test" };
        var response = new TestResponse(true);
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await behavior.Handle(request, (ct) => Task.FromResult(response), cancellationToken);

        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public async Task Handle_ValidationFails_ShouldThrowValidationException()
    {
        // Arrange
        var request = new TestRequest { Name = "Invalid" };
        var cancellationToken = CancellationToken.None;

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), cancellationToken))
            .ReturnsAsync(new ValidationResult(new[]
            {
                new ValidationFailure("Name", "Name is required")
            }));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            async () => await _behavior.Handle(request, (ct) => Task.FromResult(new TestResponse(false)), cancellationToken)
        );

        Assert.Single(exception.Errors);
        Assert.Equal("Name", exception.Errors.First().PropertyName);
        Assert.Equal("Name is required", exception.Errors.First().ErrorMessage);
    }

    [Fact]
    public async Task Handle_ValidationSucceeds_ShouldCallNext()
    {
        // Arrange
        var request = new TestRequest { Name = "Valid" };
        var response = new TestResponse(true);
        var cancellationToken = CancellationToken.None;

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), cancellationToken))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await _behavior.Handle(request, (ct) => Task.FromResult(response), cancellationToken);

        // Assert
        Assert.True(result.Success);
        _validatorMock.Verify(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_MultipleValidatorsWithErrors_ShouldAggregateAllErrors()
    {
        // Arrange
        var validator1Mock = new Mock<IValidator<TestRequest>>();
        var validator2Mock = new Mock<IValidator<TestRequest>>();
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(
            new IValidator<TestRequest>[] { _validatorMock.Object, validator1Mock.Object, validator2Mock.Object }
        );

        var request = new TestRequest { Name = "Invalid" };
        var cancellationToken = CancellationToken.None;

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), cancellationToken))
            .ReturnsAsync(new ValidationResult(new[]
            {
                new ValidationFailure("Name", "Error 1")
            }));

        validator1Mock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), cancellationToken))
            .ReturnsAsync(new ValidationResult(new[]
            {
                new ValidationFailure("Name", "Error 2")
            }));

        validator2Mock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), cancellationToken))
            .ReturnsAsync(new ValidationResult(new[]
            {
                new ValidationFailure("Name", "Error 3")
            }));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            async () => await behavior.Handle(request, (ct) => Task.FromResult(new TestResponse(false)), cancellationToken)
        );

        // Verificação corrigida:
        var errors = exception.Errors.ToList();
        Assert.Equal(3, errors.Count);
        Assert.Contains(exception.Errors, e => e.ErrorMessage == "Error 1");
        Assert.Contains(exception.Errors, e => e.ErrorMessage == "Error 2");
        Assert.Contains(exception.Errors, e => e.ErrorMessage == "Error 3");
    }
}

// Test request/response types
public record TestRequest : IRequest<TestResponse>
{
    public string Name { get; set; } = string.Empty;
}
public record TestResponse(bool Success);

