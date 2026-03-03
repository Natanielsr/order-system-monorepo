using System;

namespace OrderSystem.Domain.Entities;

public class OrderItem : Entity
{
    public required Guid OrderId { get; init; }
    public Order? Order { get; set; }
    public required Guid ProductId { get; init; }
    public Product? Product { get; set; }
    public required string ProductName { get; init; }
    public required decimal UnitPrice { get; init; }
    public required int Quantity { get; init; }

    public decimal Total
    {
        get
        {
            return UnitPrice * Quantity;
        }
    }
}
