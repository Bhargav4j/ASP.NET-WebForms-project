using Xunit;
using Films.Infrastructure.Data;
using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Tests;

public class FilmsDbContextTests
{
    [Fact]
    public void Constructor_WithValidOptions_CreatesContext()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Act
        var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context);
        Assert.NotNull(context.Films);
        Assert.NotNull(context.Actors);
        Assert.NotNull(context.Users);
        Assert.NotNull(context.DirectedBys);
        Assert.NotNull(context.Sexes);
        Assert.NotNull(context.TypeUsers);
        Assert.NotNull(context.Rights);
        Assert.NotNull(context.RefAFs);
        Assert.NotNull(context.RefDAFs);
        Assert.NotNull(context.UserRights);
    }

    [Fact]
    public async Task Films_CanAddAndRetrieve()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new FilmsDbContext(options);
        var film = new Film { Name = "Test Film", IsActive = true };

        // Act
        context.Films.Add(film);
        await context.SaveChangesAsync();

        // Assert
        var retrieved = await context.Films.FirstOrDefaultAsync();
        Assert.NotNull(retrieved);
        Assert.Equal("Test Film", retrieved.Name);
    }

    [Fact]
    public async Task Actors_CanAddAndRetrieve()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new FilmsDbContext(options);
        var actor = new Actor { Name = "Test Actor", IsActive = true };

        // Act
        context.Actors.Add(actor);
        await context.SaveChangesAsync();

        // Assert
        var retrieved = await context.Actors.FirstOrDefaultAsync();
        Assert.NotNull(retrieved);
        Assert.Equal("Test Actor", retrieved.Name);
    }

    [Fact]
    public async Task Users_CanAddAndRetrieve()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new FilmsDbContext(options);
        var user = new User
        {
            Username = "testuser",
            Password = "pass",
            Email = "test@test.com",
            IsActive = true
        };

        // Act
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Assert
        var retrieved = await context.Users.FirstOrDefaultAsync();
        Assert.NotNull(retrieved);
        Assert.Equal("testuser", retrieved.Username);
    }

    [Fact]
    public async Task RefAF_CanAddAndRetrieve()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new FilmsDbContext(options);
        var film = new Film { Name = "Test Film", IsActive = true };
        var actor = new Actor { Name = "Test Actor", IsActive = true };
        context.Films.Add(film);
        context.Actors.Add(actor);
        await context.SaveChangesAsync();

        var refAF = new RefAF { FilmId = film.Id, ActorId = actor.Id, IsActive = true };

        // Act
        context.RefAFs.Add(refAF);
        await context.SaveChangesAsync();

        // Assert
        var retrieved = await context.RefAFs
            .Include(r => r.Film)
            .Include(r => r.Actor)
            .FirstOrDefaultAsync();
        Assert.NotNull(retrieved);
        Assert.Equal(film.Id, retrieved.FilmId);
        Assert.Equal(actor.Id, retrieved.ActorId);
    }

    [Fact]
    public async Task RefDAF_CanAddAndRetrieve()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new FilmsDbContext(options);
        var film = new Film { Name = "Test Film", IsActive = true };
        var director = new DirectedBy { Name = "Test Director", IsActive = true };
        context.Films.Add(film);
        context.DirectedBys.Add(director);
        await context.SaveChangesAsync();

        var refDAF = new RefDAF { FilmId = film.Id, DirectedById = director.Id, IsActive = true };

        // Act
        context.RefDAFs.Add(refDAF);
        await context.SaveChangesAsync();

        // Assert
        var retrieved = await context.RefDAFs
            .Include(r => r.Film)
            .Include(r => r.DirectedBy)
            .FirstOrDefaultAsync();
        Assert.NotNull(retrieved);
        Assert.Equal(film.Id, retrieved.FilmId);
        Assert.Equal(director.Id, retrieved.DirectedById);
    }

    [Fact]
    public async Task UserRight_CanAddAndRetrieve()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new FilmsDbContext(options);
        var user = new User
        {
            Username = "testuser",
            Password = "pass",
            Email = "test@test.com",
            IsActive = true
        };
        var right = new Right { Name = "Admin", IsActive = true };
        context.Users.Add(user);
        context.Rights.Add(right);
        await context.SaveChangesAsync();

        var userRight = new UserRight { UserId = user.Id, RightId = right.Id, IsActive = true };

        // Act
        context.UserRights.Add(userRight);
        await context.SaveChangesAsync();

        // Assert
        var retrieved = await context.UserRights
            .Include(ur => ur.User)
            .Include(ur => ur.Right)
            .FirstOrDefaultAsync();
        Assert.NotNull(retrieved);
        Assert.Equal(user.Id, retrieved.UserId);
        Assert.Equal(right.Id, retrieved.RightId);
    }

    [Fact]
    public async Task SaveChangesAsync_PersistsChanges()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new FilmsDbContext(options);
        var film = new Film { Name = "Original", IsActive = true };
        context.Films.Add(film);
        await context.SaveChangesAsync();

        // Act
        film.Name = "Updated";
        await context.SaveChangesAsync();

        // Assert
        var retrieved = await context.Films.FirstOrDefaultAsync();
        Assert.Equal("Updated", retrieved?.Name);
    }
}
