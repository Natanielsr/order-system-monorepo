using System;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;
using OrderSystem.Infrastructure.Data;

namespace OrderSystem.Infrastructure.Repository.EntityFramework;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task<Entity> AddAsync(Entity entity)
    {
        Order order = (Order)entity;

        await context.Orders.AddAsync((Order)entity);

        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var order = await context.Orders.FindAsync(id);

        if (order != null)
        {
            context.Orders.Remove(order);
            return true;
        }

        return false;
    }

    public async Task<IEnumerable<Entity>> GetAllAsync()
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Order>> GetAllUserOrdersAsync(Guid UserId, int page, int pageSize)
    {
        return await context.Orders
            .AsNoTracking()
            .Where(o => o.UserId == UserId)
            .OrderByDescending(o => o.CreationDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    }

    public async Task<Entity> GetByIdAsync(Guid id)
    {
        var order = await context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.PaymentInfo)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);

        return order!;
    }

    public async Task<Entity> UpdateAsync(Guid id, Entity updatedEntity)
    {
        var order = await context.Orders.FindAsync(id);

        if (order != null)
        {
            order.RenewUpdateDate();
            order = (Order)updatedEntity;
        }

        return order!;
    }
}
