using System;
using AutoMapper;
using OrderSystem.Application.DTOs.User;
using OrderSystem.Application.Users.Commands.CreateUser;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateUserCommand, User>();
        CreateMap<User, CreateUserResponseDto>();
        CreateMap<User, AuthResponseDto>();
        CreateMap<User, UserDto>();
    }
}
