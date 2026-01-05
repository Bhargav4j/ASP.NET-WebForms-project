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
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
        services.AddScoped<IFilmService, FilmService>();

        return services;
    }
}
