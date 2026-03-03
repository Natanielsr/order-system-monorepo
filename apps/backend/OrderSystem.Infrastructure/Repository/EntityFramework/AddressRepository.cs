using System;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;
using OrderSystem.Infrastructure.Data;

namespace OrderSystem.Infrastructure.Repository.EntityFramework;

public class AddressRepository(AppDbContext context) : IAddressRepository
{
    public async Task<Entity> AddAsync(Entity entity)
    {
        await context.Addresses.AddAsync((Address)entity);

        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var address = await context.Addresses.FindAsync(id);

        if (address != null)
        {
            context.Addresses.Remove(address);
            return true;
        }

        return false;
    }

    public async Task<Address> DisableAsync(Guid AddressId)
    {
        Address? address = await context.Addresses.FindAsync(AddressId);

        if (address != null)
        {
            address.Disable();
        }

        return address!;
    }

    public async Task<IEnumerable<Entity>> GetAllAsync()
    {
        return await context.Addresses
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Entity> GetByIdAsync(Guid id)
    {
        var address = await context.Addresses
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);

        return address!;
    }

    public async Task<List<Address>> GetUserAddressesAsync(Guid UserId)
    {
        return await context.Addresses
            .Where(a => a.UserId == UserId)
            .Where(a => a.Active == true) //get only actives addresses
            .OrderByDescending(o => o.CreationDate)
            .ToListAsync();
    }

    public async Task<Entity> UpdateAsync(Guid id, Entity updatedEntity)
    {
        Address updateAddress = (Address)updatedEntity;
        var address = await context.Addresses.FindAsync(id);

        if (address != null)
        {
            address.RenewUpdateDate();
            address.FullName = updateAddress.FullName;
            address.Cpf = updateAddress.Cpf;
            address.ZipCode = updateAddress.ZipCode;
            address.Street = updateAddress.Street;
            address.Number = updateAddress.Number;
            address.Neighborhood = updateAddress.Neighborhood;
            address.Complement = updateAddress.Complement;
            address.City = updateAddress.City;
            address.State = updateAddress.State;
            address.IsDefault = updateAddress.IsDefault;
        }

        return address!;
    }
}
