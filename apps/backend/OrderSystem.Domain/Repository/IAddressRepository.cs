using System;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Domain.Repository;

public interface IAddressRepository : IRepository
{
    public Task<List<Address>> GetUserAddressesAsync(Guid UserId);

    public Task<Address> DisableAsync(Guid AddressId);
}
