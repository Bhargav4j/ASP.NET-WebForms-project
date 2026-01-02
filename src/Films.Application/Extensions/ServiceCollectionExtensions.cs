using AutoMapper;
using Films.Application.Interfaces;
using Films.Application.Services;
using Films.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace Films.Application.Extensions;

/// <summary>
/// Extension methods for registering Application layer services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        services.AddSingleton(mapperConfig.CreateMapper());

        // Register services
        services.AddScoped<IFilmService, FilmService>();
        services.AddScoped<IActorService, ActorService>();
        services.AddScoped<IDirectorService, DirectorService>();

        return services;
    }
}
