using Xunit;
using Films.Domain.Entities;
using System;

namespace Films.Domain.Tests;

public class UserRightTests
{
    [Fact]
    public void UserRight_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var userRight = new UserRight();

        // Assert
        Assert.Equal(0, userRight.Id);
        Assert.Equal(0, userRight.UserId);
        Assert.Equal(0, userRight.RightId);
        Assert.Equal(default(DateTime), userRight.CreatedDate);
        Assert.Null(userRight.ModifiedDate);
        Assert.False(userRight.IsActive);
        Assert.Equal(string.Empty, userRight.CreatedBy);
        Assert.Null(userRight.ModifiedBy);
        Assert.Null(userRight.User);
        Assert.Null(userRight.Right);
    }

    [Fact]
    public void UserRight_Id_CanBeSetAndRetrieved()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedId = 123;

        // Act
        userRight.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, userRight.Id);
    }

    [Fact]
    public void UserRight_UserId_CanBeSetAndRetrieved()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedUserId = 456;

        // Act
        userRight.UserId = expectedUserId;

        // Assert
        Assert.Equal(expectedUserId, userRight.UserId);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(999)]
    public void UserRight_UserId_AcceptsVariousValues(int userId)
    {
        // Arrange
        var userRight = new UserRight();

        // Act
        userRight.UserId = userId;

        // Assert
        Assert.Equal(userId, userRight.UserId);
    }

    [Fact]
    public void UserRight_RightId_CanBeSetAndRetrieved()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedRightId = 789;

        // Act
        userRight.RightId = expectedRightId;

        // Assert
        Assert.Equal(expectedRightId, userRight.RightId);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(200)]
    public void UserRight_RightId_AcceptsVariousValues(int rightId)
    {
        // Arrange
        var userRight = new UserRight();

        // Act
        userRight.RightId = rightId;

        // Assert
        Assert.Equal(rightId, userRight.RightId);
    }

    [Fact]
    public void UserRight_CreatedDate_CanBeSetAndRetrieved()
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
    public void UserRight_ModifiedDate_CanBeSetAndRetrieved()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedDate = DateTime.UtcNow;

        // Act
        userRight.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, userRight.ModifiedDate);
    }

    [Fact]
    public void UserRight_ModifiedDate_CanBeNull()
    {
        // Arrange
        var userRight = new UserRight();

        // Act
        userRight.ModifiedDate = null;

        // Assert
        Assert.Null(userRight.ModifiedDate);
    }

    [Fact]
    public void UserRight_IsActive_CanBeSetToTrue()
    {
        // Arrange
        var userRight = new UserRight();

        // Act
        userRight.IsActive = true;

        // Assert
        Assert.True(userRight.IsActive);
    }

    [Fact]
    public void UserRight_IsActive_CanBeSetToFalse()
    {
        // Arrange
        var userRight = new UserRight();

        // Act
        userRight.IsActive = false;

        // Assert
        Assert.False(userRight.IsActive);
    }

    [Fact]
    public void UserRight_CreatedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedCreatedBy = "admin";

        // Act
        userRight.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, userRight.CreatedBy);
    }

    [Fact]
    public void UserRight_ModifiedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedModifiedBy = "user123";

        // Act
        userRight.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, userRight.ModifiedBy);
    }

    [Fact]
    public void UserRight_ModifiedBy_CanBeNull()
    {
        // Arrange
        var userRight = new UserRight();

        // Act
        userRight.ModifiedBy = null;

        // Assert
        Assert.Null(userRight.ModifiedBy);
    }

    [Fact]
    public void UserRight_User_CanBeSetAndRetrieved()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedUser = new User { Id = 100, Username = "testuser" };

        // Act
        userRight.User = expectedUser;

        // Assert
        Assert.Equal(expectedUser, userRight.User);
    }

    [Fact]
    public void UserRight_User_CanBeNull()
    {
        // Arrange
        var userRight = new UserRight();

        // Act
        userRight.User = null;

        // Assert
        Assert.Null(userRight.User);
    }

    [Fact]
    public void UserRight_Right_CanBeSetAndRetrieved()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedRight = new Right { Id = 50, Name = "ViewFilms" };

        // Act
        userRight.Right = expectedRight;

        // Assert
        Assert.Equal(expectedRight, userRight.Right);
    }

    [Fact]
    public void UserRight_Right_CanBeNull()
    {
        // Arrange
        var userRight = new UserRight();

        // Act
        userRight.Right = null;

        // Assert
        Assert.Null(userRight.Right);
    }

    [Fact]
    public void UserRight_AllProperties_CanBeSetTogether()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedId = 999;
        var expectedUserId = 111;
        var expectedRightId = 222;
        var expectedCreatedDate = DateTime.Now;
        var expectedModifiedDate = DateTime.Now.AddDays(5);
        var expectedIsActive = true;
        var expectedCreatedBy = "system";
        var expectedModifiedBy = "admin";
        var expectedUser = new User { Id = 111, Username = "john" };
        var expectedRight = new Right { Id = 222, Name = "EditFilms" };

        // Act
        userRight.Id = expectedId;
        userRight.UserId = expectedUserId;
        userRight.RightId = expectedRightId;
        userRight.CreatedDate = expectedCreatedDate;
        userRight.ModifiedDate = expectedModifiedDate;
        userRight.IsActive = expectedIsActive;
        userRight.CreatedBy = expectedCreatedBy;
        userRight.ModifiedBy = expectedModifiedBy;
        userRight.User = expectedUser;
        userRight.Right = expectedRight;

        // Assert
        Assert.Equal(expectedId, userRight.Id);
        Assert.Equal(expectedUserId, userRight.UserId);
        Assert.Equal(expectedRightId, userRight.RightId);
        Assert.Equal(expectedCreatedDate, userRight.CreatedDate);
        Assert.Equal(expectedModifiedDate, userRight.ModifiedDate);
        Assert.Equal(expectedIsActive, userRight.IsActive);
        Assert.Equal(expectedCreatedBy, userRight.CreatedBy);
        Assert.Equal(expectedModifiedBy, userRight.ModifiedBy);
        Assert.Equal(expectedUser, userRight.User);
        Assert.Equal(expectedRight, userRight.Right);
    }
}
