using Xunit;
using Films.Domain.Entities;
using System;

namespace Films.Domain.Tests;

public class RefDAFTests
{
    [Fact]
    public void RefDAF_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var refDAF = new RefDAF();

        // Assert
        Assert.Equal(0, refDAF.Id);
        Assert.Equal(0, refDAF.DirectorId);
        Assert.Equal(0, refDAF.FilmId);
        Assert.Equal(default(DateTime), refDAF.CreatedDate);
        Assert.Null(refDAF.ModifiedDate);
        Assert.False(refDAF.IsActive);
        Assert.Equal(string.Empty, refDAF.CreatedBy);
        Assert.Null(refDAF.ModifiedBy);
        Assert.Null(refDAF.Director);
        Assert.Null(refDAF.Film);
    }

    [Fact]
    public void RefDAF_Id_CanBeSetAndRetrieved()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedId = 888;

        // Act
        refDAF.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, refDAF.Id);
    }

    [Fact]
    public void RefDAF_DirectorId_CanBeSetAndRetrieved()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedDirectorId = 15;

        // Act
        refDAF.DirectorId = expectedDirectorId;

        // Assert
        Assert.Equal(expectedDirectorId, refDAF.DirectorId);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(75)]
    [InlineData(999)]
    public void RefDAF_DirectorId_AcceptsVariousValues(int directorId)
    {
        // Arrange
        var refDAF = new RefDAF();

        // Act
        refDAF.DirectorId = directorId;

        // Assert
        Assert.Equal(directorId, refDAF.DirectorId);
    }

    [Fact]
    public void RefDAF_FilmId_CanBeSetAndRetrieved()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedFilmId = 25;

        // Act
        refDAF.FilmId = expectedFilmId;

        // Assert
        Assert.Equal(expectedFilmId, refDAF.FilmId);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(150)]
    [InlineData(777)]
    public void RefDAF_FilmId_AcceptsVariousValues(int filmId)
    {
        // Arrange
        var refDAF = new RefDAF();

        // Act
        refDAF.FilmId = filmId;

        // Assert
        Assert.Equal(filmId, refDAF.FilmId);
    }

    [Fact]
    public void RefDAF_CreatedDate_CanBeSetAndRetrieved()
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
    public void RefDAF_ModifiedDate_CanBeSetAndRetrieved()
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
    public void RefDAF_ModifiedDate_CanBeNull()
    {
        // Arrange
        var refDAF = new RefDAF();

        // Act
        refDAF.ModifiedDate = null;

        // Assert
        Assert.Null(refDAF.ModifiedDate);
    }

    [Fact]
    public void RefDAF_IsActive_CanBeSetToTrue()
    {
        // Arrange
        var refDAF = new RefDAF();

        // Act
        refDAF.IsActive = true;

        // Assert
        Assert.True(refDAF.IsActive);
    }

    [Fact]
    public void RefDAF_IsActive_CanBeSetToFalse()
    {
        // Arrange
        var refDAF = new RefDAF();

        // Act
        refDAF.IsActive = false;

        // Assert
        Assert.False(refDAF.IsActive);
    }

    [Fact]
    public void RefDAF_CreatedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedCreatedBy = "admin";

        // Act
        refDAF.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, refDAF.CreatedBy);
    }

    [Fact]
    public void RefDAF_ModifiedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedModifiedBy = "user456";

        // Act
        refDAF.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, refDAF.ModifiedBy);
    }

    [Fact]
    public void RefDAF_ModifiedBy_CanBeNull()
    {
        // Arrange
        var refDAF = new RefDAF();

        // Act
        refDAF.ModifiedBy = null;

        // Assert
        Assert.Null(refDAF.ModifiedBy);
    }

    [Fact]
    public void RefDAF_Director_CanBeSetAndRetrieved()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedDirector = new Director { Id = 15, Name = "Christopher Nolan" };

        // Act
        refDAF.Director = expectedDirector;

        // Assert
        Assert.Equal(expectedDirector, refDAF.Director);
    }

    [Fact]
    public void RefDAF_Director_CanBeNull()
    {
        // Arrange
        var refDAF = new RefDAF();

        // Act
        refDAF.Director = null;

        // Assert
        Assert.Null(refDAF.Director);
    }

    [Fact]
    public void RefDAF_Film_CanBeSetAndRetrieved()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedFilm = new Film { Id = 25, Name = "Inception" };

        // Act
        refDAF.Film = expectedFilm;

        // Assert
        Assert.Equal(expectedFilm, refDAF.Film);
    }

    [Fact]
    public void RefDAF_Film_CanBeNull()
    {
        // Arrange
        var refDAF = new RefDAF();

        // Act
        refDAF.Film = null;

        // Assert
        Assert.Null(refDAF.Film);
    }

    [Fact]
    public void RefDAF_AllProperties_CanBeSetTogether()
    {
        // Arrange
        var refDAF = new RefDAF();
        var expectedId = 666;
        var expectedDirectorId = 44;
        var expectedFilmId = 55;
        var expectedCreatedDate = DateTime.Now;
        var expectedModifiedDate = DateTime.Now.AddDays(2);
        var expectedIsActive = true;
        var expectedCreatedBy = "system";
        var expectedModifiedBy = "administrator";
        var expectedDirector = new Director { Id = 44, Name = "Steven Spielberg" };
        var expectedFilm = new Film { Id = 55, Name = "Jurassic Park" };

        // Act
        refDAF.Id = expectedId;
        refDAF.DirectorId = expectedDirectorId;
        refDAF.FilmId = expectedFilmId;
        refDAF.CreatedDate = expectedCreatedDate;
        refDAF.ModifiedDate = expectedModifiedDate;
        refDAF.IsActive = expectedIsActive;
        refDAF.CreatedBy = expectedCreatedBy;
        refDAF.ModifiedBy = expectedModifiedBy;
        refDAF.Director = expectedDirector;
        refDAF.Film = expectedFilm;

        // Assert
        Assert.Equal(expectedId, refDAF.Id);
        Assert.Equal(expectedDirectorId, refDAF.DirectorId);
        Assert.Equal(expectedFilmId, refDAF.FilmId);
        Assert.Equal(expectedCreatedDate, refDAF.CreatedDate);
        Assert.Equal(expectedModifiedDate, refDAF.ModifiedDate);
        Assert.Equal(expectedIsActive, refDAF.IsActive);
        Assert.Equal(expectedCreatedBy, refDAF.CreatedBy);
        Assert.Equal(expectedModifiedBy, refDAF.ModifiedBy);
        Assert.Equal(expectedDirector, refDAF.Director);
        Assert.Equal(expectedFilm, refDAF.Film);
    }
}
