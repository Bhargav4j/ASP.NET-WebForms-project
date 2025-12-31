using Xunit;
using Films.Domain.Entities;
using System;

namespace Films.Domain.Entities.Tests;

public class RightTests
{
    [Fact]
    public void Right_Constructor_InitializesCollections()
    {
        // Arrange & Act
        var right = new Right();

        // Assert
        Assert.NotNull(right.UserRights);
        Assert.Empty(right.UserRights);
    }

    [Fact]
    public void Right_SetId_ReturnsCorrectValue()
    {
        // Arrange
        var right = new Right();
        var expectedId = 1;

        // Act
        right.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, right.Id);
    }

    [Fact]
    public void Right_SetName_ReturnsCorrectValue()
    {
        // Arrange
        var right = new Right();
        var expectedName = "Read";

        // Act
        right.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, right.Name);
    }

    [Fact]
    public void Right_DefaultName_IsEmptyString()
    {
        // Arrange & Act
        var right = new Right();

        // Assert
        Assert.Equal(string.Empty, right.Name);
    }

    [Fact]
    public void Right_SetDescription_ReturnsCorrectValue()
    {
        // Arrange
        var right = new Right();
        var expectedDescription = "Read permission";

        // Act
        right.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, right.Description);
    }

    [Fact]
    public void Right_SetDescription_AllowsNull()
    {
        // Arrange
        var right = new Right();

        // Act
        right.Description = null;

        // Assert
        Assert.Null(right.Description);
    }

    [Fact]
    public void Right_SetIsActive_ReturnsCorrectValue()
    {
        // Arrange
        var right = new Right();

        // Act
        right.IsActive = true;

        // Assert
        Assert.True(right.IsActive);
    }

    [Fact]
    public void Right_SetCreatedDate_ReturnsCorrectValue()
    {
        // Arrange
        var right = new Right();
        var expectedDate = DateTime.UtcNow;

        // Act
        right.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, right.CreatedDate);
    }

    [Fact]
    public void Right_SetModifiedDate_ReturnsCorrectValue()
    {
        // Arrange
        var right = new Right();
        var expectedDate = DateTime.UtcNow;

        // Act
        right.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, right.ModifiedDate);
    }

    [Fact]
    public void Right_SetCreatedBy_ReturnsCorrectValue()
    {
        // Arrange
        var right = new Right();
        var expectedCreatedBy = "Admin";

        // Act
        right.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, right.CreatedBy);
    }

    [Fact]
    public void Right_DefaultCreatedBy_IsEmptyString()
    {
        // Arrange & Act
        var right = new Right();

        // Assert
        Assert.Equal(string.Empty, right.CreatedBy);
    }

    [Fact]
    public void Right_AddUserRight_AddsToCollection()
    {
        // Arrange
        var right = new Right();
        var userRight = new UserRight { Id = 1, UserId = 1, RightId = 1 };

        // Act
        right.UserRights.Add(userRight);

        // Assert
        Assert.Single(right.UserRights);
        Assert.Contains(userRight, right.UserRights);
    }

    [Fact]
    public void Right_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var right = new Right();
        var expectedId = 3;
        var expectedName = "Write";
        var expectedDescription = "Write permission";
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedModifiedDate = DateTime.UtcNow.AddDays(1);
        var expectedIsActive = true;
        var expectedCreatedBy = "System";
        var expectedModifiedBy = "Admin";

        // Act
        right.Id = expectedId;
        right.Name = expectedName;
        right.Description = expectedDescription;
        right.CreatedDate = expectedCreatedDate;
        right.ModifiedDate = expectedModifiedDate;
        right.IsActive = expectedIsActive;
        right.CreatedBy = expectedCreatedBy;
        right.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, right.Id);
        Assert.Equal(expectedName, right.Name);
        Assert.Equal(expectedDescription, right.Description);
        Assert.Equal(expectedCreatedDate, right.CreatedDate);
        Assert.Equal(expectedModifiedDate, right.ModifiedDate);
        Assert.Equal(expectedIsActive, right.IsActive);
        Assert.Equal(expectedCreatedBy, right.CreatedBy);
        Assert.Equal(expectedModifiedBy, right.ModifiedBy);
    }
}
