using Xunit;
using Films.Domain.Entities;
using System;

namespace Films.Domain.Entities.Tests;

public class RefDAFTests
{
    [Fact]
    public void RefDAF_SetId_ReturnsCorrectValue()
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
    public void RefDAF_SetDirectorId_ReturnsCorrectValue()
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
    public void RefDAF_SetFilmId_ReturnsCorrectValue()
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
    public void RefDAF_SetCreatedDate_ReturnsCorrectValue()
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
    public void RefDAF_SetIsActive_ReturnsCorrectValue()
    {
        // Arrange
        var refDAF = new RefDAF();

        // Act
        refDAF.IsActive = true;

        // Assert
        Assert.True(refDAF.IsActive);
    }

    [Fact]
    public void RefDAF_SetDirector_ReturnsCorrectValue()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedDirector = new Director { Id = 1, Name = "Christopher Nolan" };

        // Act
        refDAF.Director = expectedDirector;

        // Assert
        Assert.Equal(expectedDirector, refDAF.Director);
    }

    [Fact]
    public void RefDAF_SetFilm_ReturnsCorrectValue()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedFilm = new Film { Id = 1, Title = "Inception" };

        // Act
        refDAF.Film = expectedFilm;

        // Assert
        Assert.Equal(expectedFilm, refDAF.Film);
    }

    [Fact]
    public void RefDAF_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedId = 5;
        var expectedDirectorId = 15;
        var expectedFilmId = 25;
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedIsActive = true;
        var expectedDirector = new Director { Id = 15, Name = "Director Name" };
        var expectedFilm = new Film { Id = 25, Title = "Film Title" };

        // Act
        refDAF.Id = expectedId;
        refDAF.DirectorId = expectedDirectorId;
        refDAF.FilmId = expectedFilmId;
        refDAF.CreatedDate = expectedCreatedDate;
        refDAF.IsActive = expectedIsActive;
        refDAF.Director = expectedDirector;
        refDAF.Film = expectedFilm;

        // Assert
        Assert.Equal(expectedId, refDAF.Id);
        Assert.Equal(expectedDirectorId, refDAF.DirectorId);
        Assert.Equal(expectedFilmId, refDAF.FilmId);
        Assert.Equal(expectedCreatedDate, refDAF.CreatedDate);
        Assert.Equal(expectedIsActive, refDAF.IsActive);
        Assert.Equal(expectedDirector, refDAF.Director);
        Assert.Equal(expectedFilm, refDAF.Film);
    }

    [Fact]
    public void RefDAF_DirectorAndFilmRelationship_WorksCorrectly()
    {
        // Arrange
        var director = new Director { Id = 1, Name = "Steven Spielberg" };
        var film = new Film { Id = 1, Title = "Jurassic Park" };
        var refDAF = new RefDAF
        {
            Id = 1,
            DirectorId = director.Id,
            FilmId = film.Id,
            Director = director,
            Film = film
        };

        // Act & Assert
        Assert.Equal(director.Id, refDAF.DirectorId);
        Assert.Equal(film.Id, refDAF.FilmId);
        Assert.Equal(director, refDAF.Director);
        Assert.Equal(film, refDAF.Film);
    }

    [Fact]
    public void RefDAF_SetIsActive_False_ReturnsCorrectValue()
    {
        // Arrange
        var refDAF = new RefDAF();

        // Act
        refDAF.IsActive = false;

        // Assert
        Assert.False(refDAF.IsActive);
    }
}
