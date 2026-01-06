using Xunit;
using Films.Domain.Entities;

namespace Films.Domain.Tests;

public class FilmTests
{
    [Fact]
    public void Film_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var film = new Film();

        // Assert
        Assert.Equal(0, film.Id);
        Assert.Equal(string.Empty, film.Name);
        Assert.Null(film.Description);
        Assert.Null(film.Year);
        Assert.Null(film.Genre);
        Assert.True(film.IsActive);
        Assert.Equal("System", film.CreatedBy);
        Assert.Null(film.ModifiedBy);
        Assert.Null(film.ModifiedDate);
        Assert.NotNull(film.RefAFs);
        Assert.NotNull(film.RefDAFs);
        Assert.Empty(film.RefAFs);
        Assert.Empty(film.RefDAFs);
    }

    [Fact]
    public void Film_SetProperties_SetsCorrectly()
    {
        // Arrange
        var film = new Film();
        var now = DateTime.UtcNow;

        // Act
        film.Id = 1;
        film.Name = "Test Film";
        film.Description = "Test Description";
        film.Year = 2024;
        film.Genre = "Action";
        film.CreatedDate = now;
        film.ModifiedDate = now;
        film.IsActive = false;
        film.CreatedBy = "Admin";
        film.ModifiedBy = "User";

        // Assert
        Assert.Equal(1, film.Id);
        Assert.Equal("Test Film", film.Name);
        Assert.Equal("Test Description", film.Description);
        Assert.Equal(2024, film.Year);
        Assert.Equal("Action", film.Genre);
        Assert.Equal(now, film.CreatedDate);
        Assert.Equal(now, film.ModifiedDate);
        Assert.False(film.IsActive);
        Assert.Equal("Admin", film.CreatedBy);
        Assert.Equal("User", film.ModifiedBy);
    }

    [Fact]
    public void Film_Name_CanBeSetToEmptyString()
    {
        // Arrange
        var film = new Film { Name = "Initial" };

        // Act
        film.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, film.Name);
    }

    [Fact]
    public void Film_Year_CanBeNull()
    {
        // Arrange
        var film = new Film { Year = 2024 };

        // Act
        film.Year = null;

        // Assert
        Assert.Null(film.Year);
    }

    [Fact]
    public void Film_RefAFs_CanAddItems()
    {
        // Arrange
        var film = new Film();
        var refAF = new RefAF { Id = 1, FilmId = 1, ActorId = 1 };

        // Act
        film.RefAFs.Add(refAF);

        // Assert
        Assert.Single(film.RefAFs);
        Assert.Contains(refAF, film.RefAFs);
    }

    [Fact]
    public void Film_RefDAFs_CanAddItems()
    {
        // Arrange
        var film = new Film();
        var refDAF = new RefDAF { Id = 1, FilmId = 1, DirectedById = 1 };

        // Act
        film.RefDAFs.Add(refDAF);

        // Assert
        Assert.Single(film.RefDAFs);
        Assert.Contains(refDAF, film.RefDAFs);
    }

    [Fact]
    public void Film_IsActive_DefaultsToTrue()
    {
        // Arrange & Act
        var film = new Film();

        // Assert
        Assert.True(film.IsActive);
    }

    [Theory]
    [InlineData(1900)]
    [InlineData(2000)]
    [InlineData(2024)]
    [InlineData(2100)]
    public void Film_Year_AcceptsVariousYears(int year)
    {
        // Arrange
        var film = new Film();

        // Act
        film.Year = year;

        // Assert
        Assert.Equal(year, film.Year);
    }

    [Theory]
    [InlineData("Action")]
    [InlineData("Drama")]
    [InlineData("Comedy")]
    [InlineData("")]
    [InlineData(null)]
    public void Film_Genre_AcceptsVariousGenres(string? genre)
    {
        // Arrange
        var film = new Film();

        // Act
        film.Genre = genre;

        // Assert
        Assert.Equal(genre, film.Genre);
    }
}
