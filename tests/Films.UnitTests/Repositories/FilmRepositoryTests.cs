using Films.Domain.Entities;
using Films.Infrastructure.Data;
using Films.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Films.UnitTests.Repositories;

public class FilmRepositoryTests
{
    private readonly Mock<ILogger<FilmRepository>> _mockLogger;
    private readonly DbContextOptions<FilmsDbContext> _options;

    public FilmRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<FilmRepository>>();
        _options = new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void FilmRepository_Constructor_ShouldThrowOnNullContext()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new FilmRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void FilmRepository_Constructor_ShouldThrowOnNullLogger()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new FilmRepository(context, null!));
    }

    [Fact]
    public async Task FilmRepository_GetAllAsync_ShouldReturnActiveFilms()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _mockLogger.Object);

        context.Films.AddRange(
            new Film { Name = "Film1", IsActive = true, CreatedBy = "test", Year = 2020 },
            new Film { Name = "Film2", IsActive = true, CreatedBy = "test", Year = 2021 },
            new Film { Name = "Film3", IsActive = false, CreatedBy = "test", Year = 2022 }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task FilmRepository_GetByIdAsync_ShouldReturnFilm()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _mockLogger.Object);

        var film = new Film { Name = "Test Film", IsActive = true, CreatedBy = "test", Year = 2020 };
        context.Films.Add(film);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(film.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Film", result.Name);
    }

    [Fact]
    public async Task FilmRepository_GetByIdAsync_ShouldReturnNullForInactiveFilm()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _mockLogger.Object);

        var film = new Film { Name = "Inactive Film", IsActive = false, CreatedBy = "test", Year = 2020 };
        context.Films.Add(film);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(film.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FilmRepository_AddAsync_ShouldAddFilm()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _mockLogger.Object);
        var film = new Film { Name = "New Film", CreatedBy = "test", Year = 2023 };

        // Act
        var result = await repository.AddAsync(film);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.True(result.IsActive);
        Assert.NotEqual(default(DateTime), result.CreatedDate);
    }

    [Fact]
    public async Task FilmRepository_UpdateAsync_ShouldUpdateFilm()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _mockLogger.Object);

        var film = new Film { Name = "Original Name", IsActive = true, CreatedBy = "test", Year = 2020 };
        context.Films.Add(film);
        await context.SaveChangesAsync();

        // Act
        film.Name = "Updated Name";
        await repository.UpdateAsync(film);

        // Assert
        var updatedFilm = await context.Films.FindAsync(film.Id);
        Assert.NotNull(updatedFilm);
        Assert.Equal("Updated Name", updatedFilm.Name);
        Assert.NotNull(updatedFilm.ModifiedDate);
    }

    [Fact]
    public async Task FilmRepository_DeleteAsync_ShouldSoftDeleteFilm()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _mockLogger.Object);

        var film = new Film { Name = "To Delete", IsActive = true, CreatedBy = "test", Year = 2020 };
        context.Films.Add(film);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(film.Id);

        // Assert
        var deletedFilm = await context.Films.FindAsync(film.Id);
        Assert.NotNull(deletedFilm);
        Assert.False(deletedFilm.IsActive);
        Assert.NotNull(deletedFilm.ModifiedDate);
    }

    [Fact]
    public async Task FilmRepository_ExistsAsync_ShouldReturnTrueForActiveFilm()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _mockLogger.Object);

        var film = new Film { Name = "Existing Film", IsActive = true, CreatedBy = "test", Year = 2020 };
        context.Films.Add(film);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ExistsAsync(film.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task FilmRepository_ExistsAsync_ShouldReturnFalseForInactiveFilm()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _mockLogger.Object);

        var film = new Film { Name = "Inactive Film", IsActive = false, CreatedBy = "test", Year = 2020 };
        context.Films.Add(film);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ExistsAsync(film.Id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task FilmRepository_SearchAsync_ShouldReturnMatchingFilms()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _mockLogger.Object);

        context.Films.AddRange(
            new Film { Name = "The Matrix", IsActive = true, CreatedBy = "test", Year = 2020 },
            new Film { Name = "The Godfather", IsActive = true, CreatedBy = "test", Year = 2021 },
            new Film { Name = "Inception", IsActive = true, CreatedBy = "test", Year = 2022 }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("Matrix");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, f => f.Name == "The Matrix");
    }

    [Fact]
    public async Task FilmRepository_SearchAsync_ShouldSearchInDescription()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _mockLogger.Object);

        context.Films.AddRange(
            new Film { Name = "Film1", Description = "A sci-fi thriller", IsActive = true, CreatedBy = "test", Year = 2020 },
            new Film { Name = "Film2", Description = "A romantic comedy", IsActive = true, CreatedBy = "test", Year = 2021 }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("sci-fi");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, f => f.Name == "Film1");
    }
}
