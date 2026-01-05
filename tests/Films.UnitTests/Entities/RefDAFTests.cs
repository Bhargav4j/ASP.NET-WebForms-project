using Films.Domain.Entities;
using Xunit;

namespace Films.UnitTests.Entities;

public class RefDAFTests
{
    [Fact]
    public void RefDAF_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var refDAF = new RefDAF();

        // Assert
        Assert.NotNull(refDAF);
        Assert.Equal(string.Empty, refDAF.CreatedBy);
    }

    [Fact]
    public void RefDAF_SetId_ShouldSetCorrectly()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedId = 1;

        // Act
        refDAF.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, refDAF.Id);
    }

    [Fact]
    public void RefDAF_SetDirectorId_ShouldSetCorrectly()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedDirectorId = 10;

        // Act
        refDAF.DirectorId = expectedDirectorId;

        // Assert
        Assert.Equal(expectedDirectorId, refDAF.DirectorId);
    }

    [Fact]
    public void RefDAF_SetFilmId_ShouldSetCorrectly()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedFilmId = 20;

        // Act
        refDAF.FilmId = expectedFilmId;

        // Assert
        Assert.Equal(expectedFilmId, refDAF.FilmId);
    }

    [Fact]
    public void RefDAF_SetCreatedDate_ShouldSetCorrectly()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedDate = DateTime.UtcNow;

        // Act
        refDAF.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, refDAF.CreatedDate);
    }

    [Fact]
    public void RefDAF_SetModifiedDate_ShouldSetCorrectly()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedDate = DateTime.UtcNow;

        // Act
        refDAF.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, refDAF.ModifiedDate);
    }

    [Fact]
    public void RefDAF_SetIsActive_ShouldSetCorrectly()
    {
        // Arrange
        var refDAF = new RefDAF();

        // Act
        refDAF.IsActive = true;

        // Assert
        Assert.True(refDAF.IsActive);
    }

    [Fact]
    public void RefDAF_SetCreatedBy_ShouldSetCorrectly()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedUser = "admin";

        // Act
        refDAF.CreatedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, refDAF.CreatedBy);
    }

    [Fact]
    public void RefDAF_SetModifiedBy_ShouldSetCorrectly()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedUser = "admin";

        // Act
        refDAF.ModifiedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, refDAF.ModifiedBy);
    }

    [Fact]
    public void RefDAF_SetDirector_ShouldSetCorrectly()
    {
        // Arrange
        var refDAF = new RefDAF();
        var director = new Director { Id = 1, Name = "Test Director" };

        // Act
        refDAF.Director = director;

        // Assert
        Assert.NotNull(refDAF.Director);
        Assert.Equal(1, refDAF.Director.Id);
        Assert.Equal("Test Director", refDAF.Director.Name);
    }

    [Fact]
    public void RefDAF_SetFilm_ShouldSetCorrectly()
    {
        // Arrange
        var refDAF = new RefDAF();
        var film = new Film { Id = 1, Name = "Test Film" };

        // Act
        refDAF.Film = film;

        // Assert
        Assert.NotNull(refDAF.Film);
        Assert.Equal(1, refDAF.Film.Id);
        Assert.Equal("Test Film", refDAF.Film.Name);
    }

    [Fact]
    public void RefDAF_AllProperties_ShouldWorkTogether()
    {
        // Arrange & Act
        var refDAF = new RefDAF
        {
            Id = 1,
            DirectorId = 5,
            FilmId = 10,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "system",
            ModifiedBy = "admin"
        };

        // Assert
        Assert.Equal(1, refDAF.Id);
        Assert.Equal(5, refDAF.DirectorId);
        Assert.Equal(10, refDAF.FilmId);
        Assert.True(refDAF.IsActive);
        Assert.Equal("system", refDAF.CreatedBy);
        Assert.Equal("admin", refDAF.ModifiedBy);
    }

    [Fact]
    public void RefDAF_NavigationProperties_ShouldWorkCorrectly()
    {
        // Arrange
        var director = new Director { Id = 1, Name = "Director One" };
        var film = new Film { Id = 2, Name = "Film One" };

        // Act
        var refDAF = new RefDAF
        {
            Id = 1,
            DirectorId = director.Id,
            FilmId = film.Id,
            Director = director,
            Film = film
        };

        // Assert
        Assert.Equal(director.Id, refDAF.DirectorId);
        Assert.Equal(film.Id, refDAF.FilmId);
        Assert.Equal(director, refDAF.Director);
        Assert.Equal(film, refDAF.Film);
    }
}
