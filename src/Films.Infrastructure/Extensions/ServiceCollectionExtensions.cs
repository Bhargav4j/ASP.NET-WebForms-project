using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Films.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Films.Infrastructure.Extensions;

/// <summary>
/// Extension methods for configuring infrastructure services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<FilmsDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IFilmRepository, FilmRepository>();
        services.AddScoped<IActorRepository, ActorRepository>();

        return services;
    }
}
