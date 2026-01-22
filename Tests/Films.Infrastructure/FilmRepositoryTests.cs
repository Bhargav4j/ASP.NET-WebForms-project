using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Films.Domain.Entities;
using Films.Infrastructure.Repositories;
using Films.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Films.Infrastructure.Tests;

public class FilmRepositoryTests
{
    private readonly Mock<ILogger<FilmRepository>> _loggerMock;
    private readonly DbContextOptions<FilmsDbContext> _options;

    public FilmRepositoryTests()
    {
        _loggerMock = new Mock<ILogger<FilmRepository>>();
        _options = new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void FilmRepository_Constructor_ThrowsArgumentNullException_WhenContextIsNull()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new FilmRepository(null!, _loggerMock.Object));
    }

    [Fact]
    public void FilmRepository_Constructor_ThrowsArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new FilmRepository(context, null!));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsActiveFilms()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _loggerMock.Object);

        var activeFilm1 = new Film { Id = 1, Name = "Film1", IsActive = true, CreatedBy = "test" };
        var activeFilm2 = new Film { Id = 2, Name = "Film2", IsActive = true, CreatedBy = "test" };
        var inactiveFilm = new Film { Id = 3, Name = "Film3", IsActive = false, CreatedBy = "test" };

        context.Films.AddRange(activeFilm1, activeFilm2, inactiveFilm);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, film => Assert.True(film.IsActive));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsFilm_WhenFilmExists()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _loggerMock.Object);

        var film = new Film { Id = 1, Name = "TestFilm", IsActive = true, CreatedBy = "test" };
        context.Films.Add(film);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestFilm", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenFilmDoesNotExist()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _loggerMock.Object);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenFilmIsInactive()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _loggerMock.Object);

        var film = new Film { Id = 1, Name = "InactiveFilm", IsActive = false, CreatedBy = "test" };
        context.Films.Add(film);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsFilm_AndSetsCreatedDate()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _loggerMock.Object);

        var film = new Film { Name = "NewFilm", CreatedBy = "test" };

        // Act
        var result = await repository.AddAsync(film);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.True(result.IsActive);
        Assert.NotEqual(default(DateTime), result.CreatedDate);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesFilm_AndSetsModifiedDate()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _loggerMock.Object);

        var film = new Film { Name = "OriginalName", IsActive = true, CreatedBy = "test", CreatedDate = DateTime.UtcNow };
        context.Films.Add(film);
        await context.SaveChangesAsync();

        context.Entry(film).State = EntityState.Detached;

        var updatedFilm = new Film { Id = film.Id, Name = "UpdatedName", IsActive = true, CreatedBy = "test", CreatedDate = film.CreatedDate };

        // Act
        await repository.UpdateAsync(updatedFilm);

        // Assert
        var retrievedFilm = await context.Films.FindAsync(film.Id);
        Assert.NotNull(retrievedFilm);
        Assert.Equal("UpdatedName", retrievedFilm.Name);
        Assert.NotNull(retrievedFilm.ModifiedDate);
    }

    [Fact]
    public async Task DeleteAsync_SoftDeletesFilm()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _loggerMock.Object);

        var film = new Film { Name = "FilmToDelete", IsActive = true, CreatedBy = "test" };
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
    public async Task DeleteAsync_DoesNotThrow_WhenFilmDoesNotExist()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _loggerMock.Object);

        // Act & Assert
        await repository.DeleteAsync(999);
    }

    [Fact]
    public async Task ExistsAsync_ReturnsTrue_WhenFilmExists()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _loggerMock.Object);

        var film = new Film { Name = "ExistingFilm", IsActive = true, CreatedBy = "test" };
        context.Films.Add(film);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ExistsAsync(film.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_ReturnsFalse_WhenFilmDoesNotExist()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _loggerMock.Object);

        // Act
        var result = await repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_ReturnsFalse_WhenFilmIsInactive()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _loggerMock.Object);

        var film = new Film { Name = "InactiveFilm", IsActive = false, CreatedBy = "test" };
        context.Films.Add(film);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ExistsAsync(film.Id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_ReturnsMatchingFilms_ByName()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _loggerMock.Object);

        var film1 = new Film { Name = "The Matrix", IsActive = true, CreatedBy = "test" };
        var film2 = new Film { Name = "Matrix Reloaded", IsActive = true, CreatedBy = "test" };
        var film3 = new Film { Name = "Inception", IsActive = true, CreatedBy = "test" };

        context.Films.AddRange(film1, film2, film3);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("Matrix");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, film => Assert.Contains("Matrix", film.Name));
    }

    [Fact]
    public async Task SearchAsync_ReturnsMatchingFilms_ByDescription()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _loggerMock.Object);

        var film1 = new Film { Name = "Film1", Description = "Action packed", IsActive = true, CreatedBy = "test" };
        var film2 = new Film { Name = "Film2", Description = "Comedy film", IsActive = true, CreatedBy = "test" };

        context.Films.AddRange(film1, film2);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("Action");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains("Action", result.First().Description);
    }

    [Fact]
    public async Task SearchAsync_ReturnsMatchingFilms_ByGenre()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _loggerMock.Object);

        var film1 = new Film { Name = "Film1", Genre = "Sci-Fi", IsActive = true, CreatedBy = "test" };
        var film2 = new Film { Name = "Film2", Genre = "Drama", IsActive = true, CreatedBy = "test" };

        context.Films.AddRange(film1, film2);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("Sci-Fi");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Sci-Fi", result.First().Genre);
    }

    [Fact]
    public async Task SearchAsync_ReturnsAllActiveFilms_WhenSearchTermIsEmpty()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _loggerMock.Object);

        var film1 = new Film { Name = "Film1", IsActive = true, CreatedBy = "test" };
        var film2 = new Film { Name = "Film2", IsActive = true, CreatedBy = "test" };

        context.Films.AddRange(film1, film2);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task SearchAsync_ReturnsAllActiveFilms_WhenSearchTermIsWhitespace()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new FilmRepository(context, _loggerMock.Object);

        var film1 = new Film { Name = "Film1", IsActive = true, CreatedBy = "test" };
        var film2 = new Film { Name = "Film2", IsActive = true, CreatedBy = "test" };

        context.Films.AddRange(film1, film2);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("   ");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
}
