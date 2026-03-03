namespace OrderSystem.Application.DTOs.Order;

public record class CreateOrderResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required List<CreateOrderItemResponseDto> OrderItems { get; set; }

    public decimal Total { get; set; }
}
