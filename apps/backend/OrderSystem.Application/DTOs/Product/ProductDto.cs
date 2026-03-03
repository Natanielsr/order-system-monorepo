namespace OrderSystem.Application.DTOs.Product;

public record class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int AvailableQuantity { get; private set; }
    public string ImagePath { get; set; } = string.Empty;
}
