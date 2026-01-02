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

public class CreateModelTests
{
    private readonly Mock<IFilmService> _filmServiceMock;
    private readonly Mock<ILogger<CreateModel>> _loggerMock;
    private readonly CreateModel _model;

    public CreateModelTests()
    {
        _filmServiceMock = new Mock<IFilmService>();
        _loggerMock = new Mock<ILogger<CreateModel>>();
        _model = new CreateModel(_filmServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void CreateModel_Constructor_ShouldCreateInstance()
    {
        // Assert
        Assert.NotNull(_model);
    }

    [Fact]
    public void CreateModel_OnGet_ShouldReturnPage()
    {
        // Act
        var result = _model.OnGet();

        // Assert
        Assert.IsType<PageResult>(result);
    }

    [Fact]
    public async Task CreateModel_OnPostAsync_WithValidModel_ShouldRedirectToIndex()
    {
        // Arrange
        _model.Film = new CreateModel.FilmCreateViewModel
        {
            Name = "Test Film",
            Description = "Test Description"
        };
        _filmServiceMock
            .Setup(x => x.CreateAsync(It.IsAny<FilmCreateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FilmDto { Id = 1, Name = "Test Film" });

        // Act
        var result = await _model.OnPostAsync(CancellationToken.None);

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("./Index", redirectResult.PageName);
        _filmServiceMock.Verify(x => x.CreateAsync(It.IsAny<FilmCreateDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateModel_OnPostAsync_WithException_ShouldReturnPageWithError()
    {
        // Arrange
        _model.Film = new CreateModel.FilmCreateViewModel
        {
            Name = "Test Film",
            Description = "Test Description"
        };
        _filmServiceMock
            .Setup(x => x.CreateAsync(It.IsAny<FilmCreateDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _model.OnPostAsync(CancellationToken.None);

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.False(_model.ModelState.IsValid);
    }

    [Fact]
    public void FilmCreateViewModel_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var viewModel = new CreateModel.FilmCreateViewModel();

        // Act
        viewModel.Name = "Test";
        viewModel.Description = "Test Description";

        // Assert
        Assert.Equal("Test", viewModel.Name);
        Assert.Equal("Test Description", viewModel.Description);
    }
}
