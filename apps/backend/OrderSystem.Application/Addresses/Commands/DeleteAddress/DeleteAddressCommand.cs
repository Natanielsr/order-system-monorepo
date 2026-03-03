using MediatR;

namespace OrderSystem.Application.Addresses.Commands.DeleteAddress;

public record class DeleteAddressCommand(Guid AddressId) : IRequest<bool>
{

}
