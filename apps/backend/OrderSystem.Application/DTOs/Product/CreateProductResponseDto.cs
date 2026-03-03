namespace OrderSystem.Application.DTOs.Product;

public record class CreateProductResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int AvailableQuantity { get; set; }
    public string ImagePath { get; set; } = string.Empty;
}
