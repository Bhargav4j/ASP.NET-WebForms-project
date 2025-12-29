using Xunit;
using Films.Domain.DTOs;

namespace Films.Domain.Tests;

public class FilmDtoTests
{
    [Fact]
    public void FilmDto_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var filmDto = new FilmDto();

        // Assert
        Assert.NotNull(filmDto);
        Assert.Equal(0, filmDto.Id);
        Assert.Equal(string.Empty, filmDto.Name);
        Assert.Null(filmDto.Description);
        Assert.Null(filmDto.Year);
        Assert.Null(filmDto.Genre);
        Assert.Equal(default(DateTime), filmDto.CreatedDate);
        Assert.Null(filmDto.ModifiedDate);
        Assert.False(filmDto.IsActive);
    }

    [Fact]
    public void FilmDto_SetProperties_ReturnsCorrectValues()
    {
        // Arrange
        var filmDto = new FilmDto();
        var testDate = DateTime.Now;

        // Act
        filmDto.Id = 1;
        filmDto.Name = "The Matrix";
        filmDto.Description = "A computer hacker learns about the true nature of reality";
        filmDto.Year = 1999;
        filmDto.Genre = "Sci-Fi";
        filmDto.CreatedDate = testDate;
        filmDto.ModifiedDate = testDate;
        filmDto.IsActive = true;

        // Assert
        Assert.Equal(1, filmDto.Id);
        Assert.Equal("The Matrix", filmDto.Name);
        Assert.Equal("A computer hacker learns about the true nature of reality", filmDto.Description);
        Assert.Equal(1999, filmDto.Year);
        Assert.Equal("Sci-Fi", filmDto.Genre);
        Assert.Equal(testDate, filmDto.CreatedDate);
        Assert.Equal(testDate, filmDto.ModifiedDate);
        Assert.True(filmDto.IsActive);
    }

    [Fact]
    public void FilmDto_NullableFields_CanBeNull()
    {
        // Arrange & Act
        var filmDto = new FilmDto
        {
            Description = null,
            Year = null,
            Genre = null,
            ModifiedDate = null
        };

        // Assert
        Assert.Null(filmDto.Description);
        Assert.Null(filmDto.Year);
        Assert.Null(filmDto.Genre);
        Assert.Null(filmDto.ModifiedDate);
    }

    [Fact]
    public void FilmDto_YearProperty_AcceptsValidYearRange()
    {
        // Arrange
        var filmDto = new FilmDto();

        // Act
        filmDto.Year = 1895; // First film ever made
        Assert.Equal(1895, filmDto.Year);

        filmDto.Year = 2025;
        Assert.Equal(2025, filmDto.Year);
    }
}

public class FilmCreateDtoTests
{
    [Fact]
    public void FilmCreateDto_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var filmCreateDto = new FilmCreateDto();

        // Assert
        Assert.NotNull(filmCreateDto);
        Assert.Equal(string.Empty, filmCreateDto.Name);
        Assert.Null(filmCreateDto.Description);
        Assert.Null(filmCreateDto.Year);
        Assert.Null(filmCreateDto.Genre);
    }

    [Fact]
    public void FilmCreateDto_SetProperties_ReturnsCorrectValues()
    {
        // Arrange
        var filmCreateDto = new FilmCreateDto();

        // Act
        filmCreateDto.Name = "Inception";
        filmCreateDto.Description = "A thief who steals corporate secrets through dream-sharing technology";
        filmCreateDto.Year = 2010;
        filmCreateDto.Genre = "Sci-Fi/Thriller";

        // Assert
        Assert.Equal("Inception", filmCreateDto.Name);
        Assert.Equal("A thief who steals corporate secrets through dream-sharing technology", filmCreateDto.Description);
        Assert.Equal(2010, filmCreateDto.Year);
        Assert.Equal("Sci-Fi/Thriller", filmCreateDto.Genre);
    }

    [Fact]
    public void FilmCreateDto_NullableFields_CanBeNull()
    {
        // Arrange & Act
        var filmCreateDto = new FilmCreateDto
        {
            Name = "Untitled Film",
            Description = null,
            Year = null,
            Genre = null
        };

        // Assert
        Assert.Null(filmCreateDto.Description);
        Assert.Null(filmCreateDto.Year);
        Assert.Null(filmCreateDto.Genre);
    }

    [Fact]
    public void FilmCreateDto_EmptyName_IsAccepted()
    {
        // Arrange & Act
        var filmCreateDto = new FilmCreateDto
        {
            Name = ""
        };

        // Assert
        Assert.Equal(string.Empty, filmCreateDto.Name);
    }

    [Fact]
    public void FilmCreateDto_WithMinimalData_IsValid()
    {
        // Arrange & Act
        var filmCreateDto = new FilmCreateDto
        {
            Name = "Test Film"
        };

        // Assert
        Assert.Equal("Test Film", filmCreateDto.Name);
        Assert.Null(filmCreateDto.Description);
        Assert.Null(filmCreateDto.Year);
        Assert.Null(filmCreateDto.Genre);
    }
}

public class FilmUpdateDtoTests
{
    [Fact]
    public void FilmUpdateDto_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var filmUpdateDto = new FilmUpdateDto();

        // Assert
        Assert.NotNull(filmUpdateDto);
        Assert.Equal(string.Empty, filmUpdateDto.Name);
        Assert.Null(filmUpdateDto.Description);
        Assert.Null(filmUpdateDto.Year);
        Assert.Null(filmUpdateDto.Genre);
    }

    [Fact]
    public void FilmUpdateDto_SetProperties_ReturnsCorrectValues()
    {
        // Arrange
        var filmUpdateDto = new FilmUpdateDto();

        // Act
        filmUpdateDto.Name = "The Matrix Reloaded";
        filmUpdateDto.Description = "Freedom fighters revolt against machines";
        filmUpdateDto.Year = 2003;
        filmUpdateDto.Genre = "Action/Sci-Fi";

        // Assert
        Assert.Equal("The Matrix Reloaded", filmUpdateDto.Name);
        Assert.Equal("Freedom fighters revolt against machines", filmUpdateDto.Description);
        Assert.Equal(2003, filmUpdateDto.Year);
        Assert.Equal("Action/Sci-Fi", filmUpdateDto.Genre);
    }

    [Fact]
    public void FilmUpdateDto_NullableFields_CanBeNull()
    {
        // Arrange & Act
        var filmUpdateDto = new FilmUpdateDto
        {
            Name = "Updated Film",
            Description = null,
            Year = null,
            Genre = null
        };

        // Assert
        Assert.Null(filmUpdateDto.Description);
        Assert.Null(filmUpdateDto.Year);
        Assert.Null(filmUpdateDto.Genre);
    }

    [Fact]
    public void FilmUpdateDto_EmptyName_IsAccepted()
    {
        // Arrange & Act
        var filmUpdateDto = new FilmUpdateDto
        {
            Name = ""
        };

        // Assert
        Assert.Equal(string.Empty, filmUpdateDto.Name);
    }

    [Fact]
    public void FilmUpdateDto_PartialUpdate_IsSupported()
    {
        // Arrange & Act
        var filmUpdateDto = new FilmUpdateDto
        {
            Name = "Updated Title",
            Year = 2024
            // Description and Genre remain null
        };

        // Assert
        Assert.Equal("Updated Title", filmUpdateDto.Name);
        Assert.Equal(2024, filmUpdateDto.Year);
        Assert.Null(filmUpdateDto.Description);
        Assert.Null(filmUpdateDto.Genre);
    }
}
