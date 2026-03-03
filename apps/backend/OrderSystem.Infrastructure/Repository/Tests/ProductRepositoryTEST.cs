using System;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;

namespace OrderSystem.Infrastructure.Repository.Tests;

public class ProductRepositoryTEST : IProductRepository
{
    List<Product> products = new List<Product>()
    {
        Product.CreateProduct("Monitor", 1, 1, ""),
        Product.CreateProduct("Teclado", 2, 2, ""),
        Product.CreateProduct("Mouse", 3, 3, "")
    };

    public async Task<Entity> AddAsync(Entity entity)
    {
        products.Add((Product)entity);
        return entity;
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Entity>> GetAllAsync()
    {
        return products;
    }

    public async Task<Entity> GetByIdAsync(Guid Id)
    {
        return products.FirstOrDefault(p => p.Id == Id)!;
    }

    public async Task<Entity> UpdateAsync(Guid id, Entity updatedEntity)
    {
        var index = products.FindIndex(p => p.Id == id);
        products[index] = (Product)updatedEntity;

        return products[index];
    }
}
