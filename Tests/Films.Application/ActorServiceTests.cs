using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Films.Application.Tests;

public class ActorServiceTests
{
    private readonly Mock<IActorRepository> _repositoryMock;
    private readonly Mock<ILogger<ActorService>> _loggerMock;
    private readonly ActorService _service;

    public ActorServiceTests()
    {
        _repositoryMock = new Mock<IActorRepository>();
        _loggerMock = new Mock<ILogger<ActorService>>();
        _service = new ActorService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void ActorService_Constructor_ThrowsArgumentNullException_WhenRepositoryIsNull()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ActorService(null!, _loggerMock.Object));
    }

    [Fact]
    public void ActorService_Constructor_ThrowsArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ActorService(_repositoryMock.Object, null!));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsActors_FromRepository()
    {
        // Arrange
        var actors = new List<Actor>
        {
            new Actor { Id = 1, Name = "Actor1", IsActive = true, CreatedBy = "test" },
            new Actor { Id = 2, Name = "Actor2", IsActive = true, CreatedBy = "test" }
        };
        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(actors);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _repositoryMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsActor_WhenActorExists()
    {
        // Arrange
        var actor = new Actor { Id = 1, Name = "TestActor", IsActive = true, CreatedBy = "test" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(actor);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestActor", result.Name);
        _repositoryMock.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenActorDoesNotExist()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Actor?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
        _repositoryMock.Verify(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_CreatesActor_WhenValidActor()
    {
        // Arrange
        var actor = new Actor { Name = "NewActor", CreatedBy = "test" };
        var createdActor = new Actor { Id = 1, Name = "NewActor", IsActive = true, CreatedBy = "test" };
        _repositoryMock.Setup(r => r.AddAsync(actor, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdActor);

        // Act
        var result = await _service.CreateAsync(actor);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("NewActor", result.Name);
        _repositoryMock.Verify(r => r.AddAsync(actor, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowsArgumentException_WhenActorNameIsEmpty()
    {
        // Arrange
        var actor = new Actor { Name = "", CreatedBy = "test" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(actor));
    }

    [Fact]
    public async Task CreateAsync_ThrowsArgumentException_WhenActorNameIsWhitespace()
    {
        // Arrange
        var actor = new Actor { Name = "   ", CreatedBy = "test" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(actor));
    }

    [Fact]
    public async Task CreateAsync_ThrowsArgumentException_WhenActorNameIsNull()
    {
        // Arrange
        var actor = new Actor { Name = null!, CreatedBy = "test" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(actor));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesActor_WhenActorExists()
    {
        // Arrange
        var existingActor = new Actor { Id = 1, Name = "OriginalName", IsActive = true, CreatedBy = "test" };
        var updatedActor = new Actor { Name = "UpdatedName", CreatedBy = "test" };

        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingActor);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Actor>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(1, updatedActor);

        // Assert
        _repositoryMock.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Actor>(a => a.Id == 1 && a.Name == "UpdatedName"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsInvalidOperationException_WhenActorDoesNotExist()
    {
        // Arrange
        var updatedActor = new Actor { Name = "UpdatedName", CreatedBy = "test" };

        _repositoryMock.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Actor?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(999, updatedActor));
    }

    [Fact]
    public async Task DeleteAsync_DeletesActor_WhenActorExists()
    {
        // Arrange
        var existingActor = new Actor { Id = 1, Name = "ActorToDelete", IsActive = true, CreatedBy = "test" };

        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingActor);
        _repositoryMock.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _repositoryMock.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsInvalidOperationException_WhenActorDoesNotExist()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Actor?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.DeleteAsync(999));
    }

    [Fact]
    public async Task SearchAsync_ReturnsMatchingActors()
    {
        // Arrange
        var actors = new List<Actor>
        {
            new Actor { Id = 1, Name = "Tom Hanks", IsActive = true, CreatedBy = "test" },
            new Actor { Id = 2, Name = "Tom Cruise", IsActive = true, CreatedBy = "test" }
        };

        _repositoryMock.Setup(r => r.SearchAsync("Tom", It.IsAny<CancellationToken>()))
            .ReturnsAsync(actors);

        // Act
        var result = await _service.SearchAsync("Tom");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _repositoryMock.Verify(r => r.SearchAsync("Tom", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_ReturnsEmptyList_WhenNoMatches()
    {
        // Arrange
        _repositoryMock.Setup(r => r.SearchAsync("NonExistent", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Actor>());

        // Act
        var result = await _service.SearchAsync("NonExistent");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _repositoryMock.Verify(r => r.SearchAsync("NonExistent", It.IsAny<CancellationToken>()), Times.Once);
    }
}
