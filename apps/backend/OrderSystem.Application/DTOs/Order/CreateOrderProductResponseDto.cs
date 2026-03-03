namespace OrderSystem.Application.DTOs.Order;

public record class CreateOrderItemResponseDto
{
    public required Guid ProductId { get; set; }
    public required int Quantity { get; set; }
    public required decimal UnitPrice { get; set; }

    public decimal Total { get; set; }
}
