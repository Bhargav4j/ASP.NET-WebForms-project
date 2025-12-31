using Xunit;
using Films.Application.Services;
using Films.Domain.Entities;
using Films.Domain.DTOs;
using Films.Domain.Interfaces.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Films.Application.Services.Tests;

public class ActorServiceTests
{
    private readonly Mock<IActorRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<ActorService>> _mockLogger;
    private readonly ActorService _service;

    public ActorServiceTests()
    {
        _mockRepository = new Mock<IActorRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<ActorService>>();
        _service = new ActorService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public void ActorService_Constructor_ThrowsWhenRepositoryIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ActorService(null!, _mockMapper.Object, _mockLogger.Object));
    }

    [Fact]
    public void ActorService_Constructor_ThrowsWhenMapperIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ActorService(_mockRepository.Object, null!, _mockLogger.Object));
    }

    [Fact]
    public void ActorService_Constructor_ThrowsWhenLoggerIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ActorService(_mockRepository.Object, _mockMapper.Object, null!));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsActorDtos()
    {
        // Arrange
        var actors = new List<Actor> { new Actor { Id = 1, Name = "Test Actor" } };
        var actorDtos = new List<ActorDto> { new ActorDto { Id = 1, Name = "Test Actor" } };

        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(actors);
        _mockMapper.Setup(m => m.Map<IEnumerable<ActorDto>>(actors))
            .Returns(actorDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsActorDto_WhenActorExists()
    {
        // Arrange
        var actor = new Actor { Id = 1, Name = "Test Actor" };
        var actorDto = new ActorDto { Id = 1, Name = "Test Actor" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(actor);
        _mockMapper.Setup(m => m.Map<ActorDto>(actor))
            .Returns(actorDto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenActorDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Actor?)null);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedActorDto()
    {
        // Arrange
        var createDto = new ActorCreateDto { Name = "New Actor" };
        var actor = new Actor { Name = "New Actor" };
        var createdActor = new Actor { Id = 1, Name = "New Actor" };
        var actorDto = new ActorDto { Id = 1, Name = "New Actor" };

        _mockMapper.Setup(m => m.Map<Actor>(createDto)).Returns(actor);
        _mockRepository.Setup(r => r.AddAsync(actor, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdActor);
        _mockMapper.Setup(m => m.Map<ActorDto>(createdActor)).Returns(actorDto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("New Actor", result.Name);
    }

    [Fact]
    public async Task CreateAsync_ThrowsArgumentException_WhenNameIsEmpty()
    {
        // Arrange
        var createDto = new ActorCreateDto { Name = "" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesActor_WhenActorExists()
    {
        // Arrange
        var updateDto = new ActorUpdateDto { Name = "Updated Actor" };
        var existingActor = new Actor { Id = 1, Name = "Old Actor" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingActor);
        _mockMapper.Setup(m => m.Map(updateDto, existingActor));

        // Act
        await _service.UpdateAsync(1, updateDto);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(existingActor, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenActorDoesNotExist()
    {
        // Arrange
        var updateDto = new ActorUpdateDto { Name = "Updated Actor" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Actor?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(1, updateDto));
    }

    [Fact]
    public async Task DeleteAsync_DeletesActor_WhenActorExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenActorDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(1));
    }

    [Fact]
    public async Task SearchAsync_ReturnsMatchingActors()
    {
        // Arrange
        var actors = new List<Actor> { new Actor { Id = 1, Name = "Test Actor" } };
        var actorDtos = new List<ActorDto> { new ActorDto { Id = 1, Name = "Test Actor" } };

        _mockRepository.Setup(r => r.SearchAsync("Test", It.IsAny<CancellationToken>()))
            .ReturnsAsync(actors);
        _mockMapper.Setup(m => m.Map<IEnumerable<ActorDto>>(actors))
            .Returns(actorDtos);

        // Act
        var result = await _service.SearchAsync("Test");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }
}
