using AutoMapper;
using MediatR;
using OrderSystem.Application.DTOs.Address;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;

namespace OrderSystem.Application.Addresses.Queries.GetUserAddresses;

public class GetUserAddressesHandler(IAddressRepository repository, IMapper mapper) : IRequestHandler<GetUserAddressesQuery, List<AddressDto>>
{
    public async Task<List<AddressDto>> Handle(GetUserAddressesQuery request, CancellationToken cancellationToken)
    {
        List<Address> addresses = await repository.GetUserAddressesAsync(request.UserId);
        var addressesDto = mapper.Map<List<AddressDto>>(addresses);

        return addressesDto;
    }
}
