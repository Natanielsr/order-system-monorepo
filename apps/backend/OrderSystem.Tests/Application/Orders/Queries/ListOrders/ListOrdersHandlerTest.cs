#nullable disable

using AutoMapper;
using Moq;
using OrderSystem.Application.DTOs.Order;
using OrderSystem.Application.Orders.Queries.ListOrders;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;

namespace OrderSystem.Tests.Application.Orders.Queries.ListOrders;

public class ListOrdersHandlerTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ListOrdersHandler _handler;

    public ListOrdersHandlerTest()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new ListOrdersHandler(_orderRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenOrdersExist_ReturnsAllOrderDtos()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow,
                Active = true,
                UserId = Guid.NewGuid(),
                UserName = "user1",
                UserEmail = "user1@example.com",
                Total = 100m,
                Status = OrderStatus.Pending,
                Code = "ORD001",
                AddressId = Guid.NewGuid(),
                OrderItems = new List<OrderItem>()
            },
            new Order
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow,
                Active = true,
                UserId = Guid.NewGuid(),
                UserName = "user2",
                UserEmail = "user2@example.com",
                Total = 200m,
                Status = OrderStatus.Paid,
                Code = "ORD002",
                AddressId = Guid.NewGuid(),
                OrderItems = new List<OrderItem>()
            }
        };

        var ordersDto = new List<OrderDto>
        {
            new OrderDto { Id = orders[0].Id, UserName = "user1" },
            new OrderDto { Id = orders[1].Id, UserName = "user2" }
        };

        _orderRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(orders);

        _mapperMock
            .Setup(m => m.Map<List<OrderDto>>(orders))
            .Returns(ordersDto);

        var query = new ListOrdersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, o => o.UserName == "user1");
        Assert.Contains(result, o => o.UserName == "user2");
        _orderRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNoOrders_ReturnsEmptyList()
    {
        // Arrange
        var emptyList = new List<Order>();

        _orderRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(emptyList);

        _mapperMock
            .Setup(m => m.Map<List<OrderDto>>(emptyList))
            .Returns(new List<OrderDto>());

        var query = new ListOrdersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var expectedException = new Exception("Database connection error");

        _orderRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ThrowsAsync(expectedException);

        var query = new ListOrdersQuery();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_MappingFails_ThrowsException()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow,
                Active = true,
                UserId = Guid.NewGuid(),
                UserName = "user1",
                UserEmail = "user1@example.com",
                Total = 100m,
                Status = OrderStatus.Pending,
                Code = "ORD001",
                AddressId = Guid.NewGuid(),
                OrderItems = new List<OrderItem>()
            }
        };

        _orderRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(orders);

        _mapperMock
            .Setup(m => m.Map<List<OrderDto>>(orders))
            .Throws(new AutoMapperMappingException("Mapping error", null));

        var query = new ListOrdersQuery();

        // Act & Assert
        await Assert.ThrowsAsync<AutoMapperMappingException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
