using AutoMapper;
using Kris.Server.Core.Requests;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<RegisterUserCommand, UserEntity>()
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.RegisterUser.Login))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.RegisterUser.Password))
            .ForMember(dest => dest.Created, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}
