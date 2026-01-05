using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Data;

/// <summary>
/// Database context for Films application
/// </summary>
public class FilmsDbContext : DbContext
{
    public FilmsDbContext(DbContextOptions<FilmsDbContext> options) : base(options)
    {
    }

    public DbSet<Film> Films { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Director> Directors { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Sex> Sexes { get; set; }
    public DbSet<TypeUser> TypeUsers { get; set; }
    public DbSet<Right> Rights { get; set; }
    public DbSet<RefAF> RefAFs { get; set; }
    public DbSet<RefDAF> RefDAFs { get; set; }
    public DbSet<UserRight> UserRights { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FilmsDbContext).Assembly);
    }
}
