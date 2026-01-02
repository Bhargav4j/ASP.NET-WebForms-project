using System;
using Xunit;
using Films.Application.DTOs;

namespace Films.Tests.Application.DTOs;

public class FilmDtoTests
{
    [Fact]
    public void FilmDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var filmDto = new FilmDto();

        // Assert
        Assert.Equal(0, filmDto.Id);
        Assert.Equal(string.Empty, filmDto.Name);
        Assert.Null(filmDto.Description);
        Assert.False(filmDto.IsActive);
    }

    [Fact]
    public void FilmDto_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var filmDto = new FilmDto();
        var testDate = DateTime.UtcNow;

        // Act
        filmDto.Id = 1;
        filmDto.Name = "Test Film";
        filmDto.Description = "Test Description";
        filmDto.CreatedDate = testDate;
        filmDto.ModifiedDate = testDate;
        filmDto.IsActive = true;

        // Assert
        Assert.Equal(1, filmDto.Id);
        Assert.Equal("Test Film", filmDto.Name);
        Assert.Equal("Test Description", filmDto.Description);
        Assert.Equal(testDate, filmDto.CreatedDate);
        Assert.Equal(testDate, filmDto.ModifiedDate);
        Assert.True(filmDto.IsActive);
    }

    [Fact]
    public void FilmCreateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var filmCreateDto = new FilmCreateDto();

        // Assert
        Assert.Equal(string.Empty, filmCreateDto.Name);
        Assert.Null(filmCreateDto.Description);
    }

    [Fact]
    public void FilmCreateDto_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var filmCreateDto = new FilmCreateDto();

        // Act
        filmCreateDto.Name = "New Film";
        filmCreateDto.Description = "New Description";

        // Assert
        Assert.Equal("New Film", filmCreateDto.Name);
        Assert.Equal("New Description", filmCreateDto.Description);
    }

    [Fact]
    public void FilmUpdateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var filmUpdateDto = new FilmUpdateDto();

        // Assert
        Assert.Equal(string.Empty, filmUpdateDto.Name);
        Assert.Null(filmUpdateDto.Description);
        Assert.False(filmUpdateDto.IsActive);
    }

    [Fact]
    public void FilmUpdateDto_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var filmUpdateDto = new FilmUpdateDto();

        // Act
        filmUpdateDto.Name = "Updated Film";
        filmUpdateDto.Description = "Updated Description";
        filmUpdateDto.IsActive = true;

        // Assert
        Assert.Equal("Updated Film", filmUpdateDto.Name);
        Assert.Equal("Updated Description", filmUpdateDto.Description);
        Assert.True(filmUpdateDto.IsActive);
    }
}
