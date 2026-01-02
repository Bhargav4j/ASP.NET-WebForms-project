using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using Films.Application.DTOs;
using Films.Application.Interfaces;
using Films.Web.Pages.FilmPages;

namespace Films.Tests.Web.Pages.FilmPages;

public class DetailsModelTests
{
    private readonly Mock<IFilmService> _filmServiceMock;
    private readonly Mock<ILogger<DetailsModel>> _loggerMock;
    private readonly DetailsModel _model;

    public DetailsModelTests()
    {
        _filmServiceMock = new Mock<IFilmService>();
        _loggerMock = new Mock<ILogger<DetailsModel>>();
        _model = new DetailsModel(_filmServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void DetailsModel_Constructor_ShouldCreateInstance()
    {
        // Assert
        Assert.NotNull(_model);
    }

    [Fact]
    public async Task DetailsModel_OnGetAsync_WithValidId_ShouldReturnPage()
    {
        // Arrange
        var filmDto = new FilmDto
        {
            Id = 1,
            Name = "Test Film",
            Description = "Test Description",
            IsActive = true
        };
        _filmServiceMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(filmDto);

        // Act
        var result = await _model.OnGetAsync(1, CancellationToken.None);

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.NotNull(_model.Film);
        Assert.Equal("Test Film", _model.Film.Name);
    }

    [Fact]
    public async Task DetailsModel_OnGetAsync_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        _filmServiceMock
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FilmDto?)null);

        // Act
        var result = await _model.OnGetAsync(999, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DetailsModel_OnGetAsync_WithException_ShouldReturnNotFound()
    {
        // Arrange
        _filmServiceMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _model.OnGetAsync(1, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
