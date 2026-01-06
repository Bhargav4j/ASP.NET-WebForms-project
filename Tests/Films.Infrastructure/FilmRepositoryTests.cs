using Xunit;
using Films.Infrastructure.Repositories;
using Films.Infrastructure.Data;
using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Films.Infrastructure.Tests;

public class FilmRepositoryTests
{
    private readonly FilmsDbContext _context;
    private readonly Mock<ILogger<FilmRepository>> _mockLogger;
    private readonly FilmRepository _repository;

    public FilmRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new FilmsDbContext(options);
        _mockLogger = new Mock<ILogger<FilmRepository>>();
        _repository = new FilmRepository(_context, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithNullContext_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new FilmRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new FilmRepository(_context, null!));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllActiveFilms()
    {
        // Arrange
        _context.Films.AddRange(
            new Film { Id = 1, Name = "Film 1", IsActive = true },
            new Film { Id = 2, Name = "Film 2", IsActive = true },
            new Film { Id = 3, Name = "Film 3", IsActive = false }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsFilm()
    {
        // Arrange
        var film = new Film { Id = 1, Name = "Test Film", IsActive = true };
        _context.Films.Add(film);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Film", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange & Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsFilmToDatabase()
    {
        // Arrange
        var film = new Film { Name = "New Film", IsActive = true };

        // Act
        var result = await _repository.AddAsync(film);

        // Assert
        Assert.NotEqual(0, result.Id);
        Assert.Equal(1, _context.Films.Count());
    }

    [Fact]
    public async Task UpdateAsync_UpdatesExistingFilm()
    {
        // Arrange
        var film = new Film { Name = "Original Name", IsActive = true };
        _context.Films.Add(film);
        await _context.SaveChangesAsync();
        _context.Entry(film).State = EntityState.Detached;

        film.Name = "Updated Name";

        // Act
        await _repository.UpdateAsync(film);

        // Assert
        var updated = await _context.Films.FindAsync(film.Id);
        Assert.Equal("Updated Name", updated?.Name);
    }

    [Fact]
    public async Task DeleteAsync_SetsIsActiveToFalse()
    {
        // Arrange
        var film = new Film { Name = "To Delete", IsActive = true };
        _context.Films.Add(film);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(film.Id);

        // Assert
        var deleted = await _context.Films.FindAsync(film.Id);
        Assert.False(deleted?.IsActive);
        Assert.NotNull(deleted?.ModifiedDate);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_DoesNotThrow()
    {
        // Arrange & Act
        await _repository.DeleteAsync(999);

        // Assert - no exception thrown
        Assert.True(true);
    }

    [Fact]
    public async Task ExistsAsync_WithExistingId_ReturnsTrue()
    {
        // Arrange
        var film = new Film { Name = "Test Film", IsActive = true };
        _context.Films.Add(film);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync(film.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_WithNonExistingId_ReturnsFalse()
    {
        // Arrange & Act
        var result = await _repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_WithInactiveFilm_ReturnsFalse()
    {
        // Arrange
        var film = new Film { Name = "Inactive Film", IsActive = false };
        _context.Films.Add(film);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync(film.Id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_WithMatchingName_ReturnsFilms()
    {
        // Arrange
        _context.Films.AddRange(
            new Film { Name = "Action Film", IsActive = true },
            new Film { Name = "Action Movie", IsActive = true },
            new Film { Name = "Drama Film", IsActive = true }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("Action");

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task SearchAsync_WithMatchingDescription_ReturnsFilms()
    {
        // Arrange
        _context.Films.AddRange(
            new Film { Name = "Film 1", Description = "Exciting action", IsActive = true },
            new Film { Name = "Film 2", Description = "Boring drama", IsActive = true }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("action");

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task SearchAsync_WithEmptySearchTerm_ReturnsAllFilms()
    {
        // Arrange
        _context.Films.AddRange(
            new Film { Name = "Film 1", IsActive = true },
            new Film { Name = "Film 2", IsActive = true }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("");

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_ReturnsFilmsOrderedByName()
    {
        // Arrange
        _context.Films.AddRange(
            new Film { Name = "Zebra Film", IsActive = true },
            new Film { Name = "Alpha Film", IsActive = true },
            new Film { Name = "Beta Film", IsActive = true }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetAllAsync()).ToList();

        // Assert
        Assert.Equal("Alpha Film", result[0].Name);
        Assert.Equal("Beta Film", result[1].Name);
        Assert.Equal("Zebra Film", result[2].Name);
    }
}
