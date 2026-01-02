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

public class DeleteModelTests
{
    private readonly Mock<IFilmService> _filmServiceMock;
    private readonly Mock<ILogger<DeleteModel>> _loggerMock;
    private readonly DeleteModel _model;

    public DeleteModelTests()
    {
        _filmServiceMock = new Mock<IFilmService>();
        _loggerMock = new Mock<ILogger<DeleteModel>>();
        _model = new DeleteModel(_filmServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void DeleteModel_Constructor_ShouldCreateInstance()
    {
        // Assert
        Assert.NotNull(_model);
    }

    [Fact]
    public async Task DeleteModel_OnGetAsync_WithValidId_ShouldReturnPage()
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
    public async Task DeleteModel_OnGetAsync_WithInvalidId_ShouldReturnNotFound()
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
    public async Task DeleteModel_OnPostAsync_WithValidId_ShouldRedirectToIndex()
    {
        // Arrange
        _filmServiceMock
            .Setup(x => x.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _model.OnPostAsync(1, CancellationToken.None);

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("./Index", redirectResult.PageName);
        _filmServiceMock.Verify(x => x.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteModel_OnPostAsync_WithException_ShouldRedirectToIndex()
    {
        // Arrange
        _filmServiceMock
            .Setup(x => x.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _model.OnPostAsync(1, CancellationToken.None);

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("./Index", redirectResult.PageName);
    }
}
