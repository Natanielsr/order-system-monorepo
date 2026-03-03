using MediatR;
using OrderSystem.Application.DTOs.Address;

namespace OrderSystem.Application.Addresses.Queries.GetAddressById;

public record class GetAddressByIdQuery(Guid id) : IRequest<AddressDto>
{

}
