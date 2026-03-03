using System;
using AutoMapper;
using MediatR;
using OrderSystem.Application.DTOs.Address;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Exceptions;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.UnitOfWork;

namespace OrderSystem.Application.Addresses.Commands.CreateAddress;

public class CreateAddressHandler(IAddressRepository repository, IMapper mapper, IUnitOfWork unitOfWork) : IRequestHandler<CreateAddressCommand, AddressDto>
{
    public async Task<AddressDto> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        Address newAddress = mapper.Map<Address>(request);

        var userAddresses = await repository.GetUserAddressesAsync(request.UserId);
        if (userAddresses.Count() > 5)
            throw new AddressCountExceededException();

        if (request.IsDefault)
        {
            foreach (var address in userAddresses)
            {
                address.IsDefault = false;
            }
        }

        var response = await repository.AddAsync(newAddress);
        await unitOfWork.CommitAsync();

        AddressDto addressDto = mapper.Map<AddressDto>(response);

        return addressDto;
    }
}
