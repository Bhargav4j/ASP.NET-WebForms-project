using Xunit;
using Films.Infrastructure.Data;
using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Films.Infrastructure.Data.Tests;

public class FilmsDbContextTests
{
    private DbContextOptions<FilmsDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void FilmsDbContext_Constructor_InitializesSuccessfully()
    {
        // Arrange
        var options = CreateNewContextOptions();

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
    public async Task FilmsDbContext_AddFilm_SavesSuccessfully()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var film = new Film
        {
            Title = "Test Film",
            Description = "Test Description",
            Year = 2023,
            Genre = "Drama",
            CreatedBy = "Test",
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        // Act
        using (var context = new FilmsDbContext(options))
        {
            context.Films.Add(film);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new FilmsDbContext(options))
        {
            var savedFilm = await context.Films.FirstOrDefaultAsync();
            Assert.NotNull(savedFilm);
            Assert.Equal("Test Film", savedFilm.Title);
        }
    }

    [Fact]
    public async Task FilmsDbContext_AddActor_SavesSuccessfully()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var actor = new Actor
        {
            Name = "Test Actor",
            Description = "Test Description",
            CreatedBy = "Test",
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        // Act
        using (var context = new FilmsDbContext(options))
        {
            context.Actors.Add(actor);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new FilmsDbContext(options))
        {
            var savedActor = await context.Actors.FirstOrDefaultAsync();
            Assert.NotNull(savedActor);
            Assert.Equal("Test Actor", savedActor.Name);
        }
    }

    [Fact]
    public async Task FilmsDbContext_AddDirector_SavesSuccessfully()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var director = new Director
        {
            Name = "Test Director",
            Description = "Test Description",
            CreatedBy = "Test",
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        // Act
        using (var context = new FilmsDbContext(options))
        {
            context.Directors.Add(director);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new FilmsDbContext(options))
        {
            var savedDirector = await context.Directors.FirstOrDefaultAsync();
            Assert.NotNull(savedDirector);
            Assert.Equal("Test Director", savedDirector.Name);
        }
    }

    [Fact]
    public async Task FilmsDbContext_AddUser_SavesSuccessfully()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hashedpassword",
            CreatedBy = "Test",
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        // Act
        using (var context = new FilmsDbContext(options))
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new FilmsDbContext(options))
        {
            var savedUser = await context.Users.FirstOrDefaultAsync();
            Assert.NotNull(savedUser);
            Assert.Equal("testuser", savedUser.Username);
        }
    }

    [Fact]
    public void FilmsDbContext_AllDbSets_AreAccessible()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context.Films);
        Assert.NotNull(context.Actors);
        Assert.NotNull(context.Directors);
        Assert.NotNull(context.Users);
        Assert.NotNull(context.Sexes);
        Assert.NotNull(context.TypeUsers);
        Assert.NotNull(context.Rights);
        Assert.NotNull(context.ActorFilms);
        Assert.NotNull(context.DirectorFilms);
        Assert.NotNull(context.UserRights);
    }

    [Fact]
    public async Task FilmsDbContext_QueryFilms_ReturnsEmpty()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new FilmsDbContext(options);
        var films = await context.Films.ToListAsync();

        // Assert
        Assert.Empty(films);
    }

    [Fact]
    public async Task FilmsDbContext_AddMultipleFilms_SavesSuccessfully()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var films = new[]
        {
            new Film { Title = "Film 1", CreatedBy = "Test", CreatedDate = DateTime.UtcNow, IsActive = true },
            new Film { Title = "Film 2", CreatedBy = "Test", CreatedDate = DateTime.UtcNow, IsActive = true }
        };

        // Act
        using (var context = new FilmsDbContext(options))
        {
            context.Films.AddRange(films);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new FilmsDbContext(options))
        {
            var savedFilms = await context.Films.ToListAsync();
            Assert.Equal(2, savedFilms.Count);
        }
    }
}
