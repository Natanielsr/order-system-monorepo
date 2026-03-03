using MediatR;
using OrderSystem.Application.DTOs.User;

namespace OrderSystem.Application.Users.Commands.GetUser;

public record class GetUserByIdCommand(Guid UserId) : IRequest<UserDto>
{

}
