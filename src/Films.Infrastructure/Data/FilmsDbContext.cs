using Microsoft.EntityFrameworkCore;
using Films.Domain.Entities;

namespace Films.Infrastructure.Data;

/// <summary>
/// Database context for the Films application
/// </summary>
public class FilmsDbContext : DbContext
{
    public FilmsDbContext(DbContextOptions<FilmsDbContext> options) : base(options)
    {
    }

    public DbSet<Film> Films => Set<Film>();
    public DbSet<Actor> Actors => Set<Actor>();
    public DbSet<Director> Directors => Set<Director>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Sex> Sexes => Set<Sex>();
    public DbSet<TypeUser> TypeUsers => Set<TypeUser>();
    public DbSet<Right> Rights => Set<Right>();
    public DbSet<UserRight> UserRights => Set<UserRight>();
    public DbSet<RefAF> RefAFs => Set<RefAF>();
    public DbSet<RefDAF> RefDAFs => Set<RefDAF>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FilmsDbContext).Assembly);
    }
}
