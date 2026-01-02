using System;
using Xunit;
using Films.Domain.Entities;

namespace Films.Tests.Domain.Entities;

public class RefAFTests
{
    [Fact]
    public void RefAF_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var refAF = new RefAF();

        // Assert
        Assert.Equal(0, refAF.Id);
        Assert.Equal(0, refAF.ActorId);
        Assert.Equal(0, refAF.FilmId);
        Assert.False(refAF.IsActive);
        Assert.Equal(string.Empty, refAF.CreatedBy);
    }

    [Fact]
    public void RefAF_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var refAF = new RefAF();
        var testDate = DateTime.UtcNow;

        // Act
        refAF.Id = 1;
        refAF.ActorId = 10;
        refAF.FilmId = 20;
        refAF.CreatedDate = testDate;
        refAF.IsActive = true;
        refAF.CreatedBy = "TestUser";

        // Assert
        Assert.Equal(1, refAF.Id);
        Assert.Equal(10, refAF.ActorId);
        Assert.Equal(20, refAF.FilmId);
        Assert.Equal(testDate, refAF.CreatedDate);
        Assert.True(refAF.IsActive);
        Assert.Equal("TestUser", refAF.CreatedBy);
    }

    [Fact]
    public void RefAF_Actor_ShouldAcceptActorInstance()
    {
        // Arrange
        var refAF = new RefAF();
        var actor = new Actor { Id = 10, FirstName = "John", LastName = "Doe" };

        // Act
        refAF.Actor = actor;

        // Assert
        Assert.NotNull(refAF.Actor);
        Assert.Equal(10, refAF.Actor.Id);
    }

    [Fact]
    public void RefAF_Film_ShouldAcceptFilmInstance()
    {
        // Arrange
        var refAF = new RefAF();
        var film = new Film { Id = 20, Name = "Test Film" };

        // Act
        refAF.Film = film;

        // Assert
        Assert.NotNull(refAF.Film);
        Assert.Equal(20, refAF.Film.Id);
    }

    [Fact]
    public void RefAF_CreatedBy_ShouldAcceptEmptyString()
    {
        // Arrange
        var refAF = new RefAF();

        // Act
        refAF.CreatedBy = string.Empty;

        // Assert
        Assert.Equal(string.Empty, refAF.CreatedBy);
    }
}
