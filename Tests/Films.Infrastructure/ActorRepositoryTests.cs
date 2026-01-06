using Xunit;
using Films.Infrastructure.Repositories;
using Films.Infrastructure.Data;
using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Films.Infrastructure.Tests;

public class ActorRepositoryTests
{
    private readonly FilmsDbContext _context;
    private readonly Mock<ILogger<ActorRepository>> _mockLogger;
    private readonly ActorRepository _repository;

    public ActorRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new FilmsDbContext(options);
        _mockLogger = new Mock<ILogger<ActorRepository>>();
        _repository = new ActorRepository(_context, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithNullContext_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ActorRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ActorRepository(_context, null!));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllActiveActors()
    {
        // Arrange
        _context.Actors.AddRange(
            new Actor { Id = 1, Name = "Actor 1", IsActive = true },
            new Actor { Id = 2, Name = "Actor 2", IsActive = true },
            new Actor { Id = 3, Name = "Actor 3", IsActive = false }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsActor()
    {
        // Arrange
        var actor = new Actor { Id = 1, Name = "Test Actor", IsActive = true };
        _context.Actors.Add(actor);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Actor", result.Name);
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
    public async Task AddAsync_AddsActorToDatabase()
    {
        // Arrange
        var actor = new Actor { Name = "New Actor", IsActive = true };

        // Act
        var result = await _repository.AddAsync(actor);

        // Assert
        Assert.NotEqual(0, result.Id);
        Assert.Equal(1, _context.Actors.Count());
    }

    [Fact]
    public async Task UpdateAsync_UpdatesExistingActor()
    {
        // Arrange
        var actor = new Actor { Name = "Original Name", IsActive = true };
        _context.Actors.Add(actor);
        await _context.SaveChangesAsync();
        _context.Entry(actor).State = EntityState.Detached;

        actor.Name = "Updated Name";

        // Act
        await _repository.UpdateAsync(actor);

        // Assert
        var updated = await _context.Actors.FindAsync(actor.Id);
        Assert.Equal("Updated Name", updated?.Name);
    }

    [Fact]
    public async Task DeleteAsync_SetsIsActiveToFalse()
    {
        // Arrange
        var actor = new Actor { Name = "To Delete", IsActive = true };
        _context.Actors.Add(actor);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(actor.Id);

        // Assert
        var deleted = await _context.Actors.FindAsync(actor.Id);
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
        var actor = new Actor { Name = "Test Actor", IsActive = true };
        _context.Actors.Add(actor);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync(actor.Id);

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
    public async Task ExistsAsync_WithInactiveActor_ReturnsFalse()
    {
        // Arrange
        var actor = new Actor { Name = "Inactive Actor", IsActive = false };
        _context.Actors.Add(actor);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync(actor.Id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_WithMatchingName_ReturnsActors()
    {
        // Arrange
        _context.Actors.AddRange(
            new Actor { Name = "John Doe", IsActive = true },
            new Actor { Name = "John Smith", IsActive = true },
            new Actor { Name = "Jane Doe", IsActive = true }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("John");

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task SearchAsync_WithMatchingSurname_ReturnsActors()
    {
        // Arrange
        _context.Actors.AddRange(
            new Actor { Name = "John", Surname = "Smith", IsActive = true },
            new Actor { Name = "Jane", Surname = "Smith", IsActive = true },
            new Actor { Name = "Bob", Surname = "Jones", IsActive = true }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("Smith");

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task SearchAsync_WithEmptySearchTerm_ReturnsAllActors()
    {
        // Arrange
        _context.Actors.AddRange(
            new Actor { Name = "Actor 1", IsActive = true },
            new Actor { Name = "Actor 2", IsActive = true }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("");

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_ReturnsActorsOrderedByName()
    {
        // Arrange
        _context.Actors.AddRange(
            new Actor { Name = "Zebra", IsActive = true },
            new Actor { Name = "Alpha", IsActive = true },
            new Actor { Name = "Beta", IsActive = true }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetAllAsync()).ToList();

        // Assert
        Assert.Equal("Alpha", result[0].Name);
        Assert.Equal("Beta", result[1].Name);
        Assert.Equal("Zebra", result[2].Name);
    }

    [Fact]
    public async Task GetAllAsync_IncludesSexNavigationProperty()
    {
        // Arrange
        var sex = new Sex { Id = 1, Name = "Male", IsActive = true };
        _context.Sexes.Add(sex);
        await _context.SaveChangesAsync();

        var actor = new Actor { Name = "Test Actor", SexId = 1, IsActive = true };
        _context.Actors.Add(actor);
        await _context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetAllAsync()).First();

        // Assert
        Assert.NotNull(result.Sex);
        Assert.Equal("Male", result.Sex.Name);
    }
}
