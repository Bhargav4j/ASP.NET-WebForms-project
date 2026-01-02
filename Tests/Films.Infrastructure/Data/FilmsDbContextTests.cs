using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Films.Infrastructure.Data;
using Films.Domain.Entities;

namespace Films.Tests.Infrastructure.Data;

public class FilmsDbContextTests
{
    private DbContextOptions<FilmsDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void FilmsDbContext_Constructor_ShouldCreateInstance()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context);
    }

    [Fact]
    public void FilmsDbContext_HasFilmsDbSet()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context.Films);
    }

    [Fact]
    public void FilmsDbContext_HasActorsDbSet()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context.Actors);
    }

    [Fact]
    public void FilmsDbContext_HasDirectorsDbSet()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context.Directors);
    }

    [Fact]
    public void FilmsDbContext_HasUsersDbSet()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new FilmsDbContext(options);

        // Assert
        Assert.NotNull(context.Users);
    }

    [Fact]
    public void FilmsDbContext_CanAddFilm()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new FilmsDbContext(options);
        var film = new Film
        {
            Name = "Test Film",
            Description = "Test Description",
            CreatedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "Test"
        };

        // Act
        context.Films.Add(film);
        context.SaveChanges();

        // Assert
        Assert.Equal(1, context.Films.Count());
    }

    [Fact]
    public void FilmsDbContext_CanAddActor()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new FilmsDbContext(options);
        var actor = new Actor
        {
            FirstName = "John",
            LastName = "Doe",
            CreatedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "Test"
        };

        // Act
        context.Actors.Add(actor);
        context.SaveChanges();

        // Assert
        Assert.Equal(1, context.Actors.Count());
    }

    [Fact]
    public void FilmsDbContext_CanAddDirector()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new FilmsDbContext(options);
        var director = new Director
        {
            FirstName = "Jane",
            LastName = "Smith",
            CreatedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "Test"
        };

        // Act
        context.Directors.Add(director);
        context.SaveChanges();

        // Assert
        Assert.Equal(1, context.Directors.Count());
    }
}
