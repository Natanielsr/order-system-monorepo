
using OrderSystem.Domain.Entities;

namespace OrderSystem.Domain.Repository;

public interface IUserRepository : IRepository
{
    public Task<User> GetByUserNameAsync(string username);
    public Task<User> GetByEmailAsync(string email);
}
