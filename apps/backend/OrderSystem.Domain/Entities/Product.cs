using System.ComponentModel.DataAnnotations;
using OrderSystem.Domain.Exceptions;

namespace OrderSystem.Domain.Entities;

public class Product : Entity
{
    public required string Name { get; set; } = string.Empty;
    public required decimal Price { get; set; }

    public required int AvailableQuantity { get; set; }

    public required string ImagePath { get; set; }

    public int Version { get; set; }

    public Product() { }

    public static Product CreateProduct(
        string name,
        decimal price,
        int availableQuantity,
        string imagePath
    )
    {
        return new Product()
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Active = true,
            Name = name,
            Price = price,
            AvailableQuantity = availableQuantity,
            ImagePath = imagePath,
        };
    }

    public int ReduceInStock(int Quantity)
    {
        if (Quantity <= 0)
        {
            throw new Exception("Quantity must be bigger then zero");
        }

        if (Quantity > AvailableQuantity)
        {
            throw new QuantityProductInStockOverflowException();
        }


        this.AvailableQuantity -= Quantity;
        Version++; //change version to avoid concorrency andrace condition

        return this.AvailableQuantity;
    }
}
