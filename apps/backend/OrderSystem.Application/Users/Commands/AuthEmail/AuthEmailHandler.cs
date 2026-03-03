using System;
using AutoMapper;
using MediatR;
using OrderSystem.Application.DTOs.User;
using OrderSystem.Application.Services;
using OrderSystem.Domain.Exceptions;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.Services;

namespace OrderSystem.Application.Users.Commands.AuthEmail;

public class AuthEmailHandler(
    IUserRepository userRepository,
    IPasswordService passwordService,
    IMapper mapper,
    ITokenService tokenService
    ) : IRequestHandler<AuthEmailCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(AuthEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.email);
        if (user == null)
            throw new UserNotFoundException();

        var isPasswordCorrect = passwordService.VerifyPassowrd(request.password, user.HashedPassword!);
        if (!isPasswordCorrect)
            throw new InvalidPasswordException();

        AuthResponseDto authResponseDto = mapper.Map<AuthResponseDto>(user);
        authResponseDto.Token = tokenService.GenerateToken(user); //lógica para criar token

        return authResponseDto;
    }
}
