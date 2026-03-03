using System;
using OrderSystem.Domain.UnitOfWork;
using OrderSystem.Infrastructure.Data;

namespace OrderSystem.Infrastructure.UnitOfWork;

public class UnitOfWorkEf(AppDbContext dbContext) : IUnitOfWork
{
    public async Task<bool> CommitAsync()
    {
        await dbContext.SaveChangesAsync();
        return true;
    }

    public void Dispose()
    {
        dbContext.Dispose();
    }
}
