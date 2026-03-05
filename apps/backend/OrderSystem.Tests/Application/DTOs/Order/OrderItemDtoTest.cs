using OrderSystem.Application.DTOs.Order;

namespace OrderSystem.Tests.Application.DTOs.Order;

/// <summary>
/// Unit tests for OrderItemDto
/// </summary>
public class OrderItemDtoTest
{
    [Fact]
    public void OrderItemDto_Creation_WithValidValues_ShouldCreateCorrectly()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var productName = "Test Product";
        var unitPrice = 19.99m;
        var quantity = 2;
        var total = 39.98m;

        // Act
        var orderItemDto = new OrderItemDto
        {
            ProductId = productId,
            ProductName = productName,
            UnitPrice = unitPrice,
            Quantity = quantity,
            Total = total
        };

        // Assert
        Assert.Equal(productId, orderItemDto.ProductId);
        Assert.Equal(productName, orderItemDto.ProductName);
        Assert.Equal(unitPrice, orderItemDto.UnitPrice);
        Assert.Equal(quantity, orderItemDto.Quantity);
        Assert.Equal(total, orderItemDto.Total);
    }

    [Fact]
    public void OrderItemDto_Creation_WithoutValues_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var orderItemDto = new OrderItemDto();

        // Assert
        Assert.Equal(Guid.Empty, orderItemDto.ProductId);
        Assert.Equal(string.Empty, orderItemDto.ProductName);
        Assert.Equal(0, orderItemDto.UnitPrice);
        Assert.Equal(0, orderItemDto.Quantity);
        Assert.Equal(0, orderItemDto.Total);
    }

    [Fact]
    public void OrderItemDto_ProductName_DefaultValue_ShouldBeEmptyString()
    {
        // Arrange & Act
        var orderItemDto = new OrderItemDto();

        // Assert
        Assert.NotNull(orderItemDto.ProductName);
        Assert.Equal(string.Empty, orderItemDto.ProductName);
    }

    [Fact]
    public void OrderItemDto_Equality_ShouldWorkAsRecord()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var dto1 = new OrderItemDto
        {
            ProductId = productId,
            ProductName = "Product A",
            UnitPrice = 10.00m,
            Quantity = 1,
            Total = 10.00m
        };
        var dto2 = new OrderItemDto
        {
            ProductId = productId,
            ProductName = "Product A",
            UnitPrice = 10.00m,
            Quantity = 1,
            Total = 10.00m
        };

        // Act & Assert
        Assert.Equal(dto1, dto2);
        Assert.True(dto1 == dto2);
    }

    [Fact]
    public void OrderItemDto_Equality_ShouldBeDifferent_WhenPropertiesDiffer()
    {
        // Arrange
        var dto1 = new OrderItemDto
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Product A",
            UnitPrice = 10.00m,
            Quantity = 1,
            Total = 10.00m
        };
        var dto2 = new OrderItemDto
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Product B",
            UnitPrice = 20.00m,
            Quantity = 2,
            Total = 40.00m
        };

        // Act & Assert
        Assert.NotEqual(dto1, dto2);
        Assert.False(dto1 == dto2);
    }


    [Fact]
    public void OrderItemDto_WithExpression_ShouldCreateModifiedCopy()
    {
        // Arrange
        var original = new OrderItemDto
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Original Product",
            UnitPrice = 10.00m,
            Quantity = 1,
            Total = 10.00m
        };

        // Act
        var modified = original with { ProductName = "Modified Product", Quantity = 5, Total = 50.00m };

        // Assert
        Assert.Equal(original.ProductId, modified.ProductId);
        Assert.NotEqual(original.ProductName, modified.ProductName);
        Assert.Equal("Modified Product", modified.ProductName);
        Assert.Equal(original.UnitPrice, modified.UnitPrice);
        Assert.Equal(5, modified.Quantity);
        Assert.Equal(50.00m, modified.Total);
        Assert.True(original != modified);
    }

    [Fact]
    public void OrderItemDto_WithNamedDelegates_ShouldAssignCorrectly()
    {
        // Arrange & Act
        var orderItemDto = new OrderItemDto
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Test",
            UnitPrice = 5.00m,
            Quantity = 10,
            Total = 50.00m
        };

        // Assert
        Assert.IsType<Guid>(orderItemDto.ProductId);
        Assert.IsType<string>(orderItemDto.ProductName);
        Assert.IsType<decimal>(orderItemDto.UnitPrice);
        Assert.IsType<int>(orderItemDto.Quantity);
        Assert.IsType<decimal>(orderItemDto.Total);
    }
}
