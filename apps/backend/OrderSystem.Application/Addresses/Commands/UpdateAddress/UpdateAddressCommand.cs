using MediatR;
using OrderSystem.Application.DTOs.Address;

namespace OrderSystem.Application.Addresses.Commands.UpdateAddress;

public record class UpdateAddressCommand(
    Guid Id,
    string FullName,
    string Cpf,
    string Street,
    string Number,
    string? Complement,
    string Neighborhood,
    string City,
    string State,
    string ZipCode,
    string Country,
    Guid UserId,
    bool IsDefault

) : IRequest<AddressDto>
{
}
