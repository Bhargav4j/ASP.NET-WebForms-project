using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Application.Services;

namespace Films.UnitTests.Services;

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
    public async Task GetAllAsync_ShouldReturnAllFilms()
    {
        var films = new List<Film>
        {
            new Film { Id = 1, Name = "Film 1", CreatedBy = "test", IsActive = true },
            new Film { Id = 2, Name = "Film 2", CreatedBy = "test", IsActive = true }
        };
        _repositoryMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(films);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(films);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFilm_WhenFilmExists()
    {
        var film = new Film { Id = 1, Name = "Test Film", CreatedBy = "test", IsActive = true };
        _repositoryMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(film);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result?.Name.Should().Be("Test Film");
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenNameIsEmpty()
    {
        var film = new Film { Name = "", CreatedBy = "test" };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(film));
    }
}
