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

public class FilmServiceTests
{
    private readonly Mock<IFilmRepository> _repositoryMock;
    private readonly Mock<ILogger<FilmService>> _loggerMock;
    private readonly FilmService _service;

    public FilmServiceTests()
    {
        _repositoryMock = new Mock<IFilmRepository>();
        _loggerMock = new Mock<ILogger<FilmService>>();
        _service = new FilmService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void FilmService_Constructor_ThrowsArgumentNullException_WhenRepositoryIsNull()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new FilmService(null!, _loggerMock.Object));
    }

    [Fact]
    public void FilmService_Constructor_ThrowsArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new FilmService(_repositoryMock.Object, null!));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsFilms_FromRepository()
    {
        // Arrange
        var films = new List<Film>
        {
            new Film { Id = 1, Name = "Film1", IsActive = true, CreatedBy = "test" },
            new Film { Id = 2, Name = "Film2", IsActive = true, CreatedBy = "test" }
        };
        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(films);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _repositoryMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsFilm_WhenFilmExists()
    {
        // Arrange
        var film = new Film { Id = 1, Name = "TestFilm", IsActive = true, CreatedBy = "test" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(film);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestFilm", result.Name);
        _repositoryMock.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenFilmDoesNotExist()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Film?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
        _repositoryMock.Verify(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_CreatesFilm_WhenValidFilm()
    {
        // Arrange
        var film = new Film { Name = "NewFilm", CreatedBy = "test" };
        var createdFilm = new Film { Id = 1, Name = "NewFilm", IsActive = true, CreatedBy = "test" };
        _repositoryMock.Setup(r => r.AddAsync(film, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdFilm);

        // Act
        var result = await _service.CreateAsync(film);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("NewFilm", result.Name);
        _repositoryMock.Verify(r => r.AddAsync(film, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowsArgumentException_WhenFilmNameIsEmpty()
    {
        // Arrange
        var film = new Film { Name = "", CreatedBy = "test" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(film));
    }

    [Fact]
    public async Task CreateAsync_ThrowsArgumentException_WhenFilmNameIsWhitespace()
    {
        // Arrange
        var film = new Film { Name = "   ", CreatedBy = "test" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(film));
    }

    [Fact]
    public async Task CreateAsync_ThrowsArgumentException_WhenFilmNameIsNull()
    {
        // Arrange
        var film = new Film { Name = null!, CreatedBy = "test" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(film));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesFilm_WhenFilmExists()
    {
        // Arrange
        var existingFilm = new Film { Id = 1, Name = "OriginalName", IsActive = true, CreatedBy = "test" };
        var updatedFilm = new Film { Name = "UpdatedName", CreatedBy = "test" };

        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingFilm);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Film>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(1, updatedFilm);

        // Assert
        _repositoryMock.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Film>(f => f.Id == 1 && f.Name == "UpdatedName"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsInvalidOperationException_WhenFilmDoesNotExist()
    {
        // Arrange
        var updatedFilm = new Film { Name = "UpdatedName", CreatedBy = "test" };

        _repositoryMock.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Film?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(999, updatedFilm));
    }

    [Fact]
    public async Task DeleteAsync_DeletesFilm_WhenFilmExists()
    {
        // Arrange
        var existingFilm = new Film { Id = 1, Name = "FilmToDelete", IsActive = true, CreatedBy = "test" };

        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingFilm);
        _repositoryMock.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _repositoryMock.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsInvalidOperationException_WhenFilmDoesNotExist()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Film?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.DeleteAsync(999));
    }

    [Fact]
    public async Task SearchAsync_ReturnsMatchingFilms()
    {
        // Arrange
        var films = new List<Film>
        {
            new Film { Id = 1, Name = "The Matrix", IsActive = true, CreatedBy = "test" },
            new Film { Id = 2, Name = "Matrix Reloaded", IsActive = true, CreatedBy = "test" }
        };

        _repositoryMock.Setup(r => r.SearchAsync("Matrix", It.IsAny<CancellationToken>()))
            .ReturnsAsync(films);

        // Act
        var result = await _service.SearchAsync("Matrix");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _repositoryMock.Verify(r => r.SearchAsync("Matrix", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_ReturnsEmptyList_WhenNoMatches()
    {
        // Arrange
        _repositoryMock.Setup(r => r.SearchAsync("NonExistent", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Film>());

        // Act
        var result = await _service.SearchAsync("NonExistent");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _repositoryMock.Verify(r => r.SearchAsync("NonExistent", It.IsAny<CancellationToken>()), Times.Once);
    }
}
