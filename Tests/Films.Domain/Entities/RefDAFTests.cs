using System;
using Xunit;
using Films.Domain.Entities;

namespace Films.Tests.Domain.Entities;

public class RefDAFTests
{
    [Fact]
    public void RefDAF_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var refDAF = new RefDAF();

        // Assert
        Assert.Equal(0, refDAF.Id);
        Assert.Equal(0, refDAF.DirectorId);
        Assert.Equal(0, refDAF.FilmId);
        Assert.False(refDAF.IsActive);
        Assert.Equal(string.Empty, refDAF.CreatedBy);
    }

    [Fact]
    public void RefDAF_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var refDAF = new RefDAF();
        var testDate = DateTime.UtcNow;

        // Act
        refDAF.Id = 1;
        refDAF.DirectorId = 10;
        refDAF.FilmId = 20;
        refDAF.CreatedDate = testDate;
        refDAF.IsActive = true;
        refDAF.CreatedBy = "TestUser";

        // Assert
        Assert.Equal(1, refDAF.Id);
        Assert.Equal(10, refDAF.DirectorId);
        Assert.Equal(20, refDAF.FilmId);
        Assert.Equal(testDate, refDAF.CreatedDate);
        Assert.True(refDAF.IsActive);
        Assert.Equal("TestUser", refDAF.CreatedBy);
    }

    [Fact]
    public void RefDAF_Director_ShouldAcceptDirectorInstance()
    {
        // Arrange
        var refDAF = new RefDAF();
        var director = new Director { Id = 10, FirstName = "Jane", LastName = "Smith" };

        // Act
        refDAF.Director = director;

        // Assert
        Assert.NotNull(refDAF.Director);
        Assert.Equal(10, refDAF.Director.Id);
    }

    [Fact]
    public void RefDAF_Film_ShouldAcceptFilmInstance()
    {
        // Arrange
        var refDAF = new RefDAF();
        var film = new Film { Id = 20, Name = "Test Film" };

        // Act
        refDAF.Film = film;

        // Assert
        Assert.NotNull(refDAF.Film);
        Assert.Equal(20, refDAF.Film.Id);
    }

    [Fact]
    public void RefDAF_CreatedBy_ShouldAcceptEmptyString()
    {
        // Arrange
        var refDAF = new RefDAF();

        // Act
        refDAF.CreatedBy = string.Empty;

        // Assert
        Assert.Equal(string.Empty, refDAF.CreatedBy);
    }
}
