using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Films.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Films.Infrastructure.Extensions;

/// <summary>
/// Extension methods for registering infrastructure services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FilmsDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("FilmsConnection") ?? "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=films;Integrated Security=True"));

        services.AddScoped<IFilmRepository, FilmRepository>();

        return services;
    }
}
