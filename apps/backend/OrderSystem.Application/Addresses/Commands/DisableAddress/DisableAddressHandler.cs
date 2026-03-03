using System;
using AutoMapper;
using MediatR;
using OrderSystem.Application.DTOs.Address;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.UnitOfWork;

namespace OrderSystem.Application.Addresses.Commands.DisableAddress;

public class DisableAddressHandler(
    IAddressRepository addressRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<DisableAddressCommand, AddressDto>
{
    public async Task<AddressDto> Handle(DisableAddressCommand request, CancellationToken cancellationToken)
    {
        var response = await addressRepository.DisableAsync(request.AddressId);
        await unitOfWork.CommitAsync();
        var addressDto = mapper.Map<AddressDto>(response);

        return addressDto;
    }
}
