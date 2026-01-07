using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Films.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Films.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FilmsDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IFilmRepository, FilmRepository>();
        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<IDirectorRepository, DirectorRepository>();
        services.AddScoped<ISexRepository, SexRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
