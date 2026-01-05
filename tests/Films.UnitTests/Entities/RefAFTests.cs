using Films.Domain.Entities;
using Xunit;

namespace Films.UnitTests.Entities;

public class RefAFTests
{
    [Fact]
    public void RefAF_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var refAF = new RefAF();

        // Assert
        Assert.NotNull(refAF);
        Assert.Equal(string.Empty, refAF.CreatedBy);
    }

    [Fact]
    public void RefAF_SetId_ShouldSetCorrectly()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedId = 1;

        // Act
        refAF.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, refAF.Id);
    }

    [Fact]
    public void RefAF_SetActorId_ShouldSetCorrectly()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedActorId = 10;

        // Act
        refAF.ActorId = expectedActorId;

        // Assert
        Assert.Equal(expectedActorId, refAF.ActorId);
    }

    [Fact]
    public void RefAF_SetFilmId_ShouldSetCorrectly()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedFilmId = 20;

        // Act
        refAF.FilmId = expectedFilmId;

        // Assert
        Assert.Equal(expectedFilmId, refAF.FilmId);
    }

    [Fact]
    public void RefAF_SetCreatedDate_ShouldSetCorrectly()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedDate = DateTime.UtcNow;

        // Act
        refAF.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, refAF.CreatedDate);
    }

    [Fact]
    public void RefAF_SetModifiedDate_ShouldSetCorrectly()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedDate = DateTime.UtcNow;

        // Act
        refAF.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, refAF.ModifiedDate);
    }

    [Fact]
    public void RefAF_SetIsActive_ShouldSetCorrectly()
    {
        // Arrange
        var refAF = new RefAF();

        // Act
        refAF.IsActive = true;

        // Assert
        Assert.True(refAF.IsActive);
    }

    [Fact]
    public void RefAF_SetCreatedBy_ShouldSetCorrectly()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedUser = "admin";

        // Act
        refAF.CreatedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, refAF.CreatedBy);
    }

    [Fact]
    public void RefAF_SetModifiedBy_ShouldSetCorrectly()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedUser = "admin";

        // Act
        refAF.ModifiedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, refAF.ModifiedBy);
    }

    [Fact]
    public void RefAF_SetActor_ShouldSetCorrectly()
    {
        // Arrange
        var refAF = new RefAF();
        var actor = new Actor { Id = 1, Name = "Test Actor" };

        // Act
        refAF.Actor = actor;

        // Assert
        Assert.NotNull(refAF.Actor);
        Assert.Equal(1, refAF.Actor.Id);
        Assert.Equal("Test Actor", refAF.Actor.Name);
    }

    [Fact]
    public void RefAF_SetFilm_ShouldSetCorrectly()
    {
        // Arrange
        var refAF = new RefAF();
        var film = new Film { Id = 1, Name = "Test Film" };

        // Act
        refAF.Film = film;

        // Assert
        Assert.NotNull(refAF.Film);
        Assert.Equal(1, refAF.Film.Id);
        Assert.Equal("Test Film", refAF.Film.Name);
    }

    [Fact]
    public void RefAF_AllProperties_ShouldWorkTogether()
    {
        // Arrange & Act
        var refAF = new RefAF
        {
            Id = 1,
            ActorId = 5,
            FilmId = 10,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "system",
            ModifiedBy = "admin"
        };

        // Assert
        Assert.Equal(1, refAF.Id);
        Assert.Equal(5, refAF.ActorId);
        Assert.Equal(10, refAF.FilmId);
        Assert.True(refAF.IsActive);
        Assert.Equal("system", refAF.CreatedBy);
        Assert.Equal("admin", refAF.ModifiedBy);
    }

    [Fact]
    public void RefAF_NavigationProperties_ShouldWorkCorrectly()
    {
        // Arrange
        var actor = new Actor { Id = 1, Name = "Actor One" };
        var film = new Film { Id = 2, Name = "Film One" };

        // Act
        var refAF = new RefAF
        {
            Id = 1,
            ActorId = actor.Id,
            FilmId = film.Id,
            Actor = actor,
            Film = film
        };

        // Assert
        Assert.Equal(actor.Id, refAF.ActorId);
        Assert.Equal(film.Id, refAF.FilmId);
        Assert.Equal(actor, refAF.Actor);
        Assert.Equal(film, refAF.Film);
    }
}
