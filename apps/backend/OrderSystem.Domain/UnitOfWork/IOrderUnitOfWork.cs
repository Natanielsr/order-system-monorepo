using OrderSystem.Domain.Repository;

namespace OrderSystem.Domain.UnitOfWork;

public interface IOrderUnitOfWork : IUnitOfWork
{
    IOrderRepository orderRepository { get; }
    IProductRepository productRepository { get; }


}
