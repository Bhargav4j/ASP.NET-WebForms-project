using Xunit;
using Films.Domain.Entities;

namespace Films.Domain.Tests;

public class RefDAFTests
{
    [Fact]
    public void RefDAF_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var refDAF = new RefDAF();

        // Assert
        Assert.Equal(0, refDAF.Id);
        Assert.Equal(0, refDAF.DirectedById);
        Assert.Equal(0, refDAF.FilmId);
        Assert.True(refDAF.IsActive);
        Assert.Equal("System", refDAF.CreatedBy);
        Assert.Null(refDAF.ModifiedBy);
        Assert.Null(refDAF.ModifiedDate);
    }

    [Fact]
    public void RefDAF_SetProperties_SetsCorrectly()
    {
        // Arrange
        var refDAF = new RefDAF();
        var now = DateTime.UtcNow;

        // Act
        refDAF.Id = 1;
        refDAF.DirectedById = 5;
        refDAF.FilmId = 10;
        refDAF.CreatedDate = now;
        refDAF.ModifiedDate = now;
        refDAF.IsActive = false;
        refDAF.CreatedBy = "Admin";
        refDAF.ModifiedBy = "User";

        // Assert
        Assert.Equal(1, refDAF.Id);
        Assert.Equal(5, refDAF.DirectedById);
        Assert.Equal(10, refDAF.FilmId);
        Assert.Equal(now, refDAF.CreatedDate);
        Assert.Equal(now, refDAF.ModifiedDate);
        Assert.False(refDAF.IsActive);
        Assert.Equal("Admin", refDAF.CreatedBy);
        Assert.Equal("User", refDAF.ModifiedBy);
    }

    [Fact]
    public void RefDAF_IsActive_DefaultsToTrue()
    {
        // Arrange & Act
        var refDAF = new RefDAF();

        // Assert
        Assert.True(refDAF.IsActive);
    }

    [Fact]
    public void RefDAF_NavigationProperties_CanBeSet()
    {
        // Arrange
        var refDAF = new RefDAF();
        var director = new DirectedBy { Id = 1, Name = "Steven" };
        var film = new Film { Id = 1, Name = "Test Film" };

        // Act
        refDAF.DirectedBy = director;
        refDAF.Film = film;
        refDAF.DirectedById = director.Id;
        refDAF.FilmId = film.Id;

        // Assert
        Assert.NotNull(refDAF.DirectedBy);
        Assert.NotNull(refDAF.Film);
        Assert.Equal(1, refDAF.DirectedById);
        Assert.Equal(1, refDAF.FilmId);
        Assert.Equal("Steven", refDAF.DirectedBy.Name);
        Assert.Equal("Test Film", refDAF.Film.Name);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(5, 10)]
    [InlineData(100, 200)]
    public void RefDAF_DirectedByIdAndFilmId_AcceptsVariousValues(int directedById, int filmId)
    {
        // Arrange
        var refDAF = new RefDAF();

        // Act
        refDAF.DirectedById = directedById;
        refDAF.FilmId = filmId;

        // Assert
        Assert.Equal(directedById, refDAF.DirectedById);
        Assert.Equal(filmId, refDAF.FilmId);
    }

    [Fact]
    public void RefDAF_CreatedDate_CanBeSet()
    {
        // Arrange
        var refDAF = new RefDAF();
        var date = new DateTime(2020, 1, 1);

        // Act
        refDAF.CreatedDate = date;

        // Assert
        Assert.Equal(date, refDAF.CreatedDate);
    }

    [Fact]
    public void RefDAF_ModifiedDate_CanBeNull()
    {
        // Arrange
        var refDAF = new RefDAF { ModifiedDate = DateTime.UtcNow };

        // Act
        refDAF.ModifiedDate = null;

        // Assert
        Assert.Null(refDAF.ModifiedDate);
    }

    [Fact]
    public void RefDAF_ModifiedBy_CanBeNull()
    {
        // Arrange
        var refDAF = new RefDAF { ModifiedBy = "User" };

        // Act
        refDAF.ModifiedBy = null;

        // Assert
        Assert.Null(refDAF.ModifiedBy);
    }
}
