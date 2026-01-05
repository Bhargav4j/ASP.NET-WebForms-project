using Films.Domain.Entities;
using Xunit;

namespace Films.UnitTests.Entities;

public class TypeUserTests
{
    [Fact]
    public void TypeUser_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var typeUser = new TypeUser();

        // Assert
        Assert.NotNull(typeUser);
        Assert.Equal(string.Empty, typeUser.Name);
        Assert.Equal(string.Empty, typeUser.CreatedBy);
        Assert.NotNull(typeUser.Users);
        Assert.Empty(typeUser.Users);
    }

    [Fact]
    public void TypeUser_SetId_ShouldSetCorrectly()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedId = 1;

        // Act
        typeUser.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, typeUser.Id);
    }

    [Fact]
    public void TypeUser_SetName_ShouldSetCorrectly()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedName = "Administrator";

        // Act
        typeUser.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, typeUser.Name);
    }

    [Fact]
    public void TypeUser_SetDescription_ShouldSetCorrectly()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedDescription = "System administrator";

        // Act
        typeUser.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, typeUser.Description);
    }

    [Fact]
    public void TypeUser_SetCreatedDate_ShouldSetCorrectly()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedDate = DateTime.UtcNow;

        // Act
        typeUser.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, typeUser.CreatedDate);
    }

    [Fact]
    public void TypeUser_SetModifiedDate_ShouldSetCorrectly()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedDate = DateTime.UtcNow;

        // Act
        typeUser.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, typeUser.ModifiedDate);
    }

    [Fact]
    public void TypeUser_SetIsActive_ShouldSetCorrectly()
    {
        // Arrange
        var typeUser = new TypeUser();

        // Act
        typeUser.IsActive = true;

        // Assert
        Assert.True(typeUser.IsActive);
    }

    [Fact]
    public void TypeUser_SetCreatedBy_ShouldSetCorrectly()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedUser = "admin";

        // Act
        typeUser.CreatedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, typeUser.CreatedBy);
    }

    [Fact]
    public void TypeUser_SetModifiedBy_ShouldSetCorrectly()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedUser = "admin";

        // Act
        typeUser.ModifiedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, typeUser.ModifiedBy);
    }

    [Fact]
    public void TypeUser_SetUsers_ShouldSetCorrectly()
    {
        // Arrange
        var typeUser = new TypeUser();
        var users = new List<User> { new User { Id = 1, Username = "testuser" } };

        // Act
        typeUser.Users = users;

        // Assert
        Assert.Equal(users, typeUser.Users);
        Assert.Single(typeUser.Users);
    }

    [Fact]
    public void TypeUser_AllProperties_ShouldWorkTogether()
    {
        // Arrange & Act
        var typeUser = new TypeUser
        {
            Id = 1,
            Name = "Regular User",
            Description = "Standard user type",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "system",
            ModifiedBy = "admin"
        };

        // Assert
        Assert.Equal(1, typeUser.Id);
        Assert.Equal("Regular User", typeUser.Name);
        Assert.Equal("Standard user type", typeUser.Description);
        Assert.True(typeUser.IsActive);
        Assert.Equal("system", typeUser.CreatedBy);
        Assert.Equal("admin", typeUser.ModifiedBy);
    }
}
