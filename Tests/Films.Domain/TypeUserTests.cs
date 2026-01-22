using Xunit;
using Films.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Films.Domain.Tests;

public class TypeUserTests
{
    [Fact]
    public void TypeUser_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var typeUser = new TypeUser();

        // Assert
        Assert.Equal(0, typeUser.Id);
        Assert.Equal(string.Empty, typeUser.Name);
        Assert.Null(typeUser.Description);
        Assert.Equal(default(DateTime), typeUser.CreatedDate);
        Assert.Null(typeUser.ModifiedDate);
        Assert.False(typeUser.IsActive);
        Assert.Equal(string.Empty, typeUser.CreatedBy);
        Assert.Null(typeUser.ModifiedBy);
        Assert.NotNull(typeUser.Users);
        Assert.Empty(typeUser.Users);
    }

    [Fact]
    public void TypeUser_Id_CanBeSetAndRetrieved()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedId = 10;

        // Act
        typeUser.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, typeUser.Id);
    }

    [Fact]
    public void TypeUser_Name_CanBeSetAndRetrieved()
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
    public void TypeUser_Name_CanBeSetToEmptyString()
    {
        // Arrange
        var typeUser = new TypeUser();

        // Act
        typeUser.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, typeUser.Name);
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("User")]
    [InlineData("Moderator")]
    [InlineData("Guest")]
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
    public void TypeUser_Description_CanBeSetAndRetrieved()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedDescription = "Administrator with full access";

        // Act
        typeUser.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, typeUser.Description);
    }

    [Fact]
    public void TypeUser_Description_CanBeNull()
    {
        // Arrange
        var typeUser = new TypeUser();

        // Act
        typeUser.Description = null;

        // Assert
        Assert.Null(typeUser.Description);
    }

    [Fact]
    public void TypeUser_CreatedDate_CanBeSetAndRetrieved()
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
    public void TypeUser_ModifiedDate_CanBeSetAndRetrieved()
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
    public void TypeUser_ModifiedDate_CanBeNull()
    {
        // Arrange
        var typeUser = new TypeUser();

        // Act
        typeUser.ModifiedDate = null;

        // Assert
        Assert.Null(typeUser.ModifiedDate);
    }

    [Fact]
    public void TypeUser_IsActive_CanBeSetToTrue()
    {
        // Arrange
        var typeUser = new TypeUser();

        // Act
        typeUser.IsActive = true;

        // Assert
        Assert.True(typeUser.IsActive);
    }

    [Fact]
    public void TypeUser_IsActive_CanBeSetToFalse()
    {
        // Arrange
        var typeUser = new TypeUser();

        // Act
        typeUser.IsActive = false;

        // Assert
        Assert.False(typeUser.IsActive);
    }

    [Fact]
    public void TypeUser_CreatedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedCreatedBy = "system";

        // Act
        typeUser.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, typeUser.CreatedBy);
    }

    [Fact]
    public void TypeUser_ModifiedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedModifiedBy = "admin";

        // Act
        typeUser.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, typeUser.ModifiedBy);
    }

    [Fact]
    public void TypeUser_ModifiedBy_CanBeNull()
    {
        // Arrange
        var typeUser = new TypeUser();

        // Act
        typeUser.ModifiedBy = null;

        // Assert
        Assert.Null(typeUser.ModifiedBy);
    }

    [Fact]
    public void TypeUser_Users_CanBePopulated()
    {
        // Arrange
        var typeUser = new TypeUser();
        var user1 = new User { Id = 1, Username = "user1" };
        var user2 = new User { Id = 2, Username = "user2" };

        // Act
        typeUser.Users = new List<User> { user1, user2 };

        // Assert
        Assert.Equal(2, typeUser.Users.Count);
        Assert.Contains(user1, typeUser.Users);
        Assert.Contains(user2, typeUser.Users);
    }

    [Fact]
    public void TypeUser_AllProperties_CanBeSetTogether()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedId = 7;
        var expectedName = "SuperAdmin";
        var expectedDescription = "Super administrator with elevated privileges";
        var expectedCreatedDate = DateTime.Now;
        var expectedModifiedDate = DateTime.Now.AddDays(3);
        var expectedIsActive = true;
        var expectedCreatedBy = "root";
        var expectedModifiedBy = "admin";

        // Act
        typeUser.Id = expectedId;
        typeUser.Name = expectedName;
        typeUser.Description = expectedDescription;
        typeUser.CreatedDate = expectedCreatedDate;
        typeUser.ModifiedDate = expectedModifiedDate;
        typeUser.IsActive = expectedIsActive;
        typeUser.CreatedBy = expectedCreatedBy;
        typeUser.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, typeUser.Id);
        Assert.Equal(expectedName, typeUser.Name);
        Assert.Equal(expectedDescription, typeUser.Description);
        Assert.Equal(expectedCreatedDate, typeUser.CreatedDate);
        Assert.Equal(expectedModifiedDate, typeUser.ModifiedDate);
        Assert.Equal(expectedIsActive, typeUser.IsActive);
        Assert.Equal(expectedCreatedBy, typeUser.CreatedBy);
        Assert.Equal(expectedModifiedBy, typeUser.ModifiedBy);
    }
}
