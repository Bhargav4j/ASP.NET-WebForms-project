using System;
using System.Collections.Generic;
using Xunit;
using Films.Domain.Entities;

namespace Films.Tests.Domain.Entities;

public class ActorTests
{
    [Fact]
    public void Actor_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var actor = new Actor();

        // Assert
        Assert.Equal(0, actor.Id);
        Assert.Equal(string.Empty, actor.FirstName);
        Assert.Equal(string.Empty, actor.LastName);
        Assert.Null(actor.SexId);
        Assert.False(actor.IsActive);
        Assert.Equal(string.Empty, actor.CreatedBy);
        Assert.Null(actor.ModifiedBy);
        Assert.NotNull(actor.RefAFs);
    }

    [Fact]
    public void Actor_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var actor = new Actor();
        var testDate = DateTime.UtcNow;

        // Act
        actor.Id = 1;
        actor.FirstName = "John";
        actor.LastName = "Doe";
        actor.SexId = 1;
        actor.CreatedDate = testDate;
        actor.ModifiedDate = testDate;
        actor.IsActive = true;
        actor.CreatedBy = "TestUser";
        actor.ModifiedBy = "ModifiedUser";

        // Assert
        Assert.Equal(1, actor.Id);
        Assert.Equal("John", actor.FirstName);
        Assert.Equal("Doe", actor.LastName);
        Assert.Equal(1, actor.SexId);
        Assert.Equal(testDate, actor.CreatedDate);
        Assert.Equal(testDate, actor.ModifiedDate);
        Assert.True(actor.IsActive);
        Assert.Equal("TestUser", actor.CreatedBy);
        Assert.Equal("ModifiedUser", actor.ModifiedBy);
    }

    [Fact]
    public void Actor_RefAFs_ShouldInitializeAsEmptyCollection()
    {
        // Arrange & Act
        var actor = new Actor();

        // Assert
        Assert.NotNull(actor.RefAFs);
        Assert.Empty(actor.RefAFs);
        Assert.IsAssignableFrom<ICollection<RefAF>>(actor.RefAFs);
    }

    [Fact]
    public void Actor_Sex_ShouldAcceptNull()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.Sex = null;

        // Assert
        Assert.Null(actor.Sex);
    }

    [Fact]
    public void Actor_SexId_ShouldAcceptNull()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.SexId = null;

        // Assert
        Assert.Null(actor.SexId);
    }

    [Fact]
    public void Actor_ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.ModifiedDate = null;

        // Assert
        Assert.Null(actor.ModifiedDate);
    }

    [Fact]
    public void Actor_FirstName_ShouldAcceptEmptyString()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.FirstName = string.Empty;

        // Assert
        Assert.Equal(string.Empty, actor.FirstName);
    }

    [Fact]
    public void Actor_LastName_ShouldAcceptEmptyString()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.LastName = string.Empty;

        // Assert
        Assert.Equal(string.Empty, actor.LastName);
    }
}
