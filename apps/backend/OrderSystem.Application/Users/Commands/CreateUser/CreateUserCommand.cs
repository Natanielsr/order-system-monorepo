using MediatR;
using OrderSystem.Application.DTOs.User;

namespace OrderSystem.Application.Users.Commands.CreateUser;

public record class CreateUserCommand(
    string Username,
    string Email,
    string Password,
    string ConfirmPassword
) : IRequest<CreateUserResponseDto>
{

}
