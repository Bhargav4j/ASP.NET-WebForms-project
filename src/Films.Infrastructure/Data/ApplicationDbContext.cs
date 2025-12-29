using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Data;

/// <summary>
/// Application database context
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Film> Films { get; set; } = null!;
    public DbSet<Actor> Actors { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<DirectedBy> DirectedBys { get; set; } = null!;
    public DbSet<Sex> Sexes { get; set; } = null!;
    public DbSet<TypeUser> TypeUsers { get; set; } = null!;
    public DbSet<Right> Rights { get; set; } = null!;
    public DbSet<RefAF> RefAFs { get; set; } = null!;
    public DbSet<RefDAF> RefDAFs { get; set; } = null!;
    public DbSet<UserRight> UserRights { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations from the assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
