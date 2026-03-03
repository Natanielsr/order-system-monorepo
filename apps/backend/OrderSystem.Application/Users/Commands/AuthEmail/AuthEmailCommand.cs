using MediatR;
using OrderSystem.Application.DTOs.User;

namespace OrderSystem.Application.Users.Commands.AuthEmail;

public record class AuthEmailCommand(string email, string password) : IRequest<AuthResponseDto>
{

}
