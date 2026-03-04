#nullable disable

using AutoMapper;
using Moq;
using OrderSystem.Application.DTOs.Order;
using OrderSystem.Application.Orders.Queries.ListUserOrders;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;

namespace OrderSystem.Tests.Application.Orders.Queries.ListUserOrders;

public class ListUserOrdersHandlerTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ListUserOrdersHandler _handler;

    public ListUserOrdersHandlerTest()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new ListUserOrdersHandler(_orderRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserHasOrders_ReturnsUserOrderDtos()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var page = 1;
        var pageSize = 10;

        var orders = new List<Order>
        {
            new Order
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow,
                Active = true,
                UserId = userId,
                UserName = "testuser",
                UserEmail = "test@example.com",
                Total = 100m,
                Status = OrderStatus.Pending,
                Code = "ORD001",
                AddressId = Guid.NewGuid(),
                OrderItems = new List<OrderItem>()
            },
            new Order
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTimeOffset.UtcNow.AddDays(-1),
                UpdateDate = DateTimeOffset.UtcNow.AddDays(-1),
                Active = true,
                UserId = userId,
                UserName = "testuser",
                UserEmail = "test@example.com",
                Total = 200m,
                Status = OrderStatus.Paid,
                Code = "ORD002",
                AddressId = Guid.NewGuid(),
                OrderItems = new List<OrderItem>()
            }
        };

        var ordersDto = new List<OrderDto>
        {
            new OrderDto { Id = orders[0].Id, UserId = userId, UserName = "testuser" },
            new OrderDto { Id = orders[1].Id, UserId = userId, UserName = "testuser" }
        };

        _orderRepositoryMock
            .Setup(r => r.GetAllUserOrdersAsync(userId, page, pageSize))
            .ReturnsAsync(orders);

        _mapperMock
            .Setup(m => m.Map<List<OrderDto>>(orders))
            .Returns(ordersDto);

        var query = new ListUserOrdersQuery(userId, page, pageSize);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, o => Assert.Equal(userId, o.UserId));
        _orderRepositoryMock.Verify(r => r.GetAllUserOrdersAsync(userId, page, pageSize), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserHasNoOrders_ReturnsEmptyList()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var page = 1;
        var pageSize = 10;

        _orderRepositoryMock
            .Setup(r => r.GetAllUserOrdersAsync(userId, page, pageSize))
            .ReturnsAsync(new List<Order>());

        _mapperMock
            .Setup(m => m.Map<List<OrderDto>>(It.IsAny<List<Order>>()))
            .Returns(new List<OrderDto>());

        var query = new ListUserOrdersQuery(userId, page, pageSize);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_WithPagination_ReturnsCorrectPage()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var page = 2;
        var pageSize = 5;

        var orders = new List<Order>();
        for (int i = 0; i < 15; i++)
        {
            orders.Add(new Order
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow,
                Active = true,
                UserId = userId,
                UserName = "testuser",
                UserEmail = "test@example.com",
                Total = 100m,
                Status = OrderStatus.Pending,
                Code = $"ORD{i:000}",
                AddressId = Guid.NewGuid(),
                OrderItems = new List<OrderItem>()
            });
        }

        var expectedOrders = orders.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        _orderRepositoryMock
            .Setup(r => r.GetAllUserOrdersAsync(userId, page, pageSize))
            .ReturnsAsync(expectedOrders);

        _mapperMock
            .Setup(m => m.Map<List<OrderDto>>(expectedOrders))
            .Returns(expectedOrders.Select(o => new OrderDto { Id = o.Id }).ToList());

        var query = new ListUserOrdersQuery(userId, page, pageSize);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(pageSize, result.Count);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var page = 1;
        var pageSize = 10;
        var expectedException = new Exception("Database error");

        _orderRepositoryMock
            .Setup(r => r.GetAllUserOrdersAsync(userId, page, pageSize))
            .ThrowsAsync(expectedException);

        var query = new ListUserOrdersQuery(userId, page, pageSize);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
    }
}
