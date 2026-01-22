using Microsoft.Extensions.DependencyInjection;
using Films.Domain.Interfaces.Services;
using Films.Application.Services;

namespace Films.Application.Extensions;

/// <summary>
/// Extension methods for registering application services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IFilmService, FilmService>();
        services.AddScoped<IActorService, ActorService>();
        services.AddScoped<IDirectorService, DirectorService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
