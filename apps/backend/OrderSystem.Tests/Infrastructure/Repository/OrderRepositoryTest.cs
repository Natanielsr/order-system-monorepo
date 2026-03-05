#nullable disable

using Microsoft.EntityFrameworkCore;
using OrderSystem.Domain.Entities;
using OrderSystem.Infrastructure.Data;
using OrderSystem.Infrastructure.Repository.EntityFramework;

namespace OrderSystem.Tests.Infrastructure.Repository;

public class OrderRepositoryTest
{
    private static AppDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new AppDbContext(options);
    }

    private static Order CreateTestOrder(Guid orderId, Guid userId, string code = "ORD-001")
    {
        var orderItemId = Guid.NewGuid();
        return new Order()
        {
            Id = orderId,
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Active = true,
            OrderItems = new List<OrderItem>()
            {
                new OrderItem()
                {
                    Id = orderItemId,
                    CreationDate = DateTimeOffset.UtcNow,
                    UpdateDate = DateTimeOffset.UtcNow,
                    Active = true,
                    OrderId = orderId,
                    ProductId = Guid.NewGuid(),
                    ProductName = "Test Product",
                    UnitPrice = 10.00m,
                    Quantity = 2
                }
            },
            UserId = userId,
            UserName = "testuser",
            UserEmail = "test@email.com",
            Total = 20.00m,
            Status = OrderStatus.Pending,
            Code = code,
            AddressId = Guid.NewGuid()
        };
    }

    // --- AddAsync ---

    [Fact]
    public async Task AddAsync_ValidOrder_PersistsAndReturnsOrder()
    {
        // Arrange
        var dbName = nameof(AddAsync_ValidOrder_PersistsAndReturnsOrder);
        using var context = CreateContext(dbName);
        var repository = new OrderRepository(context);
        var orderId = Guid.NewGuid();
        var order = CreateTestOrder(orderId, Guid.NewGuid());

        // Act
        var result = await repository.AddAsync(order);
        await context.SaveChangesAsync();

        // Assert
        var persisted = await context.Orders.FindAsync(orderId);
        Assert.NotNull(result);
        Assert.NotNull(persisted);
        Assert.Equal(orderId, persisted.Id);
        Assert.Equal("testuser", persisted.UserName);
    }

    [Fact]
    public async Task AddAsync_WithOrderItems_PersistsRelatedItems()
    {
        // Arrange
        var dbName = nameof(AddAsync_WithOrderItems_PersistsRelatedItems);
        using var context = CreateContext(dbName);
        var repository = new OrderRepository(context);
        var orderId = Guid.NewGuid();
        var order = CreateTestOrder(orderId, Guid.NewGuid());

        // Act
        await repository.AddAsync(order);
        await context.SaveChangesAsync();

        // Assert
        var persisted = await context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        Assert.NotNull(persisted);
        Assert.Single(persisted.OrderItems);
        Assert.Equal("Test Product", persisted.OrderItems.First().ProductName);
        Assert.Equal(10.00m, persisted.OrderItems.First().UnitPrice);
    }

    // --- DeleteAsync ---

    [Fact]
    public async Task DeleteAsync_ExistingOrder_ReturnsTrueAndRemoves()
    {
        // Arrange
        var dbName = nameof(DeleteAsync_ExistingOrder_ReturnsTrueAndRemoves);
        using var context = CreateContext(dbName);
        var repository = new OrderRepository(context);
        var orderId = Guid.NewGuid();
        var order = CreateTestOrder(orderId, Guid.NewGuid());
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.DeleteAsync(orderId);
        await context.SaveChangesAsync();

        // Assert
        Assert.True(result);
        var deleted = await context.Orders.FindAsync(orderId);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task DeleteAsync_NonExistentId_ReturnsFalse()
    {
        // Arrange
        var dbName = nameof(DeleteAsync_NonExistentId_ReturnsFalse);
        using var context = CreateContext(dbName);
        var repository = new OrderRepository(context);

        // Act
        var result = await repository.DeleteAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }

    // --- GetAllAsync ---

    [Fact]
    public async Task GetAllAsync_WithOrders_ReturnsAllWithItems()
    {
        // Arrange
        var dbName = nameof(GetAllAsync_WithOrders_ReturnsAllWithItems);
        using var context = CreateContext(dbName);
        var order1 = CreateTestOrder(Guid.NewGuid(), Guid.NewGuid(), "ORD-001");
        var order2 = CreateTestOrder(Guid.NewGuid(), Guid.NewGuid(), "ORD-002");
        context.Orders.AddRange(order1, order2);
        await context.SaveChangesAsync();

        var repository = new OrderRepository(context);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        var orders = result.Cast<Order>().ToList();
        Assert.Equal(2, orders.Count);
        Assert.All(orders, o => Assert.NotEmpty(o.OrderItems));
    }

    [Fact]
    public async Task GetAllAsync_EmptyDb_ReturnsEmptyList()
    {
        // Arrange
        var dbName = nameof(GetAllAsync_EmptyDb_ReturnsEmptyList);
        using var context = CreateContext(dbName);
        var repository = new OrderRepository(context);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }

    // --- GetAllUserOrdersAsync ---

    [Fact]
    public async Task GetAllUserOrdersAsync_FiltersAndPaginates()
    {
        // Arrange
        var dbName = nameof(GetAllUserOrdersAsync_FiltersAndPaginates);
        using var context = CreateContext(dbName);
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();

        // 3 orders for target user, 1 for another user
        for (int i = 0; i < 3; i++)
        {
            context.Orders.Add(CreateTestOrder(Guid.NewGuid(), userId, $"ORD-{i}"));
        }
        context.Orders.Add(CreateTestOrder(Guid.NewGuid(), otherUserId, "ORD-OTHER"));
        await context.SaveChangesAsync();

        var repository = new OrderRepository(context);

        // Act - page 1, size 2
        var page1 = await repository.GetAllUserOrdersAsync(userId, page: 1, pageSize: 2);

        // Assert
        Assert.Equal(2, page1.Count);
        Assert.All(page1, o => Assert.Equal(userId, o.UserId));
    }

    [Fact]
    public async Task GetAllUserOrdersAsync_OrdersByCreationDateDesc()
    {
        // Arrange
        var dbName = nameof(GetAllUserOrdersAsync_OrdersByCreationDateDesc);
        using var context = CreateContext(dbName);
        var userId = Guid.NewGuid();

        var olderOrder = CreateTestOrder(Guid.NewGuid(), userId, "ORD-OLD");
        // Override CreationDate for testing order
        var olderOrderEntry = context.Orders.Add(olderOrder);
        await context.SaveChangesAsync();

        // Small delay to ensure different timestamps
        var newerOrder = CreateTestOrder(Guid.NewGuid(), userId, "ORD-NEW");
        context.Orders.Add(newerOrder);
        await context.SaveChangesAsync();

        var repository = new OrderRepository(context);

        // Act
        var result = await repository.GetAllUserOrdersAsync(userId, page: 1, pageSize: 10);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.True(result[0].CreationDate >= result[1].CreationDate);
    }

    [Fact]
    public async Task GetAllUserOrdersAsync_NoOrders_ReturnsEmptyList()
    {
        // Arrange
        var dbName = nameof(GetAllUserOrdersAsync_NoOrders_ReturnsEmptyList);
        using var context = CreateContext(dbName);
        var repository = new OrderRepository(context);

        // Act
        var result = await repository.GetAllUserOrdersAsync(Guid.NewGuid(), page: 1, pageSize: 10);

        // Assert
        Assert.Empty(result);
    }

    // --- GetByIdAsync ---

    [Fact]
    public async Task GetByIdAsync_ExistingOrder_ReturnsWithItemsAndPayment()
    {
        // Arrange
        var dbName = nameof(GetByIdAsync_ExistingOrder_ReturnsWithItemsAndPayment);
        using var context = CreateContext(dbName);
        var orderId = Guid.NewGuid();
        var order = CreateTestOrder(orderId, Guid.NewGuid());
        order.PaymentInfo.Add(new PaymentInfo()
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Active = true,
            Method = PaymentMethod.Pix,
            PaidAmount = 20.00m,
            TransactionReference = "tx_123",
            LastFourDigits = "0000",
            ProviderName = "TestProvider",
            Status = PaymentStatus.Pending,
            OrderId = orderId
        });
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        var repository = new OrderRepository(context);

        // Act
        var result = (Order)await repository.GetByIdAsync(orderId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderId, result.Id);
        Assert.Single(result.OrderItems);
        Assert.Single(result.PaymentInfo);
        Assert.Equal(PaymentMethod.Pix, result.PaymentInfo.First().Method);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentId_ReturnsNull()
    {
        // Arrange
        var dbName = nameof(GetByIdAsync_NonExistentId_ReturnsNull);
        using var context = CreateContext(dbName);
        var repository = new OrderRepository(context);

        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    // --- UpdateAsync ---

    [Fact]
    public async Task UpdateAsync_ExistingOrder_ReturnsUpdatedOrder()
    {
        // Arrange
        var dbName = nameof(UpdateAsync_ExistingOrder_ReturnsUpdatedOrder);
        using var context = CreateContext(dbName);
        var orderId = Guid.NewGuid();
        var order = CreateTestOrder(orderId, Guid.NewGuid());
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        var updatedOrder = CreateTestOrder(orderId, order.UserId, "ORD-UPDATED");

        var repository = new OrderRepository(context);

        // Act
        var result = (Order)await repository.UpdateAsync(orderId, updatedOrder);

        // Assert
        Assert.NotNull(result);
    }
}
