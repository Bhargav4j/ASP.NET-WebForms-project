using Xunit;
using Films.Domain.Entities;
using System;

namespace Films.Domain.Entities.Tests;

public class DirectorTests
{
    [Fact]
    public void Director_Constructor_InitializesCollections()
    {
        // Arrange & Act
        var director = new Director();

        // Assert
        Assert.NotNull(director.DirectorFilms);
        Assert.Empty(director.DirectorFilms);
    }

    [Fact]
    public void Director_SetId_ReturnsCorrectValue()
    {
        // Arrange
        var director = new Director();
        var expectedId = 1;

        // Act
        director.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, director.Id);
    }

    [Fact]
    public void Director_SetName_ReturnsCorrectValue()
    {
        // Arrange
        var director = new Director();
        var expectedName = "Christopher Nolan";

        // Act
        director.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, director.Name);
    }

    [Fact]
    public void Director_DefaultName_IsEmptyString()
    {
        // Arrange & Act
        var director = new Director();

        // Assert
        Assert.Equal(string.Empty, director.Name);
    }

    [Fact]
    public void Director_SetDescription_ReturnsCorrectValue()
    {
        // Arrange
        var director = new Director();
        var expectedDescription = "Famous director";

        // Act
        director.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, director.Description);
    }

    [Fact]
    public void Director_SetDescription_AllowsNull()
    {
        // Arrange
        var director = new Director();

        // Act
        director.Description = null;

        // Assert
        Assert.Null(director.Description);
    }

    [Fact]
    public void Director_SetIsActive_ReturnsCorrectValue()
    {
        // Arrange
        var director = new Director();

        // Act
        director.IsActive = true;

        // Assert
        Assert.True(director.IsActive);
    }

    [Fact]
    public void Director_SetCreatedDate_ReturnsCorrectValue()
    {
        // Arrange
        var director = new Director();
        var expectedDate = DateTime.UtcNow;

        // Act
        director.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, director.CreatedDate);
    }

    [Fact]
    public void Director_SetModifiedDate_ReturnsCorrectValue()
    {
        // Arrange
        var director = new Director();
        var expectedDate = DateTime.UtcNow;

        // Act
        director.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, director.ModifiedDate);
    }

    [Fact]
    public void Director_SetCreatedBy_ReturnsCorrectValue()
    {
        // Arrange
        var director = new Director();
        var expectedCreatedBy = "Admin";

        // Act
        director.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, director.CreatedBy);
    }

    [Fact]
    public void Director_DefaultCreatedBy_IsEmptyString()
    {
        // Arrange & Act
        var director = new Director();

        // Assert
        Assert.Equal(string.Empty, director.CreatedBy);
    }

    [Fact]
    public void Director_SetModifiedBy_ReturnsCorrectValue()
    {
        // Arrange
        var director = new Director();
        var expectedModifiedBy = "Admin";

        // Act
        director.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, director.ModifiedBy);
    }

    [Fact]
    public void Director_AddDirectorFilm_AddsToCollection()
    {
        // Arrange
        var director = new Director();
        var directorFilm = new RefDAF { Id = 1, DirectorId = 1, FilmId = 1 };

        // Act
        director.DirectorFilms.Add(directorFilm);

        // Assert
        Assert.Single(director.DirectorFilms);
        Assert.Contains(directorFilm, director.DirectorFilms);
    }

    [Fact]
    public void Director_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var director = new Director();
        var expectedId = 10;
        var expectedName = "Steven Spielberg";
        var expectedDescription = "Award-winning director";
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedModifiedDate = DateTime.UtcNow.AddDays(1);
        var expectedIsActive = true;
        var expectedCreatedBy = "User1";
        var expectedModifiedBy = "User2";

        // Act
        director.Id = expectedId;
        director.Name = expectedName;
        director.Description = expectedDescription;
        director.CreatedDate = expectedCreatedDate;
        director.ModifiedDate = expectedModifiedDate;
        director.IsActive = expectedIsActive;
        director.CreatedBy = expectedCreatedBy;
        director.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, director.Id);
        Assert.Equal(expectedName, director.Name);
        Assert.Equal(expectedDescription, director.Description);
        Assert.Equal(expectedCreatedDate, director.CreatedDate);
        Assert.Equal(expectedModifiedDate, director.ModifiedDate);
        Assert.Equal(expectedIsActive, director.IsActive);
        Assert.Equal(expectedCreatedBy, director.CreatedBy);
        Assert.Equal(expectedModifiedBy, director.ModifiedBy);
    }
}
