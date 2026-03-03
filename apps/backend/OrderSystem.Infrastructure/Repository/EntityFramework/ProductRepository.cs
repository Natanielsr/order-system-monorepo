using Microsoft.EntityFrameworkCore;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;
using OrderSystem.Infrastructure.Data;

namespace OrderSystem.Infrastructure.Repository.EntityFramework;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<Entity> AddAsync(Entity entity)
    {
        await context.Products.AddAsync((Product)entity);

        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await context.Products.FindAsync(id);

        if (product != null)
        {
            context.Products.Remove(product);
            return true;
        }

        return false;
    }

    public async Task<IEnumerable<Entity>> GetAllAsync()
    {
        return await context.Products.AsNoTracking().ToListAsync();
    }

    public async Task<Entity> GetByIdAsync(Guid id)
    {
        var order = await context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        return order!;
    }

    public async Task<Entity> UpdateAsync(Guid id, Entity updatedEntity)
    {
        var product = await context.Products.FindAsync(id);

        if (product != null)
        {
            product = (Product)updatedEntity;
            product.RenewUpdateDate();
        }

        return product!;
    }
}
