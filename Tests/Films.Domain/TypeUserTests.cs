using Xunit;
using Films.Domain.Entities;

namespace Films.Domain.Tests;

public class TypeUserTests
{
    [Fact]
    public void TypeUser_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var typeUser = new TypeUser();

        // Assert
        Assert.Equal(0, typeUser.Id);
        Assert.Equal(string.Empty, typeUser.Name);
        Assert.True(typeUser.IsActive);
        Assert.Equal("System", typeUser.CreatedBy);
        Assert.Null(typeUser.ModifiedBy);
        Assert.Null(typeUser.ModifiedDate);
        Assert.NotNull(typeUser.Users);
        Assert.Empty(typeUser.Users);
    }

    [Fact]
    public void TypeUser_SetProperties_SetsCorrectly()
    {
        // Arrange
        var typeUser = new TypeUser();
        var now = DateTime.UtcNow;

        // Act
        typeUser.Id = 1;
        typeUser.Name = "Admin";
        typeUser.CreatedDate = now;
        typeUser.ModifiedDate = now;
        typeUser.IsActive = false;
        typeUser.CreatedBy = "Admin";
        typeUser.ModifiedBy = "User";

        // Assert
        Assert.Equal(1, typeUser.Id);
        Assert.Equal("Admin", typeUser.Name);
        Assert.Equal(now, typeUser.CreatedDate);
        Assert.Equal(now, typeUser.ModifiedDate);
        Assert.False(typeUser.IsActive);
        Assert.Equal("Admin", typeUser.CreatedBy);
        Assert.Equal("User", typeUser.ModifiedBy);
    }

    [Fact]
    public void TypeUser_Name_CanBeSetToEmptyString()
    {
        // Arrange
        var typeUser = new TypeUser { Name = "Initial" };

        // Act
        typeUser.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, typeUser.Name);
    }

    [Fact]
    public void TypeUser_Users_CanAddItems()
    {
        // Arrange
        var typeUser = new TypeUser();
        var user = new User { Id = 1, Username = "testuser", TypeUserId = 1 };

        // Act
        typeUser.Users.Add(user);

        // Assert
        Assert.Single(typeUser.Users);
        Assert.Contains(user, typeUser.Users);
    }

    [Fact]
    public void TypeUser_IsActive_DefaultsToTrue()
    {
        // Arrange & Act
        var typeUser = new TypeUser();

        // Assert
        Assert.True(typeUser.IsActive);
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("User")]
    [InlineData("Guest")]
    [InlineData("Moderator")]
    [InlineData("")]
    public void TypeUser_Name_AcceptsVariousValues(string name)
    {
        // Arrange
        var typeUser = new TypeUser();

        // Act
        typeUser.Name = name;

        // Assert
        Assert.Equal(name, typeUser.Name);
    }

    [Fact]
    public void TypeUser_CreatedDate_CanBeSet()
    {
        // Arrange
        var typeUser = new TypeUser();
        var date = new DateTime(2020, 1, 1);

        // Act
        typeUser.CreatedDate = date;

        // Assert
        Assert.Equal(date, typeUser.CreatedDate);
    }

    [Fact]
    public void TypeUser_ModifiedDate_CanBeNull()
    {
        // Arrange
        var typeUser = new TypeUser { ModifiedDate = DateTime.UtcNow };

        // Act
        typeUser.ModifiedDate = null;

        // Assert
        Assert.Null(typeUser.ModifiedDate);
    }

    [Fact]
    public void TypeUser_ModifiedBy_CanBeNull()
    {
        // Arrange
        var typeUser = new TypeUser { ModifiedBy = "User" };

        // Act
        typeUser.ModifiedBy = null;

        // Assert
        Assert.Null(typeUser.ModifiedBy);
    }
}
