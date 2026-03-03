using System;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;

namespace OrderSystem.Infrastructure.Repository.Tests;

public class OrderRepositoryTEST : IOrderRepository
{
    List<Order> orders = new List<Order>();
    public async Task<Entity> AddAsync(Entity entity)
    {
        orders.Add((Order)entity);
        return entity;
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Entity>> GetAllAsync()
    {
        return orders;
    }

    public Task<List<Order>> GetAllUserOrdersAsync(Guid UserId, int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<Entity> GetByIdAsync(Guid Id)
    {
        return orders.First(o => o.Id == Id);
    }

    public Task<Entity> UpdateAsync(Guid id, Entity updatedEntity)
    {
        throw new NotImplementedException();
    }
}
