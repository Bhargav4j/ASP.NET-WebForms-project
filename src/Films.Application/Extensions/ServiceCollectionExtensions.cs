using Films.Application.Mappings;
using Films.Application.Services;
using Films.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Films.Application.Extensions;

/// <summary>
/// Extension methods for registering application services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // Register services
        services.AddScoped<IFilmService, FilmService>();
        services.AddScoped<IActorService, ActorService>();

        return services;
    }
}
