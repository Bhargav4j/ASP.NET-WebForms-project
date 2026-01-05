using Films.Domain.Entities;
using Xunit;

namespace Films.UnitTests.Entities;

public class UserRightTests
{
    [Fact]
    public void UserRight_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var userRight = new UserRight();

        // Assert
        Assert.NotNull(userRight);
        Assert.Equal(string.Empty, userRight.CreatedBy);
    }

    [Fact]
    public void UserRight_SetId_ShouldSetCorrectly()
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
    public void UserRight_SetUserId_ShouldSetCorrectly()
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
    public void UserRight_SetRightId_ShouldSetCorrectly()
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
    public void UserRight_SetCreatedDate_ShouldSetCorrectly()
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
    public void UserRight_SetModifiedDate_ShouldSetCorrectly()
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
    public void UserRight_SetIsActive_ShouldSetCorrectly()
    {
        // Arrange
        var userRight = new UserRight();

        // Act
        userRight.IsActive = true;

        // Assert
        Assert.True(userRight.IsActive);
    }

    [Fact]
    public void UserRight_SetCreatedBy_ShouldSetCorrectly()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedUser = "admin";

        // Act
        userRight.CreatedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, userRight.CreatedBy);
    }

    [Fact]
    public void UserRight_SetModifiedBy_ShouldSetCorrectly()
    {
        // Arrange
        var userRight = new UserRight();
        var expectedUser = "admin";

        // Act
        userRight.ModifiedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, userRight.ModifiedBy);
    }

    [Fact]
    public void UserRight_SetUser_ShouldSetCorrectly()
    {
        // Arrange
        var userRight = new UserRight();
        var user = new User { Id = 1, Username = "testuser" };

        // Act
        userRight.User = user;

        // Assert
        Assert.NotNull(userRight.User);
        Assert.Equal(1, userRight.User.Id);
        Assert.Equal("testuser", userRight.User.Username);
    }

    [Fact]
    public void UserRight_SetRight_ShouldSetCorrectly()
    {
        // Arrange
        var userRight = new UserRight();
        var right = new Right { Id = 1, Name = "Read" };

        // Act
        userRight.Right = right;

        // Assert
        Assert.NotNull(userRight.Right);
        Assert.Equal(1, userRight.Right.Id);
        Assert.Equal("Read", userRight.Right.Name);
    }

    [Fact]
    public void UserRight_AllProperties_ShouldWorkTogether()
    {
        // Arrange & Act
        var userRight = new UserRight
        {
            Id = 1,
            UserId = 5,
            RightId = 10,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "system",
            ModifiedBy = "admin"
        };

        // Assert
        Assert.Equal(1, userRight.Id);
        Assert.Equal(5, userRight.UserId);
        Assert.Equal(10, userRight.RightId);
        Assert.True(userRight.IsActive);
        Assert.Equal("system", userRight.CreatedBy);
        Assert.Equal("admin", userRight.ModifiedBy);
    }

    [Fact]
    public void UserRight_NavigationProperties_ShouldWorkCorrectly()
    {
        // Arrange
        var user = new User { Id = 1, Username = "user1" };
        var right = new Right { Id = 2, Name = "Write" };

        // Act
        var userRight = new UserRight
        {
            Id = 1,
            UserId = user.Id,
            RightId = right.Id,
            User = user,
            Right = right
        };

        // Assert
        Assert.Equal(user.Id, userRight.UserId);
        Assert.Equal(right.Id, userRight.RightId);
        Assert.Equal(user, userRight.User);
        Assert.Equal(right, userRight.Right);
    }
}
