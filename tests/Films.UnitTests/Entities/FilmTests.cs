using Films.Domain.Entities;
using Xunit;

namespace Films.UnitTests.Entities;

public class FilmTests
{
    [Fact]
    public void Film_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var film = new Film();

        // Assert
        Assert.NotNull(film);
        Assert.Equal(string.Empty, film.Name);
        Assert.Equal(string.Empty, film.CreatedBy);
        Assert.NotNull(film.RefAFs);
        Assert.NotNull(film.RefDAFs);
        Assert.Empty(film.RefAFs);
        Assert.Empty(film.RefDAFs);
    }

    [Fact]
    public void Film_SetId_ShouldSetCorrectly()
    {
        // Arrange
        var film = new Film();
        var expectedId = 42;

        // Act
        film.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, film.Id);
    }

    [Fact]
    public void Film_SetName_ShouldSetCorrectly()
    {
        // Arrange
        var film = new Film();
        var expectedName = "The Matrix";

        // Act
        film.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, film.Name);
    }

    [Fact]
    public void Film_SetDescription_ShouldSetCorrectly()
    {
        // Arrange
        var film = new Film();
        var expectedDescription = "A computer hacker learns about the true nature of reality.";

        // Act
        film.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, film.Description);
    }

    [Fact]
    public void Film_SetYear_ShouldSetCorrectly()
    {
        // Arrange
        var film = new Film();
        var expectedYear = 1999;

        // Act
        film.Year = expectedYear;

        // Assert
        Assert.Equal(expectedYear, film.Year);
    }

    [Fact]
    public void Film_SetGenre_ShouldSetCorrectly()
    {
        // Arrange
        var film = new Film();
        var expectedGenre = "Science Fiction";

        // Act
        film.Genre = expectedGenre;

        // Assert
        Assert.Equal(expectedGenre, film.Genre);
    }

    [Fact]
    public void Film_SetCreatedDate_ShouldSetCorrectly()
    {
        // Arrange
        var film = new Film();
        var expectedDate = DateTime.UtcNow;

        // Act
        film.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, film.CreatedDate);
    }

    [Fact]
    public void Film_SetModifiedDate_ShouldSetCorrectly()
    {
        // Arrange
        var film = new Film();
        var expectedDate = DateTime.UtcNow;

        // Act
        film.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, film.ModifiedDate);
    }

    [Fact]
    public void Film_SetIsActive_ShouldSetCorrectly()
    {
        // Arrange
        var film = new Film();

        // Act
        film.IsActive = true;

        // Assert
        Assert.True(film.IsActive);
    }

    [Fact]
    public void Film_SetCreatedBy_ShouldSetCorrectly()
    {
        // Arrange
        var film = new Film();
        var expectedUser = "admin";

        // Act
        film.CreatedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, film.CreatedBy);
    }

    [Fact]
    public void Film_SetModifiedBy_ShouldSetCorrectly()
    {
        // Arrange
        var film = new Film();
        var expectedUser = "admin";

        // Act
        film.ModifiedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, film.ModifiedBy);
    }

    [Fact]
    public void Film_SetRefAFs_ShouldSetCorrectly()
    {
        // Arrange
        var film = new Film();
        var refAFs = new List<RefAF> { new RefAF { Id = 1 } };

        // Act
        film.RefAFs = refAFs;

        // Assert
        Assert.Equal(refAFs, film.RefAFs);
        Assert.Single(film.RefAFs);
    }

    [Fact]
    public void Film_SetRefDAFs_ShouldSetCorrectly()
    {
        // Arrange
        var film = new Film();
        var refDAFs = new List<RefDAF> { new RefDAF { Id = 1 } };

        // Act
        film.RefDAFs = refDAFs;

        // Assert
        Assert.Equal(refDAFs, film.RefDAFs);
        Assert.Single(film.RefDAFs);
    }

    [Fact]
    public void Film_AllProperties_ShouldWorkTogether()
    {
        // Arrange & Act
        var film = new Film
        {
            Id = 1,
            Name = "Inception",
            Description = "A thief who steals corporate secrets",
            Year = 2010,
            Genre = "Sci-Fi",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "system",
            ModifiedBy = "admin"
        };

        // Assert
        Assert.Equal(1, film.Id);
        Assert.Equal("Inception", film.Name);
        Assert.Equal("A thief who steals corporate secrets", film.Description);
        Assert.Equal(2010, film.Year);
        Assert.Equal("Sci-Fi", film.Genre);
        Assert.True(film.IsActive);
        Assert.Equal("system", film.CreatedBy);
        Assert.Equal("admin", film.ModifiedBy);
    }
}
