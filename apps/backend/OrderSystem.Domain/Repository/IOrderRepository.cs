using OrderSystem.Domain.Entities;

namespace OrderSystem.Domain.Repository;

public interface IOrderRepository : IRepository
{
    public Task<List<Order>> GetAllUserOrdersAsync(Guid UserId, int page, int pageSize);
}
