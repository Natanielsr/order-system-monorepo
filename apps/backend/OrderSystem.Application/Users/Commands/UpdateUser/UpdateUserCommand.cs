using MediatR;
using OrderSystem.Application.DTOs.User;

namespace OrderSystem.Application.Users.Commands.UpdateUser;

public record class UpdateUserCommand(
    Guid id,
    string Phone
    ) : IRequest<UserDto>
{

}
