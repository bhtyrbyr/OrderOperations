using AutoMapper;
using OrderOperations.Application.DTOs.AuthorizationDTOs;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Profiles;

internal class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<RegisterUserViewModel, Person>()
            .ForMember(desc => desc.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(desc => desc.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(desc => desc.PasswordHash, opt => opt.Ignore())
            .ForMember(desc => desc.SecurityStamp, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}
