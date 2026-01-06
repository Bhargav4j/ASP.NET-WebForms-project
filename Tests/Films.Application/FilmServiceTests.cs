using Xunit;
using Moq;
using Films.Application.Services;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Films.Application.Tests;

public class FilmServiceTests
{
    private readonly Mock<IFilmRepository> _mockRepository;
    private readonly Mock<ILogger<FilmService>> _mockLogger;
    private readonly FilmService _service;

    public FilmServiceTests()
    {
        _mockRepository = new Mock<IFilmRepository>();
        _mockLogger = new Mock<ILogger<FilmService>>();
        _service = new FilmService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithNullRepository_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new FilmService(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new FilmService(_mockRepository.Object, null!));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllFilms()
    {
        // Arrange
        var films = new List<Film>
        {
            new Film { Id = 1, Name = "Film 1" },
            new Film { Id = 2, Name = "Film 2" }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(films);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsFilm()
    {
        // Arrange
        var film = new Film { Id = 1, Name = "Test Film" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(film);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Film", result.Name);
        _mockRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Film?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidFilm_CreatesFilm()
    {
        // Arrange
        var film = new Film { Name = "New Film" };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Film>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Film f, CancellationToken ct) => { f.Id = 1; return f; });

        // Act
        var result = await _service.CreateAsync(film);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.True(result.IsActive);
        Assert.NotEqual(default(DateTime), result.CreatedDate);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Film>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithNullFilm_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _service.CreateAsync(null!));
    }

    [Fact]
    public async Task UpdateAsync_WithValidFilm_UpdatesFilm()
    {
        // Arrange
        var film = new Film { Id = 1, Name = "Updated Film" };
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Film>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(film);

        // Assert
        Assert.NotNull(film.ModifiedDate);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Film>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithNullFilm_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _service.UpdateAsync(null!));
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_DeletesFilm()
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
    public async Task SearchAsync_WithValidSearchTerm_ReturnsMatchingFilms()
    {
        // Arrange
        var films = new List<Film>
        {
            new Film { Id = 1, Name = "Action Film" },
            new Film { Id = 2, Name = "Action Movie" }
        };
        _mockRepository.Setup(r => r.SearchAsync("Action", It.IsAny<CancellationToken>()))
            .ReturnsAsync(films);

        // Act
        var result = await _service.SearchAsync("Action");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.SearchAsync("Action", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_WithEmptySearchTerm_ReturnsEmptyList()
    {
        // Arrange
        _mockRepository.Setup(r => r.SearchAsync("", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Film>());

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
        var film = new Film { Name = "Test Film" };
        var beforeCreate = DateTime.UtcNow;

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Film>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Film f, CancellationToken ct) => f);

        // Act
        var result = await _service.CreateAsync(film);

        // Assert
        Assert.True(result.IsActive);
        Assert.True(result.CreatedDate >= beforeCreate);
    }
}
