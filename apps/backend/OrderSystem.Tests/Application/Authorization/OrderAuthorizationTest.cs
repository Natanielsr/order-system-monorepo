using OrderSystem.Application.Authorization;
using OrderSystem.Application.DTOs.Order;
using OrderSystem.Application.Orders.Commands.CreateOrder;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Tests.Application.Authorization;

public class OrderAuthorizationTest
{
    private readonly Guid _validUserId = Guid.Parse("6f9619ff-8b86-d011-b42d-00c04fc964ff");
    private readonly UserClaim _userClaim;
    private readonly UserClaim _adminClaim;

    public OrderAuthorizationTest()
    {
        _userClaim = new UserClaim
        {
            Id = _validUserId.ToString(),
            Username = "testuser",
            Email = "test@example.com",
            Role = UserRole.User
        };

        _adminClaim = new UserClaim
        {
            Id = Guid.NewGuid().ToString(),
            Username = "admin",
            Email = "admin@example.com",
            Role = UserRole.Admin
        };
    }

    [Fact]
    public void CreateOrder_WithMatchingUserIds_ReturnsSuccess()
    {
        // Arrange
        var command = new CreateOrderCommand(
            OrderItems: new List<CreateOrderItemDto>(),
            UserId: _validUserId,
            PaymentMethod: PaymentMethod.CreditCard,
            AddressId: Guid.NewGuid()
        );

        // Act
        var result = OrderAuthorization.CreateOrder(_userClaim, command);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("authorized user", result.Message);
    }

    [Fact]
    public void CreateOrder_WithNonMatchingUserIds_ReturnsFailure()
    {
        // Arrange
        var command = new CreateOrderCommand(
            OrderItems: new List<CreateOrderItemDto>(),
            UserId: Guid.NewGuid(),
            PaymentMethod: PaymentMethod.CreditCard,
            AddressId: Guid.NewGuid()
        );

        // Act
        var result = OrderAuthorization.CreateOrder(_userClaim, command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("the authenticated user id is different from the order user id", result.Message);
    }

    [Fact]
    public void CreateOrder_WithInvalidUserClaimId_ReturnsFailure()
    {
        // Arrange
        var invalidUserClaim = new UserClaim
        {
            Id = "invalid-guid",
            Username = "testuser",
            Email = "test@example.com",
            Role = UserRole.User
        };
        var command = new CreateOrderCommand(
            OrderItems: new List<CreateOrderItemDto>(),
            UserId: _validUserId,
            PaymentMethod: PaymentMethod.CreditCard,
            AddressId: Guid.NewGuid()
        );

        // Act
        var result = OrderAuthorization.CreateOrder(invalidUserClaim, command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("the authenticated id is not a valid guid", result.Message);
    }

    [Fact]
    public void GetById_WithAdminRole_AlwaysReturnsSuccess()
    {
        // Arrange
        var orderDto = new OrderDto
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Status = (int)OrderStatus.Pending,
            Active = true,
            Total = 0,
            Code = "ORD-001",
            OrderItems = new List<OrderItemDto>(),
            UserName = "user",
            UserEmail = "user@example.com",
            PaymentInfo = new List<PaymentInfoDto>()
        };

        // Act
        var result = OrderAuthorization.GetById(_adminClaim, orderDto);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("User Admin authorized", result.Message);
    }

    [Fact]
    public void GetById_WithMatchingUser_ReturnsSuccess()
    {
        // Arrange
        var orderDto = new OrderDto
        {
            Id = Guid.NewGuid(),
            UserId = _validUserId,
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Status = (int)OrderStatus.Pending,
            Active = true,
            Total = 0,
            Code = "ORD-001",
            OrderItems = new List<OrderItemDto>(),
            UserName = "user",
            UserEmail = "user@example.com",
            PaymentInfo = new List<PaymentInfoDto>()
        };

        // Act
        var result = OrderAuthorization.GetById(_userClaim, orderDto);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("authorized user", result.Message);
    }

    [Fact]
    public void GetById_WithNonMatchingUser_ReturnsFailure()
    {
        // Arrange
        var orderDto = new OrderDto
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Status = (int)OrderStatus.Pending,
            Active = true,
            Total = 0,
            Code = "ORD-001",
            OrderItems = new List<OrderItemDto>(),
            UserName = "user",
            UserEmail = "user@example.com",
            PaymentInfo = new List<PaymentInfoDto>()
        };

        // Act
        var result = OrderAuthorization.GetById(_userClaim, orderDto);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("the authenticated user id is different from the request user id", result.Message);
    }

    [Fact]
    public void GetById_WithInvalidUserClaimId_ReturnsFailure()
    {
        // Arrange
        var invalidUserClaim = new UserClaim
        {
            Id = "invalid-guid",
            Username = "testuser",
            Email = "test@example.com",
            Role = UserRole.User
        };
        var orderDto = new OrderDto
        {
            Id = Guid.NewGuid(),
            UserId = _validUserId,
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Status = (int)OrderStatus.Pending,
            Active = true,
            Total = 0,
            Code = "ORD-001",
            OrderItems = new List<OrderItemDto>(),
            UserName = "user",
            UserEmail = "user@example.com",
            PaymentInfo = new List<PaymentInfoDto>()
        };

        // Act
        var result = OrderAuthorization.GetById(invalidUserClaim, orderDto);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("the authenticated id is not a valid guid", result.Message);
    }

    [Fact]
    public void GetById_WithNullOrderDto_ThrowsNullReferenceException()
    {
        // Arrange
        OrderDto? orderDto = null;

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => OrderAuthorization.GetById(_userClaim, orderDto));
    }
}
