using Xunit;
using Films.Domain.Entities;

namespace Films.Domain.Tests;

public class RefAFTests
{
    [Fact]
    public void RefAF_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var refAF = new RefAF();

        // Assert
        Assert.Equal(0, refAF.Id);
        Assert.Equal(0, refAF.ActorId);
        Assert.Equal(0, refAF.FilmId);
        Assert.True(refAF.IsActive);
        Assert.Equal("System", refAF.CreatedBy);
        Assert.Null(refAF.ModifiedBy);
        Assert.Null(refAF.ModifiedDate);
    }

    [Fact]
    public void RefAF_SetProperties_SetsCorrectly()
    {
        // Arrange
        var refAF = new RefAF();
        var now = DateTime.UtcNow;

        // Act
        refAF.Id = 1;
        refAF.ActorId = 5;
        refAF.FilmId = 10;
        refAF.CreatedDate = now;
        refAF.ModifiedDate = now;
        refAF.IsActive = false;
        refAF.CreatedBy = "Admin";
        refAF.ModifiedBy = "User";

        // Assert
        Assert.Equal(1, refAF.Id);
        Assert.Equal(5, refAF.ActorId);
        Assert.Equal(10, refAF.FilmId);
        Assert.Equal(now, refAF.CreatedDate);
        Assert.Equal(now, refAF.ModifiedDate);
        Assert.False(refAF.IsActive);
        Assert.Equal("Admin", refAF.CreatedBy);
        Assert.Equal("User", refAF.ModifiedBy);
    }

    [Fact]
    public void RefAF_IsActive_DefaultsToTrue()
    {
        // Arrange & Act
        var refAF = new RefAF();

        // Assert
        Assert.True(refAF.IsActive);
    }

    [Fact]
    public void RefAF_NavigationProperties_CanBeSet()
    {
        // Arrange
        var refAF = new RefAF();
        var actor = new Actor { Id = 1, Name = "John" };
        var film = new Film { Id = 1, Name = "Test Film" };

        // Act
        refAF.Actor = actor;
        refAF.Film = film;
        refAF.ActorId = actor.Id;
        refAF.FilmId = film.Id;

        // Assert
        Assert.NotNull(refAF.Actor);
        Assert.NotNull(refAF.Film);
        Assert.Equal(1, refAF.ActorId);
        Assert.Equal(1, refAF.FilmId);
        Assert.Equal("John", refAF.Actor.Name);
        Assert.Equal("Test Film", refAF.Film.Name);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(5, 10)]
    [InlineData(100, 200)]
    public void RefAF_ActorIdAndFilmId_AcceptsVariousValues(int actorId, int filmId)
    {
        // Arrange
        var refAF = new RefAF();

        // Act
        refAF.ActorId = actorId;
        refAF.FilmId = filmId;

        // Assert
        Assert.Equal(actorId, refAF.ActorId);
        Assert.Equal(filmId, refAF.FilmId);
    }

    [Fact]
    public void RefAF_CreatedDate_CanBeSet()
    {
        // Arrange
        var refAF = new RefAF();
        var date = new DateTime(2020, 1, 1);

        // Act
        refAF.CreatedDate = date;

        // Assert
        Assert.Equal(date, refAF.CreatedDate);
    }

    [Fact]
    public void RefAF_ModifiedDate_CanBeNull()
    {
        // Arrange
        var refAF = new RefAF { ModifiedDate = DateTime.UtcNow };

        // Act
        refAF.ModifiedDate = null;

        // Assert
        Assert.Null(refAF.ModifiedDate);
    }

    [Fact]
    public void RefAF_ModifiedBy_CanBeNull()
    {
        // Arrange
        var refAF = new RefAF { ModifiedBy = "User" };

        // Act
        refAF.ModifiedBy = null;

        // Assert
        Assert.Null(refAF.ModifiedBy);
    }
}
