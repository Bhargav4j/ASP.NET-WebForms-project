using Xunit;
using Films.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Films.Domain.Tests;

public class FilmTests
{
    [Fact]
    public void Film_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var film = new Film();

        // Assert
        Assert.Equal(0, film.Id);
        Assert.Equal(string.Empty, film.Name);
        Assert.Null(film.Description);
        Assert.Null(film.Year);
        Assert.Null(film.Genre);
        Assert.Equal(default(DateTime), film.CreatedDate);
        Assert.Null(film.ModifiedDate);
        Assert.False(film.IsActive);
        Assert.Equal(string.Empty, film.CreatedBy);
        Assert.Null(film.ModifiedBy);
        Assert.NotNull(film.RefAFs);
        Assert.NotNull(film.RefDAFs);
        Assert.Empty(film.RefAFs);
        Assert.Empty(film.RefDAFs);
    }

    [Fact]
    public void Film_Id_CanBeSetAndRetrieved()
    {
        // Arrange
        var film = new Film();
        var expectedId = 123;

        // Act
        film.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, film.Id);
    }

    [Fact]
    public void Film_Name_CanBeSetAndRetrieved()
    {
        // Arrange
        var film = new Film();
        var expectedName = "The Matrix";

        // Act
        film.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, film.Name);
    }

    [Fact]
    public void Film_Name_CanBeSetToEmptyString()
    {
        // Arrange
        var film = new Film();

        // Act
        film.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, film.Name);
    }

    [Fact]
    public void Film_Description_CanBeSetAndRetrieved()
    {
        // Arrange
        var film = new Film();
        var expectedDescription = "A computer hacker learns from mysterious rebels about the true nature of his reality";

        // Act
        film.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, film.Description);
    }

    [Fact]
    public void Film_Description_CanBeNull()
    {
        // Arrange
        var film = new Film();

        // Act
        film.Description = null;

        // Assert
        Assert.Null(film.Description);
    }

    [Fact]
    public void Film_Year_CanBeSetAndRetrieved()
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
    public void Film_Year_CanBeNull()
    {
        // Arrange
        var film = new Film();

        // Act
        film.Year = null;

        // Assert
        Assert.Null(film.Year);
    }

    [Theory]
    [InlineData(1900)]
    [InlineData(2000)]
    [InlineData(2024)]
    [InlineData(2050)]
    public void Film_Year_AcceptsVariousYears(int year)
    {
        // Arrange
        var film = new Film();

        // Act
        film.Year = year;

        // Assert
        Assert.Equal(year, film.Year);
    }

    [Fact]
    public void Film_Genre_CanBeSetAndRetrieved()
    {
        // Arrange
        var film = new Film();
        var expectedGenre = "Science Fiction";

        // Act
        film.Genre = expectedGenre;

        // Assert
        Assert.Equal(expectedGenre, film.Genre);
    }

    [Fact]
    public void Film_Genre_CanBeNull()
    {
        // Arrange
        var film = new Film();

        // Act
        film.Genre = null;

        // Assert
        Assert.Null(film.Genre);
    }

    [Fact]
    public void Film_CreatedDate_CanBeSetAndRetrieved()
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
    public void Film_ModifiedDate_CanBeSetAndRetrieved()
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
    public void Film_ModifiedDate_CanBeNull()
    {
        // Arrange
        var film = new Film();

        // Act
        film.ModifiedDate = null;

        // Assert
        Assert.Null(film.ModifiedDate);
    }

    [Fact]
    public void Film_IsActive_CanBeSetToTrue()
    {
        // Arrange
        var film = new Film();

        // Act
        film.IsActive = true;

        // Assert
        Assert.True(film.IsActive);
    }

    [Fact]
    public void Film_IsActive_CanBeSetToFalse()
    {
        // Arrange
        var film = new Film();

        // Act
        film.IsActive = false;

        // Assert
        Assert.False(film.IsActive);
    }

    [Fact]
    public void Film_CreatedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var film = new Film();
        var expectedCreatedBy = "admin";

        // Act
        film.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, film.CreatedBy);
    }

    [Fact]
    public void Film_ModifiedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var film = new Film();
        var expectedModifiedBy = "user123";

        // Act
        film.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, film.ModifiedBy);
    }

    [Fact]
    public void Film_ModifiedBy_CanBeNull()
    {
        // Arrange
        var film = new Film();

        // Act
        film.ModifiedBy = null;

        // Assert
        Assert.Null(film.ModifiedBy);
    }

    [Fact]
    public void Film_RefAFs_CanBePopulated()
    {
        // Arrange
        var film = new Film();
        var refAF1 = new RefAF();
        var refAF2 = new RefAF();

        // Act
        film.RefAFs = new List<RefAF> { refAF1, refAF2 };

        // Assert
        Assert.Equal(2, film.RefAFs.Count);
        Assert.Contains(refAF1, film.RefAFs);
        Assert.Contains(refAF2, film.RefAFs);
    }

    [Fact]
    public void Film_RefDAFs_CanBePopulated()
    {
        // Arrange
        var film = new Film();
        var refDAF1 = new RefDAF();
        var refDAF2 = new RefDAF();

        // Act
        film.RefDAFs = new List<RefDAF> { refDAF1, refDAF2 };

        // Assert
        Assert.Equal(2, film.RefDAFs.Count);
        Assert.Contains(refDAF1, film.RefDAFs);
        Assert.Contains(refDAF2, film.RefDAFs);
    }

    [Fact]
    public void Film_AllProperties_CanBeSetTogether()
    {
        // Arrange
        var film = new Film();
        var expectedId = 100;
        var expectedName = "Inception";
        var expectedDescription = "A thief who steals corporate secrets";
        var expectedYear = 2010;
        var expectedGenre = "Action/Sci-Fi";
        var expectedCreatedDate = DateTime.Now;
        var expectedModifiedDate = DateTime.Now.AddDays(1);
        var expectedIsActive = true;
        var expectedCreatedBy = "admin";
        var expectedModifiedBy = "moderator";

        // Act
        film.Id = expectedId;
        film.Name = expectedName;
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
        Assert.Equal(expectedName, film.Name);
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
