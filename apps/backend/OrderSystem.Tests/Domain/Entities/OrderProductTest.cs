using System;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Tests.Domain.Entities;

public class OrderProductTest
{
    [Fact]
    public void TestTotal()
    {
        //Arrange
        OrderItem orderProduct = new()
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Active = true,
            OrderId = Guid.Empty,
            ProductId = Guid.Empty,
            ProductName = "product.Name",
            UnitPrice = 10,
            Quantity = 10
        };

        //Act
        var total = orderProduct.Total;

        //Assert
        Assert.Equal(100m, total);
    }
}
