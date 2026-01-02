using System;
using System.Collections.Generic;
using Xunit;
using Films.Domain.Entities;

namespace Films.Tests.Domain.Entities;

public class FilmTests
{
    [Fact]
    public void Film_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var film = new Film();

        // Assert
        Assert.Equal(0, film.Id);
        Assert.Equal(string.Empty, film.Name);
        Assert.Null(film.Description);
        Assert.False(film.IsActive);
        Assert.Equal(string.Empty, film.CreatedBy);
        Assert.Null(film.ModifiedBy);
        Assert.NotNull(film.RefAFs);
        Assert.NotNull(film.RefDAFs);
    }

    [Fact]
    public void Film_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var film = new Film();
        var testDate = DateTime.UtcNow;

        // Act
        film.Id = 1;
        film.Name = "Test Film";
        film.Description = "Test Description";
        film.CreatedDate = testDate;
        film.ModifiedDate = testDate;
        film.IsActive = true;
        film.CreatedBy = "TestUser";
        film.ModifiedBy = "ModifiedUser";

        // Assert
        Assert.Equal(1, film.Id);
        Assert.Equal("Test Film", film.Name);
        Assert.Equal("Test Description", film.Description);
        Assert.Equal(testDate, film.CreatedDate);
        Assert.Equal(testDate, film.ModifiedDate);
        Assert.True(film.IsActive);
        Assert.Equal("TestUser", film.CreatedBy);
        Assert.Equal("ModifiedUser", film.ModifiedBy);
    }

    [Fact]
    public void Film_RefAFs_ShouldInitializeAsEmptyCollection()
    {
        // Arrange & Act
        var film = new Film();

        // Assert
        Assert.NotNull(film.RefAFs);
        Assert.Empty(film.RefAFs);
        Assert.IsAssignableFrom<ICollection<RefAF>>(film.RefAFs);
    }

    [Fact]
    public void Film_RefDAFs_ShouldInitializeAsEmptyCollection()
    {
        // Arrange & Act
        var film = new Film();

        // Assert
        Assert.NotNull(film.RefDAFs);
        Assert.Empty(film.RefDAFs);
        Assert.IsAssignableFrom<ICollection<RefDAF>>(film.RefDAFs);
    }

    [Fact]
    public void Film_Name_ShouldAcceptEmptyString()
    {
        // Arrange
        var film = new Film();

        // Act
        film.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, film.Name);
    }

    [Fact]
    public void Film_Description_ShouldAcceptNull()
    {
        // Arrange
        var film = new Film();

        // Act
        film.Description = null;

        // Assert
        Assert.Null(film.Description);
    }

    [Fact]
    public void Film_ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var film = new Film();

        // Act
        film.ModifiedDate = null;

        // Assert
        Assert.Null(film.ModifiedDate);
    }
}
