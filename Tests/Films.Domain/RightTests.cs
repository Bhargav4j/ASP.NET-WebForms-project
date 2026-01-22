using Xunit;
using Films.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Films.Domain.Tests;

public class RightTests
{
    [Fact]
    public void Right_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var right = new Right();

        // Assert
        Assert.Equal(0, right.Id);
        Assert.Equal(string.Empty, right.Name);
        Assert.Null(right.Description);
        Assert.Equal(default(DateTime), right.CreatedDate);
        Assert.Null(right.ModifiedDate);
        Assert.False(right.IsActive);
        Assert.Equal(string.Empty, right.CreatedBy);
        Assert.Null(right.ModifiedBy);
        Assert.NotNull(right.UserRights);
        Assert.Empty(right.UserRights);
    }

    [Fact]
    public void Right_Id_CanBeSetAndRetrieved()
    {
        // Arrange
        var right = new Right();
        var expectedId = 25;

        // Act
        right.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, right.Id);
    }

    [Fact]
    public void Right_Name_CanBeSetAndRetrieved()
    {
        // Arrange
        var right = new Right();
        var expectedName = "ViewFilms";

        // Act
        right.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, right.Name);
    }

    [Fact]
    public void Right_Name_CanBeSetToEmptyString()
    {
        // Arrange
        var right = new Right();

        // Act
        right.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, right.Name);
    }

    [Theory]
    [InlineData("Read")]
    [InlineData("Write")]
    [InlineData("Delete")]
    [InlineData("Admin")]
    public void Right_Name_AcceptsVariousValues(string name)
    {
        // Arrange
        var right = new Right();

        // Act
        right.Name = name;

        // Assert
        Assert.Equal(name, right.Name);
    }

    [Fact]
    public void Right_Description_CanBeSetAndRetrieved()
    {
        // Arrange
        var right = new Right();
        var expectedDescription = "Allows viewing all films";

        // Act
        right.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, right.Description);
    }

    [Fact]
    public void Right_Description_CanBeNull()
    {
        // Arrange
        var right = new Right();

        // Act
        right.Description = null;

        // Assert
        Assert.Null(right.Description);
    }

    [Fact]
    public void Right_CreatedDate_CanBeSetAndRetrieved()
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
    public void Right_ModifiedDate_CanBeSetAndRetrieved()
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
    public void Right_ModifiedDate_CanBeNull()
    {
        // Arrange
        var right = new Right();

        // Act
        right.ModifiedDate = null;

        // Assert
        Assert.Null(right.ModifiedDate);
    }

    [Fact]
    public void Right_IsActive_CanBeSetToTrue()
    {
        // Arrange
        var right = new Right();

        // Act
        right.IsActive = true;

        // Assert
        Assert.True(right.IsActive);
    }

    [Fact]
    public void Right_IsActive_CanBeSetToFalse()
    {
        // Arrange
        var right = new Right();

        // Act
        right.IsActive = false;

        // Assert
        Assert.False(right.IsActive);
    }

    [Fact]
    public void Right_CreatedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var right = new Right();
        var expectedCreatedBy = "system";

        // Act
        right.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, right.CreatedBy);
    }

    [Fact]
    public void Right_ModifiedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var right = new Right();
        var expectedModifiedBy = "admin";

        // Act
        right.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, right.ModifiedBy);
    }

    [Fact]
    public void Right_ModifiedBy_CanBeNull()
    {
        // Arrange
        var right = new Right();

        // Act
        right.ModifiedBy = null;

        // Assert
        Assert.Null(right.ModifiedBy);
    }

    [Fact]
    public void Right_UserRights_CanBePopulated()
    {
        // Arrange
        var right = new Right();
        var userRight1 = new UserRight();
        var userRight2 = new UserRight();

        // Act
        right.UserRights = new List<UserRight> { userRight1, userRight2 };

        // Assert
        Assert.Equal(2, right.UserRights.Count);
        Assert.Contains(userRight1, right.UserRights);
        Assert.Contains(userRight2, right.UserRights);
    }

    [Fact]
    public void Right_AllProperties_CanBeSetTogether()
    {
        // Arrange
        var right = new Right();
        var expectedId = 50;
        var expectedName = "EditFilms";
        var expectedDescription = "Permission to edit film entries";
        var expectedCreatedDate = DateTime.Now;
        var expectedModifiedDate = DateTime.Now.AddDays(7);
        var expectedIsActive = true;
        var expectedCreatedBy = "root";
        var expectedModifiedBy = "superadmin";

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
