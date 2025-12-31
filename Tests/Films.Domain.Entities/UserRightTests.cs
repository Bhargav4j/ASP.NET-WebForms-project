using Xunit;
using Films.Domain.Entities;
using System;

namespace Films.Domain.Entities.Tests;

public class UserRightTests
{
    [Fact]
    public void UserRight_SetId_ReturnsCorrectValue()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedId = 1;

        // Act
        userRight.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, userRight.Id);
    }

    [Fact]
    public void UserRight_SetUserId_ReturnsCorrectValue()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedUserId = 10;

        // Act
        userRight.UserId = expectedUserId;

        // Assert
        Assert.Equal(expectedUserId, userRight.UserId);
    }

    [Fact]
    public void UserRight_SetRightId_ReturnsCorrectValue()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedRightId = 20;

        // Act
        userRight.RightId = expectedRightId;

        // Assert
        Assert.Equal(expectedRightId, userRight.RightId);
    }

    [Fact]
    public void UserRight_SetCreatedDate_ReturnsCorrectValue()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedDate = DateTime.UtcNow;

        // Act
        userRight.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, userRight.CreatedDate);
    }

    [Fact]
    public void UserRight_SetIsActive_ReturnsCorrectValue()
    {
        // Arrange
        var userRight = new UserRight();

        // Act
        userRight.IsActive = true;

        // Assert
        Assert.True(userRight.IsActive);
    }

    [Fact]
    public void UserRight_SetUser_ReturnsCorrectValue()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedUser = new User { Id = 1, Username = "johndoe" };

        // Act
        userRight.User = expectedUser;

        // Assert
        Assert.Equal(expectedUser, userRight.User);
    }

    [Fact]
    public void UserRight_SetRight_ReturnsCorrectValue()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedRight = new Right { Id = 1, Name = "Read" };

        // Act
        userRight.Right = expectedRight;

        // Assert
        Assert.Equal(expectedRight, userRight.Right);
    }

    [Fact]
    public void UserRight_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedId = 5;
        var expectedUserId = 15;
        var expectedRightId = 25;
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedIsActive = true;
        var expectedUser = new User { Id = 15, Username = "testuser" };
        var expectedRight = new Right { Id = 25, Name = "Write" };

        // Act
        userRight.Id = expectedId;
        userRight.UserId = expectedUserId;
        userRight.RightId = expectedRightId;
        userRight.CreatedDate = expectedCreatedDate;
        userRight.IsActive = expectedIsActive;
        userRight.User = expectedUser;
        userRight.Right = expectedRight;

        // Assert
        Assert.Equal(expectedId, userRight.Id);
        Assert.Equal(expectedUserId, userRight.UserId);
        Assert.Equal(expectedRightId, userRight.RightId);
        Assert.Equal(expectedCreatedDate, userRight.CreatedDate);
        Assert.Equal(expectedIsActive, userRight.IsActive);
        Assert.Equal(expectedUser, userRight.User);
        Assert.Equal(expectedRight, userRight.Right);
    }

    [Fact]
    public void UserRight_UserAndRightRelationship_WorksCorrectly()
    {
        // Arrange
        var user = new User { Id = 1, Username = "admin" };
        var right = new Right { Id = 1, Name = "Admin" };
        var userRight = new UserRight
        {
            Id = 1,
            UserId = user.Id,
            RightId = right.Id,
            User = user,
            Right = right
        };

        // Act & Assert
        Assert.Equal(user.Id, userRight.UserId);
        Assert.Equal(right.Id, userRight.RightId);
        Assert.Equal(user, userRight.User);
        Assert.Equal(right, userRight.Right);
    }

    [Fact]
    public void UserRight_SetIsActive_False_ReturnsCorrectValue()
    {
        // Arrange
        var userRight = new UserRight();

        // Act
        userRight.IsActive = false;

        // Assert
        Assert.False(userRight.IsActive);
    }
}
