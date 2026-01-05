using Films.Domain.DTOs;
using Xunit;

namespace Films.UnitTests.DTOs;

public class FilmDtoTests
{
    [Fact]
    public void FilmDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var dto = new FilmDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.Name);
    }

    [Fact]
    public void FilmDto_AllProperties_ShouldSetCorrectly()
    {
        // Arrange & Act
        var dto = new FilmDto
        {
            Id = 1,
            Name = "Test Film",
            Description = "Test Description",
            Year = 2020,
            Genre = "Action",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true
        };

        // Assert
        Assert.Equal(1, dto.Id);
        Assert.Equal("Test Film", dto.Name);
        Assert.Equal("Test Description", dto.Description);
        Assert.Equal(2020, dto.Year);
        Assert.Equal("Action", dto.Genre);
        Assert.True(dto.IsActive);
    }

    [Fact]
    public void FilmCreateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var dto = new FilmCreateDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.Name);
    }

    [Fact]
    public void FilmCreateDto_AllProperties_ShouldSetCorrectly()
    {
        // Arrange & Act
        var dto = new FilmCreateDto
        {
            Name = "New Film",
            Description = "New Description",
            Year = 2023,
            Genre = "Drama"
        };

        // Assert
        Assert.Equal("New Film", dto.Name);
        Assert.Equal("New Description", dto.Description);
        Assert.Equal(2023, dto.Year);
        Assert.Equal("Drama", dto.Genre);
    }

    [Fact]
    public void FilmUpdateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var dto = new FilmUpdateDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.Name);
    }

    [Fact]
    public void FilmUpdateDto_AllProperties_ShouldSetCorrectly()
    {
        // Arrange & Act
        var dto = new FilmUpdateDto
        {
            Name = "Updated Film",
            Description = "Updated Description",
            Year = 2024,
            Genre = "Thriller"
        };

        // Assert
        Assert.Equal("Updated Film", dto.Name);
        Assert.Equal("Updated Description", dto.Description);
        Assert.Equal(2024, dto.Year);
        Assert.Equal("Thriller", dto.Genre);
    }
}
