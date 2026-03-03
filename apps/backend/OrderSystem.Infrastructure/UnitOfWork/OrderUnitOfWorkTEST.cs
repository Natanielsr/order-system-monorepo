using System;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.UnitOfWork;

namespace OrderSystem.Infrastructure.UnitOfWork;

public class OrderUnitOfWorkTEST : IOrderUnitOfWork
{
    public IOrderRepository orderRepository { get; private set; }

    public IProductRepository productRepository { get; private set; }

    public OrderUnitOfWorkTEST(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        this.orderRepository = orderRepository;
        this.productRepository = productRepository;
    }

    public async Task<bool> CommitAsync()
    {
        await Task.Delay(200);
        return true;
    }

    public void Dispose()
    {
        Console.WriteLine("OrderUnitOfWorkTEST Dispose");
    }
}
