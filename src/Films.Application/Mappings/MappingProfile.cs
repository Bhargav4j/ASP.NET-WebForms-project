using AutoMapper;
using Films.Domain.DTOs;
using Films.Domain.Entities;

namespace Films.Application.Mappings;

/// <summary>
/// AutoMapper configuration profile
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Film mappings
        CreateMap<Film, FilmDto>();
        CreateMap<FilmCreateDto, Film>();
        CreateMap<FilmUpdateDto, Film>();

        // Actor mappings
        CreateMap<Actor, ActorDto>()
            .ForMember(dest => dest.SexName, opt => opt.MapFrom(src => src.Sex != null ? src.Sex.Name : null));
        CreateMap<ActorCreateDto, Actor>();
        CreateMap<ActorUpdateDto, Actor>();

        // Director mappings
        CreateMap<Director, DirectorDto>()
            .ForMember(dest => dest.SexName, opt => opt.MapFrom(src => src.Sex != null ? src.Sex.Name : null));
        CreateMap<DirectorCreateDto, Director>();
        CreateMap<DirectorUpdateDto, Director>();

        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.SexName, opt => opt.MapFrom(src => src.Sex != null ? src.Sex.Name : null))
            .ForMember(dest => dest.TypeUserName, opt => opt.MapFrom(src => src.TypeUser != null ? src.TypeUser.Name : null));
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>();
    }
}
