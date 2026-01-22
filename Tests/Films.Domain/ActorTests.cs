using Xunit;
using Films.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Films.Domain.Tests;

public class ActorTests
{
    [Fact]
    public void Actor_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var actor = new Actor();

        // Assert
        Assert.Equal(0, actor.Id);
        Assert.Equal(string.Empty, actor.Name);
        Assert.Null(actor.Surname);
        Assert.Null(actor.SexId);
        Assert.Null(actor.Country);
        Assert.Equal(default(DateTime), actor.CreatedDate);
        Assert.Null(actor.ModifiedDate);
        Assert.False(actor.IsActive);
        Assert.Equal(string.Empty, actor.CreatedBy);
        Assert.Null(actor.ModifiedBy);
        Assert.Null(actor.Sex);
        Assert.NotNull(actor.RefAFs);
        Assert.Empty(actor.RefAFs);
    }

    [Fact]
    public void Actor_Id_CanBeSetAndRetrieved()
    {
        // Arrange
        var actor = new Actor();
        var expectedId = 456;

        // Act
        actor.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, actor.Id);
    }

    [Fact]
    public void Actor_Name_CanBeSetAndRetrieved()
    {
        // Arrange
        var actor = new Actor();
        var expectedName = "Keanu Reeves";

        // Act
        actor.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, actor.Name);
    }

    [Fact]
    public void Actor_Name_CanBeSetToEmptyString()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, actor.Name);
    }

    [Fact]
    public void Actor_Surname_CanBeSetAndRetrieved()
    {
        // Arrange
        var actor = new Actor();
        var expectedSurname = "Reeves";

        // Act
        actor.Surname = expectedSurname;

        // Assert
        Assert.Equal(expectedSurname, actor.Surname);
    }

    [Fact]
    public void Actor_Surname_CanBeNull()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.Surname = null;

        // Assert
        Assert.Null(actor.Surname);
    }

    [Fact]
    public void Actor_SexId_CanBeSetAndRetrieved()
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
    public void Actor_SexId_CanBeNull()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.SexId = null;

        // Assert
        Assert.Null(actor.SexId);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Actor_SexId_AcceptsVariousValues(int sexId)
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.SexId = sexId;

        // Assert
        Assert.Equal(sexId, actor.SexId);
    }

    [Fact]
    public void Actor_Country_CanBeSetAndRetrieved()
    {
        // Arrange
        var actor = new Actor();
        var expectedCountry = "United States";

        // Act
        actor.Country = expectedCountry;

        // Assert
        Assert.Equal(expectedCountry, actor.Country);
    }

    [Fact]
    public void Actor_Country_CanBeNull()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.Country = null;

        // Assert
        Assert.Null(actor.Country);
    }

    [Fact]
    public void Actor_CreatedDate_CanBeSetAndRetrieved()
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
    public void Actor_ModifiedDate_CanBeSetAndRetrieved()
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
    public void Actor_ModifiedDate_CanBeNull()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.ModifiedDate = null;

        // Assert
        Assert.Null(actor.ModifiedDate);
    }

    [Fact]
    public void Actor_IsActive_CanBeSetToTrue()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.IsActive = true;

        // Assert
        Assert.True(actor.IsActive);
    }

    [Fact]
    public void Actor_IsActive_CanBeSetToFalse()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.IsActive = false;

        // Assert
        Assert.False(actor.IsActive);
    }

    [Fact]
    public void Actor_CreatedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var actor = new Actor();
        var expectedCreatedBy = "admin";

        // Act
        actor.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, actor.CreatedBy);
    }

    [Fact]
    public void Actor_ModifiedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var actor = new Actor();
        var expectedModifiedBy = "user123";

        // Act
        actor.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, actor.ModifiedBy);
    }

    [Fact]
    public void Actor_ModifiedBy_CanBeNull()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.ModifiedBy = null;

        // Assert
        Assert.Null(actor.ModifiedBy);
    }

    [Fact]
    public void Actor_Sex_CanBeSetAndRetrieved()
    {
        // Arrange
        var actor = new Actor();
        var expectedSex = new Sex();

        // Act
        actor.Sex = expectedSex;

        // Assert
        Assert.Equal(expectedSex, actor.Sex);
    }

    [Fact]
    public void Actor_Sex_CanBeNull()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.Sex = null;

        // Assert
        Assert.Null(actor.Sex);
    }

    [Fact]
    public void Actor_RefAFs_CanBePopulated()
    {
        // Arrange
        var actor = new Actor();
        var refAF1 = new RefAF();
        var refAF2 = new RefAF();

        // Act
        actor.RefAFs = new List<RefAF> { refAF1, refAF2 };

        // Assert
        Assert.Equal(2, actor.RefAFs.Count);
        Assert.Contains(refAF1, actor.RefAFs);
        Assert.Contains(refAF2, actor.RefAFs);
    }

    [Fact]
    public void Actor_AllProperties_CanBeSetTogether()
    {
        // Arrange
        var actor = new Actor();
        var expectedId = 789;
        var expectedName = "Tom";
        var expectedSurname = "Hanks";
        var expectedSexId = 1;
        var expectedCountry = "USA";
        var expectedCreatedDate = DateTime.Now;
        var expectedModifiedDate = DateTime.Now.AddDays(5);
        var expectedIsActive = true;
        var expectedCreatedBy = "admin";
        var expectedModifiedBy = "moderator";
        var expectedSex = new Sex { Id = 1 };

        // Act
        actor.Id = expectedId;
        actor.Name = expectedName;
        actor.Surname = expectedSurname;
        actor.SexId = expectedSexId;
        actor.Country = expectedCountry;
        actor.CreatedDate = expectedCreatedDate;
        actor.ModifiedDate = expectedModifiedDate;
        actor.IsActive = expectedIsActive;
        actor.CreatedBy = expectedCreatedBy;
        actor.ModifiedBy = expectedModifiedBy;
        actor.Sex = expectedSex;

        // Assert
        Assert.Equal(expectedId, actor.Id);
        Assert.Equal(expectedName, actor.Name);
        Assert.Equal(expectedSurname, actor.Surname);
        Assert.Equal(expectedSexId, actor.SexId);
        Assert.Equal(expectedCountry, actor.Country);
        Assert.Equal(expectedCreatedDate, actor.CreatedDate);
        Assert.Equal(expectedModifiedDate, actor.ModifiedDate);
        Assert.Equal(expectedIsActive, actor.IsActive);
        Assert.Equal(expectedCreatedBy, actor.CreatedBy);
        Assert.Equal(expectedModifiedBy, actor.ModifiedBy);
        Assert.Equal(expectedSex, actor.Sex);
    }
}
