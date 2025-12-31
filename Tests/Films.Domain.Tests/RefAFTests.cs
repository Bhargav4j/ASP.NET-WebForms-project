using Xunit;
using Films.Domain.Entities;
using System;

namespace Films.Domain.Entities.Tests;

public class RefAFTests
{
    [Fact]
    public void RefAF_SetId_ReturnsCorrectValue()
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
    public void RefAF_SetActorId_ReturnsCorrectValue()
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
    public void RefAF_SetFilmId_ReturnsCorrectValue()
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
    public void RefAF_SetCreatedDate_ReturnsCorrectValue()
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
    public void RefAF_SetIsActive_ReturnsCorrectValue()
    {
        // Arrange
        var refAF = new RefAF();

        // Act
        refAF.IsActive = true;

        // Assert
        Assert.True(refAF.IsActive);
    }

    [Fact]
    public void RefAF_SetActor_ReturnsCorrectValue()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedActor = new Actor { Id = 1, Name = "Tom Hanks" };

        // Act
        refAF.Actor = expectedActor;

        // Assert
        Assert.Equal(expectedActor, refAF.Actor);
    }

    [Fact]
    public void RefAF_SetFilm_ReturnsCorrectValue()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedFilm = new Film { Id = 1, Title = "Forrest Gump" };

        // Act
        refAF.Film = expectedFilm;

        // Assert
        Assert.Equal(expectedFilm, refAF.Film);
    }

    [Fact]
    public void RefAF_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var refAF = new RefAF();
        var expectedId = 5;
        var expectedActorId = 15;
        var expectedFilmId = 25;
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedIsActive = true;
        var expectedActor = new Actor { Id = 15, Name = "Actor Name" };
        var expectedFilm = new Film { Id = 25, Title = "Film Title" };

        // Act
        refAF.Id = expectedId;
        refAF.ActorId = expectedActorId;
        refAF.FilmId = expectedFilmId;
        refAF.CreatedDate = expectedCreatedDate;
        refAF.IsActive = expectedIsActive;
        refAF.Actor = expectedActor;
        refAF.Film = expectedFilm;

        // Assert
        Assert.Equal(expectedId, refAF.Id);
        Assert.Equal(expectedActorId, refAF.ActorId);
        Assert.Equal(expectedFilmId, refAF.FilmId);
        Assert.Equal(expectedCreatedDate, refAF.CreatedDate);
        Assert.Equal(expectedIsActive, refAF.IsActive);
        Assert.Equal(expectedActor, refAF.Actor);
        Assert.Equal(expectedFilm, refAF.Film);
    }

    [Fact]
    public void RefAF_ActorAndFilmRelationship_WorksCorrectly()
    {
        // Arrange
        var actor = new Actor { Id = 1, Name = "Leonardo DiCaprio" };
        var film = new Film { Id = 1, Title = "Inception" };
        var refAF = new RefAF
        {
            Id = 1,
            ActorId = actor.Id,
            FilmId = film.Id,
            Actor = actor,
            Film = film
        };

        // Act & Assert
        Assert.Equal(actor.Id, refAF.ActorId);
        Assert.Equal(film.Id, refAF.FilmId);
        Assert.Equal(actor, refAF.Actor);
        Assert.Equal(film, refAF.Film);
    }

    [Fact]
    public void RefAF_SetIsActive_False_ReturnsCorrectValue()
    {
        // Arrange
        var refAF = new RefAF();

        // Act
        refAF.IsActive = false;

        // Assert
        Assert.False(refAF.IsActive);
    }
}
