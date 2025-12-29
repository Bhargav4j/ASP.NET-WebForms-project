using AutoMapper;
using Films.Domain.DTOs;
using Films.Domain.Entities;

namespace Films.Application.Mappings;

/// <summary>
/// AutoMapper profile for entity to DTO mappings
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Film mappings
        CreateMap<Film, FilmDto>();
        CreateMap<FilmCreateDto, Film>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(_ => "System"))
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.RefAFs, opt => opt.Ignore())
            .ForMember(dest => dest.RefDAFs, opt => opt.Ignore());

        CreateMap<FilmUpdateDto, Film>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(_ => "System"))
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.RefAFs, opt => opt.Ignore())
            .ForMember(dest => dest.RefDAFs, opt => opt.Ignore());

        // Actor mappings
        CreateMap<Actor, ActorDto>()
            .ForMember(dest => dest.SexName, opt => opt.MapFrom(src => src.Sex != null ? src.Sex.Name : null));

        CreateMap<ActorCreateDto, Actor>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(_ => "System"))
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Sex, opt => opt.Ignore())
            .ForMember(dest => dest.RefAFs, opt => opt.Ignore());

        CreateMap<ActorUpdateDto, Actor>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(_ => "System"))
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.Sex, opt => opt.Ignore())
            .ForMember(dest => dest.RefAFs, opt => opt.Ignore());

        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.TypeUserName, opt => opt.MapFrom(src => src.TypeUser != null ? src.TypeUser.Name : null));

        CreateMap<UserCreateDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(_ => "System"))
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.TypeUser, opt => opt.Ignore())
            .ForMember(dest => dest.UserRights, opt => opt.Ignore());

        CreateMap<UserUpdateDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(_ => "System"))
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.TypeUser, opt => opt.Ignore())
            .ForMember(dest => dest.UserRights, opt => opt.Ignore());

        // DirectedBy mappings
        CreateMap<DirectedBy, DirectedByDto>();
        CreateMap<DirectedByCreateDto, DirectedBy>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(_ => "System"))
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.RefDAFs, opt => opt.Ignore());
    }
}

public class DirectedByDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}

public class DirectedByCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
