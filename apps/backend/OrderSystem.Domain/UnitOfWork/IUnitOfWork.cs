using System;

namespace OrderSystem.Domain.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    public Task<bool> CommitAsync();
}
