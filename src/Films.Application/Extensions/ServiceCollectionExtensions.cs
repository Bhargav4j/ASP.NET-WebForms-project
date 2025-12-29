using Films.Application.Services;
using Films.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Films.Application.Extensions;

/// <summary>
/// Extension methods for configuring application services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

        // Register services
        services.AddScoped<IFilmService, FilmService>();
        services.AddScoped<IActorService, ActorService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
