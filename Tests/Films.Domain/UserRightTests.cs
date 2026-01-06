using Xunit;
using Films.Domain.Entities;

namespace Films.Domain.Tests;

public class UserRightTests
{
    [Fact]
    public void UserRight_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var userRight = new UserRight();

        // Assert
        Assert.Equal(0, userRight.Id);
        Assert.Equal(0, userRight.UserId);
        Assert.Equal(0, userRight.RightId);
        Assert.True(userRight.IsActive);
        Assert.Equal("System", userRight.CreatedBy);
        Assert.Null(userRight.ModifiedBy);
        Assert.Null(userRight.ModifiedDate);
    }

    [Fact]
    public void UserRight_SetProperties_SetsCorrectly()
    {
        // Arrange
        var userRight = new UserRight();
        var now = DateTime.UtcNow;

        // Act
        userRight.Id = 1;
        userRight.UserId = 5;
        userRight.RightId = 10;
        userRight.CreatedDate = now;
        userRight.ModifiedDate = now;
        userRight.IsActive = false;
        userRight.CreatedBy = "Admin";
        userRight.ModifiedBy = "User";

        // Assert
        Assert.Equal(1, userRight.Id);
        Assert.Equal(5, userRight.UserId);
        Assert.Equal(10, userRight.RightId);
        Assert.Equal(now, userRight.CreatedDate);
        Assert.Equal(now, userRight.ModifiedDate);
        Assert.False(userRight.IsActive);
        Assert.Equal("Admin", userRight.CreatedBy);
        Assert.Equal("User", userRight.ModifiedBy);
    }

    [Fact]
    public void UserRight_IsActive_DefaultsToTrue()
    {
        // Arrange & Act
        var userRight = new UserRight();

        // Assert
        Assert.True(userRight.IsActive);
    }

    [Fact]
    public void UserRight_NavigationProperties_CanBeSet()
    {
        // Arrange
        var userRight = new UserRight();
        var user = new User { Id = 1, Username = "testuser" };
        var right = new Right { Id = 1, Name = "Admin" };

        // Act
        userRight.User = user;
        userRight.Right = right;
        userRight.UserId = user.Id;
        userRight.RightId = right.Id;

        // Assert
        Assert.NotNull(userRight.User);
        Assert.NotNull(userRight.Right);
        Assert.Equal(1, userRight.UserId);
        Assert.Equal(1, userRight.RightId);
        Assert.Equal("testuser", userRight.User.Username);
        Assert.Equal("Admin", userRight.Right.Name);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(5, 10)]
    [InlineData(100, 200)]
    public void UserRight_UserIdAndRightId_AcceptsVariousValues(int userId, int rightId)
    {
        // Arrange
        var userRight = new UserRight();

        // Act
        userRight.UserId = userId;
        userRight.RightId = rightId;

        // Assert
        Assert.Equal(userId, userRight.UserId);
        Assert.Equal(rightId, userRight.RightId);
    }

    [Fact]
    public void UserRight_CreatedDate_CanBeSet()
    {
        // Arrange
        var userRight = new UserRight();
        var date = new DateTime(2020, 1, 1);

        // Act
        userRight.CreatedDate = date;

        // Assert
        Assert.Equal(date, userRight.CreatedDate);
    }

    [Fact]
    public void UserRight_ModifiedDate_CanBeNull()
    {
        // Arrange
        var userRight = new UserRight { ModifiedDate = DateTime.UtcNow };

        // Act
        userRight.ModifiedDate = null;

        // Assert
        Assert.Null(userRight.ModifiedDate);
    }

    [Fact]
    public void UserRight_ModifiedBy_CanBeNull()
    {
        // Arrange
        var userRight = new UserRight { ModifiedBy = "User" };

        // Act
        userRight.ModifiedBy = null;

        // Assert
        Assert.Null(userRight.ModifiedBy);
    }
}
