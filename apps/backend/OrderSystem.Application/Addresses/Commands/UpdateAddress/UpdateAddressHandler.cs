using System;
using AutoMapper;
using MediatR;
using OrderSystem.Application.DTOs.Address;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.UnitOfWork;

namespace OrderSystem.Application.Addresses.Commands.UpdateAddress;

public class UpdateAddressHandler(IAddressRepository repository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateAddressCommand, AddressDto>
{
    public async Task<AddressDto> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        if (request.IsDefault)
        {
            var userAddresses = await repository.GetUserAddressesAsync(request.UserId);
            foreach (var a in userAddresses)
            {
                a.IsDefault = false;
            }
        }

        Address addressRequest = mapper.Map<Address>(request);
        Address addressResponse = (Address)await repository.UpdateAsync(request.Id, addressRequest);

        var success = await unitOfWork.CommitAsync();
        if (!success)
            throw new Exception("It was not possible to update the address in the repository.");

        AddressDto addressResponseDto = mapper.Map<AddressDto>(addressResponse);

        return addressResponseDto;

    }
}
