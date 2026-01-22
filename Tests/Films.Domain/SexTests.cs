using Xunit;
using Films.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Films.Domain.Tests;

public class SexTests
{
    [Fact]
    public void Sex_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var sex = new Sex();

        // Assert
        Assert.Equal(0, sex.Id);
        Assert.Equal(string.Empty, sex.Name);
        Assert.Null(sex.Description);
        Assert.Equal(default(DateTime), sex.CreatedDate);
        Assert.Null(sex.ModifiedDate);
        Assert.False(sex.IsActive);
        Assert.Equal(string.Empty, sex.CreatedBy);
        Assert.Null(sex.ModifiedBy);
        Assert.NotNull(sex.Actors);
        Assert.NotNull(sex.Directors);
        Assert.Empty(sex.Actors);
        Assert.Empty(sex.Directors);
    }

    [Fact]
    public void Sex_Id_CanBeSetAndRetrieved()
    {
        // Arrange
        var sex = new Sex();
        var expectedId = 1;

        // Act
        sex.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, sex.Id);
    }

    [Fact]
    public void Sex_Name_CanBeSetAndRetrieved()
    {
        // Arrange
        var sex = new Sex();
        var expectedName = "Male";

        // Act
        sex.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, sex.Name);
    }

    [Fact]
    public void Sex_Name_CanBeSetToEmptyString()
    {
        // Arrange
        var sex = new Sex();

        // Act
        sex.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, sex.Name);
    }

    [Theory]
    [InlineData("Male")]
    [InlineData("Female")]
    [InlineData("Non-binary")]
    [InlineData("Other")]
    public void Sex_Name_AcceptsVariousValues(string name)
    {
        // Arrange
        var sex = new Sex();

        // Act
        sex.Name = name;

        // Assert
        Assert.Equal(name, sex.Name);
    }

    [Fact]
    public void Sex_Description_CanBeSetAndRetrieved()
    {
        // Arrange
        var sex = new Sex();
        var expectedDescription = "Biological male sex";

        // Act
        sex.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, sex.Description);
    }

    [Fact]
    public void Sex_Description_CanBeNull()
    {
        // Arrange
        var sex = new Sex();

        // Act
        sex.Description = null;

        // Assert
        Assert.Null(sex.Description);
    }

    [Fact]
    public void Sex_CreatedDate_CanBeSetAndRetrieved()
    {
        // Arrange
        var sex = new Sex();
        var expectedDate = DateTime.UtcNow;

        // Act
        sex.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, sex.CreatedDate);
    }

    [Fact]
    public void Sex_ModifiedDate_CanBeSetAndRetrieved()
    {
        // Arrange
        var sex = new Sex();
        var expectedDate = DateTime.UtcNow;

        // Act
        sex.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, sex.ModifiedDate);
    }

    [Fact]
    public void Sex_ModifiedDate_CanBeNull()
    {
        // Arrange
        var sex = new Sex();

        // Act
        sex.ModifiedDate = null;

        // Assert
        Assert.Null(sex.ModifiedDate);
    }

    [Fact]
    public void Sex_IsActive_CanBeSetToTrue()
    {
        // Arrange
        var sex = new Sex();

        // Act
        sex.IsActive = true;

        // Assert
        Assert.True(sex.IsActive);
    }

    [Fact]
    public void Sex_IsActive_CanBeSetToFalse()
    {
        // Arrange
        var sex = new Sex();

        // Act
        sex.IsActive = false;

        // Assert
        Assert.False(sex.IsActive);
    }

    [Fact]
    public void Sex_CreatedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var sex = new Sex();
        var expectedCreatedBy = "admin";

        // Act
        sex.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, sex.CreatedBy);
    }

    [Fact]
    public void Sex_ModifiedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var sex = new Sex();
        var expectedModifiedBy = "user123";

        // Act
        sex.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, sex.ModifiedBy);
    }

    [Fact]
    public void Sex_ModifiedBy_CanBeNull()
    {
        // Arrange
        var sex = new Sex();

        // Act
        sex.ModifiedBy = null;

        // Assert
        Assert.Null(sex.ModifiedBy);
    }

    [Fact]
    public void Sex_Actors_CanBePopulated()
    {
        // Arrange
        var sex = new Sex();
        var actor1 = new Actor { Id = 1, Name = "Actor1" };
        var actor2 = new Actor { Id = 2, Name = "Actor2" };

        // Act
        sex.Actors = new List<Actor> { actor1, actor2 };

        // Assert
        Assert.Equal(2, sex.Actors.Count);
        Assert.Contains(actor1, sex.Actors);
        Assert.Contains(actor2, sex.Actors);
    }

    [Fact]
    public void Sex_Directors_CanBePopulated()
    {
        // Arrange
        var sex = new Sex();
        var director1 = new Director { Id = 1, Name = "Director1" };
        var director2 = new Director { Id = 2, Name = "Director2" };

        // Act
        sex.Directors = new List<Director> { director1, director2 };

        // Assert
        Assert.Equal(2, sex.Directors.Count);
        Assert.Contains(director1, sex.Directors);
        Assert.Contains(director2, sex.Directors);
    }

    [Fact]
    public void Sex_AllProperties_CanBeSetTogether()
    {
        // Arrange
        var sex = new Sex();
        var expectedId = 5;
        var expectedName = "Female";
        var expectedDescription = "Biological female sex";
        var expectedCreatedDate = DateTime.Now;
        var expectedModifiedDate = DateTime.Now.AddDays(1);
        var expectedIsActive = true;
        var expectedCreatedBy = "system";
        var expectedModifiedBy = "admin";

        // Act
        sex.Id = expectedId;
        sex.Name = expectedName;
        sex.Description = expectedDescription;
        sex.CreatedDate = expectedCreatedDate;
        sex.ModifiedDate = expectedModifiedDate;
        sex.IsActive = expectedIsActive;
        sex.CreatedBy = expectedCreatedBy;
        sex.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, sex.Id);
        Assert.Equal(expectedName, sex.Name);
        Assert.Equal(expectedDescription, sex.Description);
        Assert.Equal(expectedCreatedDate, sex.CreatedDate);
        Assert.Equal(expectedModifiedDate, sex.ModifiedDate);
        Assert.Equal(expectedIsActive, sex.IsActive);
        Assert.Equal(expectedCreatedBy, sex.CreatedBy);
        Assert.Equal(expectedModifiedBy, sex.ModifiedBy);
    }
}
