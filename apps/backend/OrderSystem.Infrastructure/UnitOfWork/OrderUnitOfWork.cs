using System;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.UnitOfWork;
using OrderSystem.Infrastructure.Data;

namespace OrderSystem.Infrastructure.UnitOfWork;

public class OrderUnitOfWork : IOrderUnitOfWork
{
    private readonly AppDbContext dbContext;
    public IOrderRepository orderRepository { get; private set; }

    public IProductRepository productRepository { get; private set; }

    public OrderUnitOfWork(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        AppDbContext dbContext)
    {
        this.dbContext = dbContext;
        this.orderRepository = orderRepository;
        this.productRepository = productRepository;
    }

    public async Task<bool> CommitAsync()
    {
        await dbContext.SaveChangesAsync();
        return true;
    }

    public void Dispose()
    {
        dbContext.Dispose();
        Console.WriteLine("EF Dispose");
    }
}
