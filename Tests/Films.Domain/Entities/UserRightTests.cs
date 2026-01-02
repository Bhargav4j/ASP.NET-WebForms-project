using System;
using Xunit;
using Films.Domain.Entities;

namespace Films.Tests.Domain.Entities;

public class UserRightTests
{
    [Fact]
    public void UserRight_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var userRight = new UserRight();

        // Assert
        Assert.Equal(0, userRight.Id);
        Assert.Equal(0, userRight.UserId);
        Assert.Equal(0, userRight.RightId);
        Assert.False(userRight.IsActive);
        Assert.Equal(string.Empty, userRight.CreatedBy);
    }

    [Fact]
    public void UserRight_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var userRight = new UserRight();
        var testDate = DateTime.UtcNow;

        // Act
        userRight.Id = 1;
        userRight.UserId = 10;
        userRight.RightId = 20;
        userRight.CreatedDate = testDate;
        userRight.IsActive = true;
        userRight.CreatedBy = "TestUser";

        // Assert
        Assert.Equal(1, userRight.Id);
        Assert.Equal(10, userRight.UserId);
        Assert.Equal(20, userRight.RightId);
        Assert.Equal(testDate, userRight.CreatedDate);
        Assert.True(userRight.IsActive);
        Assert.Equal("TestUser", userRight.CreatedBy);
    }

    [Fact]
    public void UserRight_User_ShouldAcceptUserInstance()
    {
        // Arrange
        var userRight = new UserRight();
        var user = new User { Id = 10, Username = "testuser" };

        // Act
        userRight.User = user;

        // Assert
        Assert.NotNull(userRight.User);
        Assert.Equal(10, userRight.User.Id);
    }

    [Fact]
    public void UserRight_Right_ShouldAcceptRightInstance()
    {
        // Arrange
        var userRight = new UserRight();
        var right = new Right { Id = 20, Name = "ViewFilms" };

        // Act
        userRight.Right = right;

        // Assert
        Assert.NotNull(userRight.Right);
        Assert.Equal(20, userRight.Right.Id);
    }

    [Fact]
    public void UserRight_CreatedBy_ShouldAcceptEmptyString()
    {
        // Arrange
        var userRight = new UserRight();

        // Act
        userRight.CreatedBy = string.Empty;

        // Assert
        Assert.Equal(string.Empty, userRight.CreatedBy);
    }
}
