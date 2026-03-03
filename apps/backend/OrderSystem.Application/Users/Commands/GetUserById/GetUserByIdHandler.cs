using System;
using AutoMapper;
using MediatR;
using OrderSystem.Application.DTOs.User;
using OrderSystem.Domain.Repository;

namespace OrderSystem.Application.Users.Commands.GetUser;

public class GetUserByIdHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUserByIdCommand, UserDto>
{
    public async Task<UserDto> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
    {
        var response = await userRepository.GetByIdAsync(request.UserId);
        var dto = mapper.Map<UserDto>(response);

        return dto;
    }
}
