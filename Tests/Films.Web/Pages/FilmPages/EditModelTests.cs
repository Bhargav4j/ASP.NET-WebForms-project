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

public class EditModelTests
{
    private readonly Mock<IFilmService> _filmServiceMock;
    private readonly Mock<ILogger<EditModel>> _loggerMock;
    private readonly EditModel _model;

    public EditModelTests()
    {
        _filmServiceMock = new Mock<IFilmService>();
        _loggerMock = new Mock<ILogger<EditModel>>();
        _model = new EditModel(_filmServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void EditModel_Constructor_ShouldCreateInstance()
    {
        // Assert
        Assert.NotNull(_model);
    }

    [Fact]
    public async Task EditModel_OnGetAsync_WithValidId_ShouldReturnPage()
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
        Assert.Equal("Test Film", _model.Film.Name);
    }

    [Fact]
    public async Task EditModel_OnGetAsync_WithInvalidId_ShouldReturnNotFound()
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
    public async Task EditModel_OnPostAsync_WithValidModel_ShouldRedirectToIndex()
    {
        // Arrange
        _model.Film = new EditModel.FilmEditViewModel
        {
            Id = 1,
            Name = "Updated Film",
            Description = "Updated Description",
            IsActive = true
        };
        _filmServiceMock
            .Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<FilmUpdateDto>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _model.OnPostAsync(CancellationToken.None);

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("./Index", redirectResult.PageName);
        _filmServiceMock.Verify(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<FilmUpdateDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task EditModel_OnPostAsync_WithException_ShouldReturnPageWithError()
    {
        // Arrange
        _model.Film = new EditModel.FilmEditViewModel
        {
            Id = 1,
            Name = "Test Film",
            Description = "Test Description",
            IsActive = true
        };
        _filmServiceMock
            .Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<FilmUpdateDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _model.OnPostAsync(CancellationToken.None);

        // Assert
        Assert.IsType<PageResult>(result);
    }

    [Fact]
    public void FilmEditViewModel_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var viewModel = new EditModel.FilmEditViewModel();

        // Act
        viewModel.Id = 1;
        viewModel.Name = "Test";
        viewModel.Description = "Test Description";
        viewModel.IsActive = true;

        // Assert
        Assert.Equal(1, viewModel.Id);
        Assert.Equal("Test", viewModel.Name);
        Assert.Equal("Test Description", viewModel.Description);
        Assert.True(viewModel.IsActive);
    }
}
