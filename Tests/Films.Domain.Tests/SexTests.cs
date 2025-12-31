using Xunit;
using Films.Domain.Entities;
using System;

namespace Films.Domain.Entities.Tests;

public class SexTests
{
    [Fact]
    public void Sex_Constructor_InitializesCollections()
    {
        // Arrange & Act
        var sex = new Sex();

        // Assert
        Assert.NotNull(sex.Actors);
        Assert.Empty(sex.Actors);
    }

    [Fact]
    public void Sex_SetId_ReturnsCorrectValue()
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
    public void Sex_SetName_ReturnsCorrectValue()
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
    public void Sex_DefaultName_IsEmptyString()
    {
        // Arrange & Act
        var sex = new Sex();

        // Assert
        Assert.Equal(string.Empty, sex.Name);
    }

    [Fact]
    public void Sex_SetDescription_ReturnsCorrectValue()
    {
        // Arrange
        var sex = new Sex();
        var expectedDescription = "Male gender";

        // Act
        sex.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, sex.Description);
    }

    [Fact]
    public void Sex_SetDescription_AllowsNull()
    {
        // Arrange
        var sex = new Sex();

        // Act
        sex.Description = null;

        // Assert
        Assert.Null(sex.Description);
    }

    [Fact]
    public void Sex_SetIsActive_ReturnsCorrectValue()
    {
        // Arrange
        var sex = new Sex();

        // Act
        sex.IsActive = true;

        // Assert
        Assert.True(sex.IsActive);
    }

    [Fact]
    public void Sex_SetCreatedDate_ReturnsCorrectValue()
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
    public void Sex_SetModifiedDate_ReturnsCorrectValue()
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
    public void Sex_SetCreatedBy_ReturnsCorrectValue()
    {
        // Arrange
        var sex = new Sex();
        var expectedCreatedBy = "Admin";

        // Act
        sex.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, sex.CreatedBy);
    }

    [Fact]
    public void Sex_DefaultCreatedBy_IsEmptyString()
    {
        // Arrange & Act
        var sex = new Sex();

        // Assert
        Assert.Equal(string.Empty, sex.CreatedBy);
    }

    [Fact]
    public void Sex_AddActor_AddsToCollection()
    {
        // Arrange
        var sex = new Sex();
        var actor = new Actor { Id = 1, Name = "John Doe" };

        // Act
        sex.Actors.Add(actor);

        // Assert
        Assert.Single(sex.Actors);
        Assert.Contains(actor, sex.Actors);
    }

    [Fact]
    public void Sex_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var sex = new Sex();
        var expectedId = 2;
        var expectedName = "Female";
        var expectedDescription = "Female gender";
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedModifiedDate = DateTime.UtcNow.AddDays(1);
        var expectedIsActive = true;
        var expectedCreatedBy = "System";
        var expectedModifiedBy = "Admin";

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
