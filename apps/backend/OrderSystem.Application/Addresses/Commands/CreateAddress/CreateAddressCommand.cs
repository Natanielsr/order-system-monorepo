using MediatR;
using OrderSystem.Application.DTOs.Address;

namespace OrderSystem.Application.Addresses.Commands.CreateAddress;

public record class CreateAddressCommand(
    string FullName,
    string Cpf,
    string Street,
    string Number,
    string Complement,
    string Neighborhood,
    string City,
    string State,
    string ZipCode,
    Guid UserId,
    bool IsDefault
) : IRequest<AddressDto>
{

}
