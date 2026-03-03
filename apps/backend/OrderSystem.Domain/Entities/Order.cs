using System.Dynamic;
using OrderSystem.Domain.Exceptions;

namespace OrderSystem.Domain.Entities;

public enum OrderStatus
{
    Pending,
    Paid,
    Shipped,
    Canceled
}

public class Order : Entity
{
    public required List<OrderItem> OrderItems { get; init; }
    public required Guid UserId { get; init; }
    public User? User { get; set; }
    public required string UserName { get; init; }
    public required string UserEmail { get; init; }
    public required decimal Total { get; init; }
    public required OrderStatus Status { get; init; }
    public List<PaymentInfo> PaymentInfo { get; set; } = new List<PaymentInfo>();
    public required string Code { get; init; }
    public required Guid AddressId { get; init; }
    public Address? Address { get; set; }

    public static decimal CalcTotal(List<OrderItem> orderItems)
    {
        decimal total = 0;
        foreach (OrderItem i in orderItems)
        {
            total += i.Total;
        }

        return total;
    }

    public Order() { }

    public void AddProductOrder(OrderItem orderItem)
    {
        if (orderItem is null)
            throw new AddProductOrderException("productOrder cant be null");

        if (orderItem.Quantity <= 0)
            throw new AddProductOrderException("productOrder quantity must be bigger then zero");

        if (orderItem.UnitPrice <= 0)
            throw new AddProductOrderException("productOrder UnitPrice must be bigger then zero");

        if (ProductExistsInOrder(orderItem.ProductId))
            throw new AddProductOrderException("ProductId already exists in productOrder");

        OrderItems.Add(orderItem);
    }

    private bool ProductExistsInOrder(Guid productId)
    {
        // "Existe algum produto onde o ID seja igual ao productId?"
        return OrderItems.Any(x => x.ProductId == productId);
    }

    public static Order CreateOrder(
        List<OrderItem> orderItems,
        Guid userId,
        string userName,
        string userEmail,
        decimal total,
        OrderStatus status,
        string code,
        Guid addressId
        )
    {
        return new Order()
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Active = true,
            OrderItems = orderItems,
            UserId = userId,
            UserName = userName,
            UserEmail = userEmail,
            Total = total,
            Status = status,
            Code = code,
            AddressId = addressId
        };
    }



}
