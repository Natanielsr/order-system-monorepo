using MediatR;
using OrderSystem.Application.DTOs.User;

namespace OrderSystem.Application.Users.Commands.Auth;

public record class AuthCommand(string username, string password) : IRequest<AuthResponseDto>
{

}
