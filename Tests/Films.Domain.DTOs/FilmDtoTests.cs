using Xunit;
using Films.Domain.DTOs;
using System;

namespace Films.Domain.DTOs.Tests;

public class FilmDtoTests
{
    [Fact]
    public void FilmDto_SetId_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new FilmDto();
        var expectedId = 1;

        // Act
        dto.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, dto.Id);
    }

    [Fact]
    public void FilmDto_SetTitle_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new FilmDto();
        var expectedTitle = "The Matrix";

        // Act
        dto.Title = expectedTitle;

        // Assert
        Assert.Equal(expectedTitle, dto.Title);
    }

    [Fact]
    public void FilmDto_DefaultTitle_IsEmptyString()
    {
        // Arrange & Act
        var dto = new FilmDto();

        // Assert
        Assert.Equal(string.Empty, dto.Title);
    }

    [Fact]
    public void FilmDto_SetDescription_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new FilmDto();
        var expectedDescription = "A sci-fi film";

        // Act
        dto.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, dto.Description);
    }

    [Fact]
    public void FilmDto_SetYear_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new FilmDto();
        var expectedYear = 1999;

        // Act
        dto.Year = expectedYear;

        // Assert
        Assert.Equal(expectedYear, dto.Year);
    }

    [Fact]
    public void FilmDto_SetGenre_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new FilmDto();
        var expectedGenre = "Sci-Fi";

        // Act
        dto.Genre = expectedGenre;

        // Assert
        Assert.Equal(expectedGenre, dto.Genre);
    }

    [Fact]
    public void FilmDto_SetIsActive_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new FilmDto();

        // Act
        dto.IsActive = true;

        // Assert
        Assert.True(dto.IsActive);
    }

    [Fact]
    public void FilmDto_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var dto = new FilmDto();
        var expectedId = 10;
        var expectedTitle = "Inception";
        var expectedDescription = "A dream film";
        var expectedYear = 2010;
        var expectedGenre = "Thriller";
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedIsActive = true;

        // Act
        dto.Id = expectedId;
        dto.Title = expectedTitle;
        dto.Description = expectedDescription;
        dto.Year = expectedYear;
        dto.Genre = expectedGenre;
        dto.CreatedDate = expectedCreatedDate;
        dto.IsActive = expectedIsActive;

        // Assert
        Assert.Equal(expectedId, dto.Id);
        Assert.Equal(expectedTitle, dto.Title);
        Assert.Equal(expectedDescription, dto.Description);
        Assert.Equal(expectedYear, dto.Year);
        Assert.Equal(expectedGenre, dto.Genre);
        Assert.Equal(expectedCreatedDate, dto.CreatedDate);
        Assert.Equal(expectedIsActive, dto.IsActive);
    }

    [Fact]
    public void FilmCreateDto_SetTitle_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new FilmCreateDto();
        var expectedTitle = "New Film";

        // Act
        dto.Title = expectedTitle;

        // Assert
        Assert.Equal(expectedTitle, dto.Title);
    }

    [Fact]
    public void FilmCreateDto_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var dto = new FilmCreateDto();
        var expectedTitle = "New Film";
        var expectedDescription = "New Description";
        var expectedYear = 2023;
        var expectedGenre = "Drama";

        // Act
        dto.Title = expectedTitle;
        dto.Description = expectedDescription;
        dto.Year = expectedYear;
        dto.Genre = expectedGenre;

        // Assert
        Assert.Equal(expectedTitle, dto.Title);
        Assert.Equal(expectedDescription, dto.Description);
        Assert.Equal(expectedYear, dto.Year);
        Assert.Equal(expectedGenre, dto.Genre);
    }

    [Fact]
    public void FilmUpdateDto_SetTitle_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new FilmUpdateDto();
        var expectedTitle = "Updated Film";

        // Act
        dto.Title = expectedTitle;

        // Assert
        Assert.Equal(expectedTitle, dto.Title);
    }

    [Fact]
    public void FilmUpdateDto_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var dto = new FilmUpdateDto();
        var expectedTitle = "Updated Film";
        var expectedDescription = "Updated Description";
        var expectedYear = 2024;
        var expectedGenre = "Action";

        // Act
        dto.Title = expectedTitle;
        dto.Description = expectedDescription;
        dto.Year = expectedYear;
        dto.Genre = expectedGenre;

        // Assert
        Assert.Equal(expectedTitle, dto.Title);
        Assert.Equal(expectedDescription, dto.Description);
        Assert.Equal(expectedYear, dto.Year);
        Assert.Equal(expectedGenre, dto.Genre);
    }
}
