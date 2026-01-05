using Films.Domain.Entities;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Films.UnitTests.Infrastructure;

public class FilmsDbContextTests
{
    private DbContextOptions<FilmsDbContext> CreateInMemoryOptions()
    {
        return new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void FilmsDbContext_Constructor_ShouldInitialize()
    {
        // Arrange
        var options = CreateInMemoryOptions();

        // Act
        using var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context);
        Assert.NotNull(context.Films);
        Assert.NotNull(context.Actors);
        Assert.NotNull(context.Directors);
        Assert.NotNull(context.Users);
    }

    [Fact]
    public void FilmsDbContext_FilmsDbSet_ShouldBeAccessible()
    {
        // Arrange
        var options = CreateInMemoryOptions();

        // Act
        using var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context.Films);
    }

    [Fact]
    public void FilmsDbContext_ActorsDbSet_ShouldBeAccessible()
    {
        // Arrange
        var options = CreateInMemoryOptions();

        // Act
        using var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context.Actors);
    }

    [Fact]
    public void FilmsDbContext_DirectorsDbSet_ShouldBeAccessible()
    {
        // Arrange
        var options = CreateInMemoryOptions();

        // Act
        using var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context.Directors);
    }

    [Fact]
    public void FilmsDbContext_UsersDbSet_ShouldBeAccessible()
    {
        // Arrange
        var options = CreateInMemoryOptions();

        // Act
        using var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context.Users);
    }

    [Fact]
    public void FilmsDbContext_SexesDbSet_ShouldBeAccessible()
    {
        // Arrange
        var options = CreateInMemoryOptions();

        // Act
        using var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context.Sexes);
    }

    [Fact]
    public void FilmsDbContext_TypeUsersDbSet_ShouldBeAccessible()
    {
        // Arrange
        var options = CreateInMemoryOptions();

        // Act
        using var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context.TypeUsers);
    }

    [Fact]
    public void FilmsDbContext_RightsDbSet_ShouldBeAccessible()
    {
        // Arrange
        var options = CreateInMemoryOptions();

        // Act
        using var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context.Rights);
    }

    [Fact]
    public void FilmsDbContext_RefAFsDbSet_ShouldBeAccessible()
    {
        // Arrange
        var options = CreateInMemoryOptions();

        // Act
        using var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context.RefAFs);
    }

    [Fact]
    public void FilmsDbContext_RefDAFsDbSet_ShouldBeAccessible()
    {
        // Arrange
        var options = CreateInMemoryOptions();

        // Act
        using var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context.RefDAFs);
    }

    [Fact]
    public void FilmsDbContext_UserRightsDbSet_ShouldBeAccessible()
    {
        // Arrange
        var options = CreateInMemoryOptions();

        // Act
        using var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context.UserRights);
    }

    [Fact]
    public async Task FilmsDbContext_AddFilm_ShouldAddSuccessfully()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new FilmsDbContext(options);
        var film = new Film
        {
            Name = "Test Film",
            Year = 2020,
            IsActive = true,
            CreatedBy = "test"
        };

        // Act
        context.Films.Add(film);
        await context.SaveChangesAsync();

        // Assert
        var savedFilm = await context.Films.FirstOrDefaultAsync(f => f.Name == "Test Film");
        Assert.NotNull(savedFilm);
        Assert.Equal("Test Film", savedFilm.Name);
    }

    [Fact]
    public async Task FilmsDbContext_AddActor_ShouldAddSuccessfully()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new FilmsDbContext(options);
        var actor = new Actor
        {
            Name = "Test Actor",
            IsActive = true,
            CreatedBy = "test"
        };

        // Act
        context.Actors.Add(actor);
        await context.SaveChangesAsync();

        // Assert
        var savedActor = await context.Actors.FirstOrDefaultAsync(a => a.Name == "Test Actor");
        Assert.NotNull(savedActor);
        Assert.Equal("Test Actor", savedActor.Name);
    }

    [Fact]
    public async Task FilmsDbContext_AddDirector_ShouldAddSuccessfully()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new FilmsDbContext(options);
        var director = new Director
        {
            Name = "Test Director",
            IsActive = true,
            CreatedBy = "test"
        };

        // Act
        context.Directors.Add(director);
        await context.SaveChangesAsync();

        // Assert
        var savedDirector = await context.Directors.FirstOrDefaultAsync(d => d.Name == "Test Director");
        Assert.NotNull(savedDirector);
        Assert.Equal("Test Director", savedDirector.Name);
    }

    [Fact]
    public async Task FilmsDbContext_AddUser_ShouldAddSuccessfully()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new FilmsDbContext(options);
        var user = new User
        {
            Username = "testuser",
            Password = "password",
            IsActive = true,
            CreatedBy = "test"
        };

        // Act
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Assert
        var savedUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
        Assert.NotNull(savedUser);
        Assert.Equal("testuser", savedUser.Username);
    }
}
