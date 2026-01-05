using AutoMapper;
using Films.Application.DTOs;
using Films.Application.Services;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Films.UnitTests.Services;

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
    public async Task GetAllAsync_Should_Return_All_Films()
    {
        // Arrange
        var films = new List<Film>
        {
            new Film { Id = 1, Name = "Film 1", Year = 2020, IsActive = true },
            new Film { Id = 2, Name = "Film 2", Year = 2021, IsActive = true }
        };
        var filmDtos = new List<FilmDto>
        {
            new FilmDto { Id = 1, Name = "Film 1", Year = 2020 },
            new FilmDto { Id = 2, Name = "Film 2", Year = 2021 }
        };

        _mockRepository.Setup(r => r.GetAllAsync(default)).ReturnsAsync(films);
        _mockMapper.Setup(m => m.Map<IEnumerable<FilmDto>>(films)).Returns(filmDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(filmDtos);
    }
}
