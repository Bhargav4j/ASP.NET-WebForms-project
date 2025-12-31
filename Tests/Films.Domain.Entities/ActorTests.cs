using Xunit;
using Films.Domain.Entities;
using System;

namespace Films.Domain.Entities.Tests;

public class ActorTests
{
    [Fact]
    public void Actor_Constructor_InitializesCollections()
    {
        // Arrange & Act
        var actor = new Actor();

        // Assert
        Assert.NotNull(actor.ActorFilms);
        Assert.Empty(actor.ActorFilms);
    }

    [Fact]
    public void Actor_SetId_ReturnsCorrectValue()
    {
        // Arrange
        var actor = new Actor();
        var expectedId = 1;

        // Act
        actor.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, actor.Id);
    }

    [Fact]
    public void Actor_SetName_ReturnsCorrectValue()
    {
        // Arrange
        var actor = new Actor();
        var expectedName = "Tom Hanks";

        // Act
        actor.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, actor.Name);
    }

    [Fact]
    public void Actor_DefaultName_IsEmptyString()
    {
        // Arrange & Act
        var actor = new Actor();

        // Assert
        Assert.Equal(string.Empty, actor.Name);
    }

    [Fact]
    public void Actor_SetDescription_ReturnsCorrectValue()
    {
        // Arrange
        var actor = new Actor();
        var expectedDescription = "Famous actor";

        // Act
        actor.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, actor.Description);
    }

    [Fact]
    public void Actor_SetDescription_AllowsNull()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.Description = null;

        // Assert
        Assert.Null(actor.Description);
    }

    [Fact]
    public void Actor_SetSexId_ReturnsCorrectValue()
    {
        // Arrange
        var actor = new Actor();
        var expectedSexId = 1;

        // Act
        actor.SexId = expectedSexId;

        // Assert
        Assert.Equal(expectedSexId, actor.SexId);
    }

    [Fact]
    public void Actor_SetSexId_AllowsNull()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.SexId = null;

        // Assert
        Assert.Null(actor.SexId);
    }

    [Fact]
    public void Actor_SetSex_ReturnsCorrectValue()
    {
        // Arrange
        var actor = new Actor();
        var expectedSex = new Sex { Id = 1, Name = "Male" };

        // Act
        actor.Sex = expectedSex;

        // Assert
        Assert.Equal(expectedSex, actor.Sex);
    }

    [Fact]
    public void Actor_SetIsActive_ReturnsCorrectValue()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.IsActive = true;

        // Assert
        Assert.True(actor.IsActive);
    }

    [Fact]
    public void Actor_SetCreatedDate_ReturnsCorrectValue()
    {
        // Arrange
        var actor = new Actor();
        var expectedDate = DateTime.UtcNow;

        // Act
        actor.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, actor.CreatedDate);
    }

    [Fact]
    public void Actor_SetModifiedDate_ReturnsCorrectValue()
    {
        // Arrange
        var actor = new Actor();
        var expectedDate = DateTime.UtcNow;

        // Act
        actor.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, actor.ModifiedDate);
    }

    [Fact]
    public void Actor_SetCreatedBy_ReturnsCorrectValue()
    {
        // Arrange
        var actor = new Actor();
        var expectedCreatedBy = "Admin";

        // Act
        actor.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, actor.CreatedBy);
    }

    [Fact]
    public void Actor_DefaultCreatedBy_IsEmptyString()
    {
        // Arrange & Act
        var actor = new Actor();

        // Assert
        Assert.Equal(string.Empty, actor.CreatedBy);
    }

    [Fact]
    public void Actor_AddActorFilm_AddsToCollection()
    {
        // Arrange
        var actor = new Actor();
        var actorFilm = new RefAF { Id = 1, ActorId = 1, FilmId = 1 };

        // Act
        actor.ActorFilms.Add(actorFilm);

        // Assert
        Assert.Single(actor.ActorFilms);
        Assert.Contains(actorFilm, actor.ActorFilms);
    }

    [Fact]
    public void Actor_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var actor = new Actor();
        var expectedId = 10;
        var expectedName = "Brad Pitt";
        var expectedDescription = "Award-winning actor";
        var expectedSexId = 1;
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedModifiedDate = DateTime.UtcNow.AddDays(1);
        var expectedIsActive = true;
        var expectedCreatedBy = "User1";
        var expectedModifiedBy = "User2";

        // Act
        actor.Id = expectedId;
        actor.Name = expectedName;
        actor.Description = expectedDescription;
        actor.SexId = expectedSexId;
        actor.CreatedDate = expectedCreatedDate;
        actor.ModifiedDate = expectedModifiedDate;
        actor.IsActive = expectedIsActive;
        actor.CreatedBy = expectedCreatedBy;
        actor.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, actor.Id);
        Assert.Equal(expectedName, actor.Name);
        Assert.Equal(expectedDescription, actor.Description);
        Assert.Equal(expectedSexId, actor.SexId);
        Assert.Equal(expectedCreatedDate, actor.CreatedDate);
        Assert.Equal(expectedModifiedDate, actor.ModifiedDate);
        Assert.Equal(expectedIsActive, actor.IsActive);
        Assert.Equal(expectedCreatedBy, actor.CreatedBy);
        Assert.Equal(expectedModifiedBy, actor.ModifiedBy);
    }
}
