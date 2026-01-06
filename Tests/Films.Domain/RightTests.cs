using Xunit;
using Films.Domain.Entities;

namespace Films.Domain.Tests;

public class RightTests
{
    [Fact]
    public void Right_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var right = new Right();

        // Assert
        Assert.Equal(0, right.Id);
        Assert.Equal(string.Empty, right.Name);
        Assert.Null(right.Description);
        Assert.True(right.IsActive);
        Assert.Equal("System", right.CreatedBy);
        Assert.Null(right.ModifiedBy);
        Assert.Null(right.ModifiedDate);
        Assert.NotNull(right.UserRights);
        Assert.Empty(right.UserRights);
    }

    [Fact]
    public void Right_SetProperties_SetsCorrectly()
    {
        // Arrange
        var right = new Right();
        var now = DateTime.UtcNow;

        // Act
        right.Id = 1;
        right.Name = "Admin";
        right.Description = "Administrator rights";
        right.CreatedDate = now;
        right.ModifiedDate = now;
        right.IsActive = false;
        right.CreatedBy = "Admin";
        right.ModifiedBy = "User";

        // Assert
        Assert.Equal(1, right.Id);
        Assert.Equal("Admin", right.Name);
        Assert.Equal("Administrator rights", right.Description);
        Assert.Equal(now, right.CreatedDate);
        Assert.Equal(now, right.ModifiedDate);
        Assert.False(right.IsActive);
        Assert.Equal("Admin", right.CreatedBy);
        Assert.Equal("User", right.ModifiedBy);
    }

    [Fact]
    public void Right_Name_CanBeSetToEmptyString()
    {
        // Arrange
        var right = new Right { Name = "Initial" };

        // Act
        right.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, right.Name);
    }

    [Fact]
    public void Right_Description_CanBeNull()
    {
        // Arrange
        var right = new Right { Description = "Test" };

        // Act
        right.Description = null;

        // Assert
        Assert.Null(right.Description);
    }

    [Fact]
    public void Right_UserRights_CanAddItems()
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
    public void Right_IsActive_DefaultsToTrue()
    {
        // Arrange & Act
        var right = new Right();

        // Assert
        Assert.True(right.IsActive);
    }

    [Theory]
    [InlineData("Read", "Read access")]
    [InlineData("Write", "Write access")]
    [InlineData("Delete", "Delete access")]
    [InlineData("Admin", null)]
    public void Right_NameAndDescription_AcceptsVariousValues(string name, string? description)
    {
        // Arrange
        var right = new Right();

        // Act
        right.Name = name;
        right.Description = description;

        // Assert
        Assert.Equal(name, right.Name);
        Assert.Equal(description, right.Description);
    }

    [Fact]
    public void Right_CreatedDate_CanBeSet()
    {
        // Arrange
        var right = new Right();
        var date = new DateTime(2020, 1, 1);

        // Act
        right.CreatedDate = date;

        // Assert
        Assert.Equal(date, right.CreatedDate);
    }

    [Fact]
    public void Right_ModifiedDate_CanBeNull()
    {
        // Arrange
        var right = new Right { ModifiedDate = DateTime.UtcNow };

        // Act
        right.ModifiedDate = null;

        // Assert
        Assert.Null(right.ModifiedDate);
    }

    [Fact]
    public void Right_ModifiedBy_CanBeNull()
    {
        // Arrange
        var right = new Right { ModifiedBy = "User" };

        // Act
        right.ModifiedBy = null;

        // Assert
        Assert.Null(right.ModifiedBy);
    }
}
