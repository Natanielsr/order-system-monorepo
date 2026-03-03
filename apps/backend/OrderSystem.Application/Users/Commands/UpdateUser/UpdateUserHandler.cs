using System;
using AutoMapper;
using MediatR;
using OrderSystem.Application.DTOs.User;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.UnitOfWork;

namespace OrderSystem.Application.Users.Commands.UpdateUser;

public class UpdateUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = (User)await userRepository.GetByIdAsync(request.id);
        if (user == null)
            return null!;

        user.Phone = request.Phone;
        user.RenewUpdateDate();
        var response = await userRepository.UpdateAsync(request.id, user);

        var success = await unitOfWork.CommitAsync();
        if (success)
        {
            var dto = mapper.Map<UserDto>(response);
            return dto;
        }
        else
            return null!;
    }
}
