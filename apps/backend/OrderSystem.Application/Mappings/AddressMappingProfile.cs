using System;
using AutoMapper;
using OrderSystem.Application.Addresses.Commands.CreateAddress;
using OrderSystem.Application.Addresses.Commands.UpdateAddress;
using OrderSystem.Application.DTOs.Address;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Application.Mappings;

public class AddressMappingProfile : Profile
{
    public AddressMappingProfile()
    {
        CreateMap<CreateAddressCommand, Address>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.Active, opt => opt.MapFrom(src => true));

        CreateMap<Address, AddressDto>();
        CreateMap<UpdateAddressCommand, Address>();
    }
}
