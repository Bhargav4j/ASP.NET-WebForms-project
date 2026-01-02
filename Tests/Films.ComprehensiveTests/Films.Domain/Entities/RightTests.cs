using System;
using System.Collections.Generic;
using Xunit;
using Films.Domain.Entities;

namespace Films.Tests.Domain.Entities;

public class RightTests
{
    [Fact]
    public void Right_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var right = new Right();

        // Assert
        Assert.Equal(0, right.Id);
        Assert.Equal(string.Empty, right.Name);
        Assert.Null(right.Description);
        Assert.False(right.IsActive);
        Assert.Equal(string.Empty, right.CreatedBy);
        Assert.Null(right.ModifiedBy);
        Assert.NotNull(right.UserRights);
    }

    [Fact]
    public void Right_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var right = new Right();
        var testDate = DateTime.UtcNow;

        // Act
        right.Id = 1;
        right.Name = "ViewFilms";
        right.Description = "Permission to view films";
        right.CreatedDate = testDate;
        right.ModifiedDate = testDate;
        right.IsActive = true;
        right.CreatedBy = "TestUser";
        right.ModifiedBy = "ModifiedUser";

        // Assert
        Assert.Equal(1, right.Id);
        Assert.Equal("ViewFilms", right.Name);
        Assert.Equal("Permission to view films", right.Description);
        Assert.Equal(testDate, right.CreatedDate);
        Assert.Equal(testDate, right.ModifiedDate);
        Assert.True(right.IsActive);
        Assert.Equal("TestUser", right.CreatedBy);
        Assert.Equal("ModifiedUser", right.ModifiedBy);
    }

    [Fact]
    public void Right_UserRights_ShouldInitializeAsEmptyCollection()
    {
        // Arrange & Act
        var right = new Right();

        // Assert
        Assert.NotNull(right.UserRights);
        Assert.Empty(right.UserRights);
        Assert.IsAssignableFrom<ICollection<UserRight>>(right.UserRights);
    }

    [Fact]
    public void Right_Description_ShouldAcceptNull()
    {
        // Arrange
        var right = new Right();

        // Act
        right.Description = null;

        // Assert
        Assert.Null(right.Description);
    }

    [Fact]
    public void Right_ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var right = new Right();

        // Act
        right.ModifiedDate = null;

        // Assert
        Assert.Null(right.ModifiedDate);
    }

    [Fact]
    public void Right_Name_ShouldAcceptEmptyString()
    {
        // Arrange
        var right = new Right();

        // Act
        right.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, right.Name);
    }
}
