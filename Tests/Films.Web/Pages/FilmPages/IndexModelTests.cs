using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using Films.Application.DTOs;
using Films.Application.Interfaces;
using Films.Web.Pages.FilmPages;

namespace Films.Tests.Web.Pages.FilmPages;

public class FilmPagesIndexModelTests
{
    private readonly Mock<IFilmService> _filmServiceMock;
    private readonly Mock<ILogger<IndexModel>> _loggerMock;
    private readonly IndexModel _model;

    public FilmPagesIndexModelTests()
    {
        _filmServiceMock = new Mock<IFilmService>();
        _loggerMock = new Mock<ILogger<IndexModel>>();
        _model = new IndexModel(_filmServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void IndexModel_Constructor_ShouldCreateInstance()
    {
        // Assert
        Assert.NotNull(_model);
    }

    [Fact]
    public async Task IndexModel_OnGetAsync_WithoutSearchTerm_ShouldReturnAllFilms()
    {
        // Arrange
        var films = new List<FilmDto>
        {
            new FilmDto { Id = 1, Name = "Film 1" },
            new FilmDto { Id = 2, Name = "Film 2" }
        };
        _filmServiceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(films);

        // Act
        await _model.OnGetAsync(CancellationToken.None);

        // Assert
        Assert.Equal(2, _model.Films.Count());
        _filmServiceMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task IndexModel_OnGetAsync_WithSearchTerm_ShouldReturnFilteredFilms()
    {
        // Arrange
        _model.SearchTerm = "Test";
        var films = new List<FilmDto>
        {
            new FilmDto { Id = 1, Name = "Test Film" }
        };
        _filmServiceMock
            .Setup(x => x.SearchAsync("Test", It.IsAny<CancellationToken>()))
            .ReturnsAsync(films);

        // Act
        await _model.OnGetAsync(CancellationToken.None);

        // Assert
        Assert.Single(_model.Films);
        _filmServiceMock.Verify(x => x.SearchAsync("Test", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task IndexModel_OnGetAsync_WithException_ShouldReturnEmptyList()
    {
        // Arrange
        _filmServiceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        await _model.OnGetAsync(CancellationToken.None);

        // Assert
        Assert.Empty(_model.Films);
    }
}
