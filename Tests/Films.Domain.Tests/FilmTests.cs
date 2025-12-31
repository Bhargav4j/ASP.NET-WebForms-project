using Xunit;
using Films.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Films.Domain.Entities.Tests;

public class FilmTests
{
    [Fact]
    public void Film_Constructor_InitializesCollections()
    {
        // Arrange & Act
        var film = new Film();

        // Assert
        Assert.NotNull(film.ActorFilms);
        Assert.NotNull(film.DirectorFilms);
        Assert.Empty(film.ActorFilms);
        Assert.Empty(film.DirectorFilms);
    }

    [Fact]
    public void Film_SetId_ReturnsCorrectValue()
    {
        // Arrange
        var film = new Film();
        var expectedId = 1;

        // Act
        film.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, film.Id);
    }

    [Fact]
    public void Film_SetTitle_ReturnsCorrectValue()
    {
        // Arrange
        var film = new Film();
        var expectedTitle = "The Matrix";

        // Act
        film.Title = expectedTitle;

        // Assert
        Assert.Equal(expectedTitle, film.Title);
    }

    [Fact]
    public void Film_DefaultTitle_IsEmptyString()
    {
        // Arrange & Act
        var film = new Film();

        // Assert
        Assert.Equal(string.Empty, film.Title);
    }

    [Fact]
    public void Film_SetDescription_ReturnsCorrectValue()
    {
        // Arrange
        var film = new Film();
        var expectedDescription = "A sci-fi action film";

        // Act
        film.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, film.Description);
    }

    [Fact]
    public void Film_SetDescription_AllowsNull()
    {
        // Arrange
        var film = new Film();

        // Act
        film.Description = null;

        // Assert
        Assert.Null(film.Description);
    }

    [Fact]
    public void Film_SetYear_ReturnsCorrectValue()
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
    public void Film_SetYear_AllowsNull()
    {
        // Arrange
        var film = new Film();

        // Act
        film.Year = null;

        // Assert
        Assert.Null(film.Year);
    }

    [Fact]
    public void Film_SetGenre_ReturnsCorrectValue()
    {
        // Arrange
        var film = new Film();
        var expectedGenre = "Sci-Fi";

        // Act
        film.Genre = expectedGenre;

        // Assert
        Assert.Equal(expectedGenre, film.Genre);
    }

    [Fact]
    public void Film_SetIsActive_ReturnsCorrectValue()
    {
        // Arrange
        var film = new Film();

        // Act
        film.IsActive = true;

        // Assert
        Assert.True(film.IsActive);
    }

    [Fact]
    public void Film_SetCreatedDate_ReturnsCorrectValue()
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
    public void Film_SetModifiedDate_ReturnsCorrectValue()
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
    public void Film_SetCreatedBy_ReturnsCorrectValue()
    {
        // Arrange
        var film = new Film();
        var expectedCreatedBy = "Admin";

        // Act
        film.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, film.CreatedBy);
    }

    [Fact]
    public void Film_DefaultCreatedBy_IsEmptyString()
    {
        // Arrange & Act
        var film = new Film();

        // Assert
        Assert.Equal(string.Empty, film.CreatedBy);
    }

    [Fact]
    public void Film_SetModifiedBy_ReturnsCorrectValue()
    {
        // Arrange
        var film = new Film();
        var expectedModifiedBy = "Admin";

        // Act
        film.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, film.ModifiedBy);
    }

    [Fact]
    public void Film_AddActorFilm_AddsToCollection()
    {
        // Arrange
        var film = new Film();
        var actorFilm = new RefAF { Id = 1, ActorId = 1, FilmId = 1 };

        // Act
        film.ActorFilms.Add(actorFilm);

        // Assert
        Assert.Single(film.ActorFilms);
        Assert.Contains(actorFilm, film.ActorFilms);
    }

    [Fact]
    public void Film_AddDirectorFilm_AddsToCollection()
    {
        // Arrange
        var film = new Film();
        var directorFilm = new RefDAF { Id = 1, DirectorId = 1, FilmId = 1 };

        // Act
        film.DirectorFilms.Add(directorFilm);

        // Assert
        Assert.Single(film.DirectorFilms);
        Assert.Contains(directorFilm, film.DirectorFilms);
    }

    [Fact]
    public void Film_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var film = new Film();
        var expectedId = 10;
        var expectedTitle = "Inception";
        var expectedDescription = "A dream within a dream";
        var expectedYear = 2010;
        var expectedGenre = "Thriller";
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedModifiedDate = DateTime.UtcNow.AddDays(1);
        var expectedIsActive = true;
        var expectedCreatedBy = "User1";
        var expectedModifiedBy = "User2";

        // Act
        film.Id = expectedId;
        film.Title = expectedTitle;
        film.Description = expectedDescription;
        film.Year = expectedYear;
        film.Genre = expectedGenre;
        film.CreatedDate = expectedCreatedDate;
        film.ModifiedDate = expectedModifiedDate;
        film.IsActive = expectedIsActive;
        film.CreatedBy = expectedCreatedBy;
        film.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, film.Id);
        Assert.Equal(expectedTitle, film.Title);
        Assert.Equal(expectedDescription, film.Description);
        Assert.Equal(expectedYear, film.Year);
        Assert.Equal(expectedGenre, film.Genre);
        Assert.Equal(expectedCreatedDate, film.CreatedDate);
        Assert.Equal(expectedModifiedDate, film.ModifiedDate);
        Assert.Equal(expectedIsActive, film.IsActive);
        Assert.Equal(expectedCreatedBy, film.CreatedBy);
        Assert.Equal(expectedModifiedBy, film.ModifiedBy);
    }
}
