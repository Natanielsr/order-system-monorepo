#nullable disable

using AutoMapper;
using Moq;
using OrderSystem.Application.DTOs.Order;
using OrderSystem.Application.Orders.Queries.GetOrderById;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;

namespace OrderSystem.Tests.Application.Orders.Queries.GetById;

public class GetOrderByIdHandlerTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetOrderByIdHandler _handler;

    public GetOrderByIdHandlerTest()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetOrderByIdHandler(_orderRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingOrder_ReturnsOrderDto()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Order
        {
            Id = orderId,
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Active = true,
            UserId = Guid.NewGuid(),
            UserName = "testuser",
            UserEmail = "test@example.com",
            Total = 100m,
            Status = OrderStatus.Pending,
            Code = "ORD001",
            AddressId = Guid.NewGuid(),
            OrderItems = new List<OrderItem>()
        };

        var orderDto = new OrderDto
        {
            Id = orderId,
            UserName = "testuser",
            UserEmail = "test@example.com"
        };

        _orderRepositoryMock
            .Setup(r => r.GetByIdAsync(orderId))
            .ReturnsAsync(order);

        _mapperMock
            .Setup(m => m.Map<OrderDto>(order))
            .Returns(orderDto);

        var query = new GetOrderByIdQuery(orderId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderId, result.Id);
        _orderRepositoryMock.Verify(r => r.GetByIdAsync(orderId), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingOrder_ReturnsNull()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _orderRepositoryMock
            .Setup(r => r.GetByIdAsync(orderId))
            .ReturnsAsync((Order)null);

        var query = new GetOrderByIdQuery(orderId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var expectedException = new Exception("Database error");

        _orderRepositoryMock
            .Setup(r => r.GetByIdAsync(orderId))
            .ThrowsAsync(expectedException);

        var query = new GetOrderByIdQuery(orderId);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
    }
}
