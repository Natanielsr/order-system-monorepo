namespace OrderSystem.Application.DTOs.Order;

public record class OrderDto
{
    public Guid Id { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset UpdateDate { get; set; }
    public List<OrderItemDto>? OrderItems { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public bool Active { get; set; }
    public decimal Total { get; set; }
    public int Status { get; set; }
    public List<PaymentInfoDto>? PaymentInfo { get; set; }
    public string Code { get; set; } = string.Empty;

}
