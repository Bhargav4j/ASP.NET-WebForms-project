using Xunit;
using Moq;
using Films.Application.Services;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Films.Application.Tests;

public class ActorServiceTests
{
    private readonly Mock<IActorRepository> _mockRepository;
    private readonly Mock<ILogger<ActorService>> _mockLogger;
    private readonly ActorService _service;

    public ActorServiceTests()
    {
        _mockRepository = new Mock<IActorRepository>();
        _mockLogger = new Mock<ILogger<ActorService>>();
        _service = new ActorService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithNullRepository_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ActorService(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ActorService(_mockRepository.Object, null!));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllActors()
    {
        // Arrange
        var actors = new List<Actor>
        {
            new Actor { Id = 1, Name = "Actor 1" },
            new Actor { Id = 2, Name = "Actor 2" }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(actors);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsActor()
    {
        // Arrange
        var actor = new Actor { Id = 1, Name = "Test Actor" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(actor);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Actor", result.Name);
        _mockRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Actor?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidActor_CreatesActor()
    {
        // Arrange
        var actor = new Actor { Name = "New Actor" };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Actor>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Actor a, CancellationToken ct) => { a.Id = 1; return a; });

        // Act
        var result = await _service.CreateAsync(actor);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.True(result.IsActive);
        Assert.NotEqual(default(DateTime), result.CreatedDate);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Actor>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithNullActor_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _service.CreateAsync(null!));
    }

    [Fact]
    public async Task UpdateAsync_WithValidActor_UpdatesActor()
    {
        // Arrange
        var actor = new Actor { Id = 1, Name = "Updated Actor" };
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Actor>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(actor);

        // Assert
        Assert.NotNull(actor.ModifiedDate);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Actor>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithNullActor_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _service.UpdateAsync(null!));
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_DeletesActor()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_WithValidSearchTerm_ReturnsMatchingActors()
    {
        // Arrange
        var actors = new List<Actor>
        {
            new Actor { Id = 1, Name = "John Doe" },
            new Actor { Id = 2, Name = "John Smith" }
        };
        _mockRepository.Setup(r => r.SearchAsync("John", It.IsAny<CancellationToken>()))
            .ReturnsAsync(actors);

        // Act
        var result = await _service.SearchAsync("John");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.SearchAsync("John", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_WithEmptySearchTerm_ReturnsEmptyList()
    {
        // Arrange
        _mockRepository.Setup(r => r.SearchAsync("", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Actor>());

        // Act
        var result = await _service.SearchAsync("");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _mockRepository.Verify(r => r.SearchAsync("", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryThrowsException_RethrowsException()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.GetAllAsync());
    }

    [Fact]
    public async Task CreateAsync_SetsCreatedDateAndIsActive()
    {
        // Arrange
        var actor = new Actor { Name = "Test Actor" };
        var beforeCreate = DateTime.UtcNow;

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Actor>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Actor a, CancellationToken ct) => a);

        // Act
        var result = await _service.CreateAsync(actor);

        // Assert
        Assert.True(result.IsActive);
        Assert.True(result.CreatedDate >= beforeCreate);
    }
}
