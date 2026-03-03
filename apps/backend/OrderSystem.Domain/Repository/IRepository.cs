using OrderSystem.Domain.Entities;

namespace OrderSystem.Domain.Repository;

public interface IRepository
{
    public Task<Entity> AddAsync(Entity entity);
    public Task<IEnumerable<Entity>> GetAllAsync();
    public Task<Entity> GetByIdAsync(Guid id);
    public Task<bool> DeleteAsync(Guid id);
    public Task<Entity> UpdateAsync(Guid id, Entity updatedEntity);
}
