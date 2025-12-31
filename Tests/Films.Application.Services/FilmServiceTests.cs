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

public class FilmServiceTests
{
    private readonly Mock<IFilmRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<FilmService>> _mockLogger;
    private readonly FilmService _service;

    public FilmServiceTests()
    {
        _mockRepository = new Mock<IFilmRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<FilmService>>();
        _service = new FilmService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public void FilmService_Constructor_ThrowsWhenRepositoryIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new FilmService(null!, _mockMapper.Object, _mockLogger.Object));
    }

    [Fact]
    public void FilmService_Constructor_ThrowsWhenMapperIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new FilmService(_mockRepository.Object, null!, _mockLogger.Object));
    }

    [Fact]
    public void FilmService_Constructor_ThrowsWhenLoggerIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new FilmService(_mockRepository.Object, _mockMapper.Object, null!));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsFilmDtos()
    {
        // Arrange
        var films = new List<Film> { new Film { Id = 1, Title = "Test Film" } };
        var filmDtos = new List<FilmDto> { new FilmDto { Id = 1, Title = "Test Film" } };

        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(films);
        _mockMapper.Setup(m => m.Map<IEnumerable<FilmDto>>(films))
            .Returns(filmDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsFilmDto_WhenFilmExists()
    {
        // Arrange
        var film = new Film { Id = 1, Title = "Test Film" };
        var filmDto = new FilmDto { Id = 1, Title = "Test Film" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(film);
        _mockMapper.Setup(m => m.Map<FilmDto>(film))
            .Returns(filmDto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenFilmDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Film?)null);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedFilmDto()
    {
        // Arrange
        var createDto = new FilmCreateDto { Title = "New Film" };
        var film = new Film { Title = "New Film" };
        var createdFilm = new Film { Id = 1, Title = "New Film" };
        var filmDto = new FilmDto { Id = 1, Title = "New Film" };

        _mockMapper.Setup(m => m.Map<Film>(createDto)).Returns(film);
        _mockRepository.Setup(r => r.AddAsync(film, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdFilm);
        _mockMapper.Setup(m => m.Map<FilmDto>(createdFilm)).Returns(filmDto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("New Film", result.Title);
    }

    [Fact]
    public async Task CreateAsync_ThrowsArgumentException_WhenTitleIsEmpty()
    {
        // Arrange
        var createDto = new FilmCreateDto { Title = "" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesFilm_WhenFilmExists()
    {
        // Arrange
        var updateDto = new FilmUpdateDto { Title = "Updated Film" };
        var existingFilm = new Film { Id = 1, Title = "Old Film" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingFilm);
        _mockMapper.Setup(m => m.Map(updateDto, existingFilm));

        // Act
        await _service.UpdateAsync(1, updateDto);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(existingFilm, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenFilmDoesNotExist()
    {
        // Arrange
        var updateDto = new FilmUpdateDto { Title = "Updated Film" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Film?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(1, updateDto));
    }

    [Fact]
    public async Task DeleteAsync_DeletesFilm_WhenFilmExists()
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
    public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenFilmDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(1));
    }

    [Fact]
    public async Task SearchAsync_ReturnsMatchingFilms()
    {
        // Arrange
        var films = new List<Film> { new Film { Id = 1, Title = "Test Film" } };
        var filmDtos = new List<FilmDto> { new FilmDto { Id = 1, Title = "Test Film" } };

        _mockRepository.Setup(r => r.SearchAsync("Test", It.IsAny<CancellationToken>()))
            .ReturnsAsync(films);
        _mockMapper.Setup(m => m.Map<IEnumerable<FilmDto>>(films))
            .Returns(filmDtos);

        // Act
        var result = await _service.SearchAsync("Test");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }
}
