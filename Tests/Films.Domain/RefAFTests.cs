using Xunit;
using Films.Domain.Entities;
using System;

namespace Films.Domain.Tests;

public class RefAFTests
{
    [Fact]
    public void RefAF_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var refAF = new RefAF();

        // Assert
        Assert.Equal(0, refAF.Id);
        Assert.Equal(0, refAF.ActorId);
        Assert.Equal(0, refAF.FilmId);
        Assert.Equal(default(DateTime), refAF.CreatedDate);
        Assert.Null(refAF.ModifiedDate);
        Assert.False(refAF.IsActive);
        Assert.Equal(string.Empty, refAF.CreatedBy);
        Assert.Null(refAF.ModifiedBy);
        Assert.Null(refAF.Actor);
        Assert.Null(refAF.Film);
    }

    [Fact]
    public void RefAF_Id_CanBeSetAndRetrieved()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedId = 555;

        // Act
        refAF.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, refAF.Id);
    }

    [Fact]
    public void RefAF_ActorId_CanBeSetAndRetrieved()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedActorId = 10;

        // Act
        refAF.ActorId = expectedActorId;

        // Assert
        Assert.Equal(expectedActorId, refAF.ActorId);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(999)]
    public void RefAF_ActorId_AcceptsVariousValues(int actorId)
    {
        // Arrange
        var refAF = new RefAF();

        // Act
        refAF.ActorId = actorId;

        // Assert
        Assert.Equal(actorId, refAF.ActorId);
    }

    [Fact]
    public void RefAF_FilmId_CanBeSetAndRetrieved()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedFilmId = 20;

        // Act
        refAF.FilmId = expectedFilmId;

        // Assert
        Assert.Equal(expectedFilmId, refAF.FilmId);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(500)]
    public void RefAF_FilmId_AcceptsVariousValues(int filmId)
    {
        // Arrange
        var refAF = new RefAF();

        // Act
        refAF.FilmId = filmId;

        // Assert
        Assert.Equal(filmId, refAF.FilmId);
    }

    [Fact]
    public void RefAF_CreatedDate_CanBeSetAndRetrieved()
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
    public void RefAF_ModifiedDate_CanBeSetAndRetrieved()
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
    public void RefAF_ModifiedDate_CanBeNull()
    {
        // Arrange
        var refAF = new RefAF();

        // Act
        refAF.ModifiedDate = null;

        // Assert
        Assert.Null(refAF.ModifiedDate);
    }

    [Fact]
    public void RefAF_IsActive_CanBeSetToTrue()
    {
        // Arrange
        var refAF = new RefAF();

        // Act
        refAF.IsActive = true;

        // Assert
        Assert.True(refAF.IsActive);
    }

    [Fact]
    public void RefAF_IsActive_CanBeSetToFalse()
    {
        // Arrange
        var refAF = new RefAF();

        // Act
        refAF.IsActive = false;

        // Assert
        Assert.False(refAF.IsActive);
    }

    [Fact]
    public void RefAF_CreatedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedCreatedBy = "admin";

        // Act
        refAF.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, refAF.CreatedBy);
    }

    [Fact]
    public void RefAF_ModifiedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedModifiedBy = "user123";

        // Act
        refAF.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, refAF.ModifiedBy);
    }

    [Fact]
    public void RefAF_ModifiedBy_CanBeNull()
    {
        // Arrange
        var refAF = new RefAF();

        // Act
        refAF.ModifiedBy = null;

        // Assert
        Assert.Null(refAF.ModifiedBy);
    }

    [Fact]
    public void RefAF_Actor_CanBeSetAndRetrieved()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedActor = new Actor { Id = 10, Name = "John Doe" };

        // Act
        refAF.Actor = expectedActor;

        // Assert
        Assert.Equal(expectedActor, refAF.Actor);
    }

    [Fact]
    public void RefAF_Actor_CanBeNull()
    {
        // Arrange
        var refAF = new RefAF();

        // Act
        refAF.Actor = null;

        // Assert
        Assert.Null(refAF.Actor);
    }

    [Fact]
    public void RefAF_Film_CanBeSetAndRetrieved()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedFilm = new Film { Id = 20, Name = "The Matrix" };

        // Act
        refAF.Film = expectedFilm;

        // Assert
        Assert.Equal(expectedFilm, refAF.Film);
    }

    [Fact]
    public void RefAF_Film_CanBeNull()
    {
        // Arrange
        var refAF = new RefAF();

        // Act
        refAF.Film = null;

        // Assert
        Assert.Null(refAF.Film);
    }

    [Fact]
    public void RefAF_AllProperties_CanBeSetTogether()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedId = 777;
        var expectedActorId = 88;
        var expectedFilmId = 99;
        var expectedCreatedDate = DateTime.Now;
        var expectedModifiedDate = DateTime.Now.AddDays(3);
        var expectedIsActive = true;
        var expectedCreatedBy = "system";
        var expectedModifiedBy = "moderator";
        var expectedActor = new Actor { Id = 88, Name = "Tom Hanks" };
        var expectedFilm = new Film { Id = 99, Name = "Forrest Gump" };

        // Act
        refAF.Id = expectedId;
        refAF.ActorId = expectedActorId;
        refAF.FilmId = expectedFilmId;
        refAF.CreatedDate = expectedCreatedDate;
        refAF.ModifiedDate = expectedModifiedDate;
        refAF.IsActive = expectedIsActive;
        refAF.CreatedBy = expectedCreatedBy;
        refAF.ModifiedBy = expectedModifiedBy;
        refAF.Actor = expectedActor;
        refAF.Film = expectedFilm;

        // Assert
        Assert.Equal(expectedId, refAF.Id);
        Assert.Equal(expectedActorId, refAF.ActorId);
        Assert.Equal(expectedFilmId, refAF.FilmId);
        Assert.Equal(expectedCreatedDate, refAF.CreatedDate);
        Assert.Equal(expectedModifiedDate, refAF.ModifiedDate);
        Assert.Equal(expectedIsActive, refAF.IsActive);
        Assert.Equal(expectedCreatedBy, refAF.CreatedBy);
        Assert.Equal(expectedModifiedBy, refAF.ModifiedBy);
        Assert.Equal(expectedActor, refAF.Actor);
        Assert.Equal(expectedFilm, refAF.Film);
    }
}
