using MediatR;
using OrderSystem.Application.DTOs.Address;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Application.Addresses.Queries.GetUserAddresses;

public record class GetUserAddressesQuery(Guid UserId) : IRequest<List<AddressDto>>
{

}
