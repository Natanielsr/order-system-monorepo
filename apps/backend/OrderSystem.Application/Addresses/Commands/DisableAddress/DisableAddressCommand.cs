using MediatR;
using OrderSystem.Application.DTOs.Address;

namespace OrderSystem.Application.Addresses.Commands.DisableAddress;

public record class DisableAddressCommand(Guid AddressId) : IRequest<AddressDto>
{

}
