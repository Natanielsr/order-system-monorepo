using System;
using MediatR;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.UnitOfWork;

namespace OrderSystem.Application.Addresses.Commands.DeleteAddress;

public class DeleteAddressHandler(IAddressRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteAddressCommand, bool>
{
    public async Task<bool> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var response = await repository.DeleteAsync(request.AddressId);
        if (response)
            response = await unitOfWork.CommitAsync();
        return response;
    }
}
