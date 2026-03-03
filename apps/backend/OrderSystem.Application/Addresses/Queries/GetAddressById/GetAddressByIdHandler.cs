using System;
using AutoMapper;
using MediatR;
using OrderSystem.Application.DTOs.Address;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;

namespace OrderSystem.Application.Addresses.Queries.GetAddressById;

public class GetAddressByIdHandler(IAddressRepository repository, IMapper mapper) : IRequestHandler<GetAddressByIdQuery, AddressDto>
{
    public async Task<AddressDto> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
    {
        Address address = (Address)await repository.GetByIdAsync(request.id);
        AddressDto addressDto = mapper.Map<AddressDto>(address);

        return addressDto;
    }
}
