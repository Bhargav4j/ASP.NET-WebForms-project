using Xunit;
using Films.Domain.Entities;
using System;

namespace Films.Domain.Entities.Tests;

public class UserTests
{
    [Fact]
    public void User_Constructor_InitializesCollections()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.NotNull(user.UserRights);
        Assert.Empty(user.UserRights);
    }

    [Fact]
    public void User_SetId_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();
        var expectedId = 1;

        // Act
        user.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, user.Id);
    }

    [Fact]
    public void User_SetUsername_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();
        var expectedUsername = "johndoe";

        // Act
        user.Username = expectedUsername;

        // Assert
        Assert.Equal(expectedUsername, user.Username);
    }

    [Fact]
    public void User_DefaultUsername_IsEmptyString()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal(string.Empty, user.Username);
    }

    [Fact]
    public void User_SetEmail_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();
        var expectedEmail = "john@example.com";

        // Act
        user.Email = expectedEmail;

        // Assert
        Assert.Equal(expectedEmail, user.Email);
    }

    [Fact]
    public void User_DefaultEmail_IsEmptyString()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal(string.Empty, user.Email);
    }

    [Fact]
    public void User_SetPasswordHash_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();
        var expectedHash = "hashedpassword123";

        // Act
        user.PasswordHash = expectedHash;

        // Assert
        Assert.Equal(expectedHash, user.PasswordHash);
    }

    [Fact]
    public void User_DefaultPasswordHash_IsEmptyString()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal(string.Empty, user.PasswordHash);
    }

    [Fact]
    public void User_SetTypeUserId_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();
        var expectedTypeUserId = 1;

        // Act
        user.TypeUserId = expectedTypeUserId;

        // Assert
        Assert.Equal(expectedTypeUserId, user.TypeUserId);
    }

    [Fact]
    public void User_SetTypeUserId_AllowsNull()
    {
        // Arrange
        var user = new User();

        // Act
        user.TypeUserId = null;

        // Assert
        Assert.Null(user.TypeUserId);
    }

    [Fact]
    public void User_SetTypeUser_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();
        var expectedTypeUser = new TypeUser { Id = 1, Name = "Admin" };

        // Act
        user.TypeUser = expectedTypeUser;

        // Assert
        Assert.Equal(expectedTypeUser, user.TypeUser);
    }

    [Fact]
    public void User_SetIsActive_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();

        // Act
        user.IsActive = true;

        // Assert
        Assert.True(user.IsActive);
    }

    [Fact]
    public void User_SetCreatedDate_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();
        var expectedDate = DateTime.UtcNow;

        // Act
        user.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, user.CreatedDate);
    }

    [Fact]
    public void User_SetModifiedDate_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();
        var expectedDate = DateTime.UtcNow;

        // Act
        user.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, user.ModifiedDate);
    }

    [Fact]
    public void User_SetCreatedBy_ReturnsCorrectValue()
    {
        // Arrange
        var user = new User();
        var expectedCreatedBy = "Admin";

        // Act
        user.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, user.CreatedBy);
    }

    [Fact]
    public void User_DefaultCreatedBy_IsEmptyString()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal(string.Empty, user.CreatedBy);
    }

    [Fact]
    public void User_AddUserRight_AddsToCollection()
    {
        // Arrange
        var user = new User();
        var userRight = new UserRight { Id = 1, UserId = 1, RightId = 1 };

        // Act
        user.UserRights.Add(userRight);

        // Assert
        Assert.Single(user.UserRights);
        Assert.Contains(userRight, user.UserRights);
    }

    [Fact]
    public void User_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var user = new User();
        var expectedId = 10;
        var expectedUsername = "testuser";
        var expectedEmail = "test@example.com";
        var expectedPasswordHash = "hashed123";
        var expectedTypeUserId = 1;
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedModifiedDate = DateTime.UtcNow.AddDays(1);
        var expectedIsActive = true;
        var expectedCreatedBy = "System";
        var expectedModifiedBy = "Admin";

        // Act
        user.Id = expectedId;
        user.Username = expectedUsername;
        user.Email = expectedEmail;
        user.PasswordHash = expectedPasswordHash;
        user.TypeUserId = expectedTypeUserId;
        user.CreatedDate = expectedCreatedDate;
        user.ModifiedDate = expectedModifiedDate;
        user.IsActive = expectedIsActive;
        user.CreatedBy = expectedCreatedBy;
        user.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, user.Id);
        Assert.Equal(expectedUsername, user.Username);
        Assert.Equal(expectedEmail, user.Email);
        Assert.Equal(expectedPasswordHash, user.PasswordHash);
        Assert.Equal(expectedTypeUserId, user.TypeUserId);
        Assert.Equal(expectedCreatedDate, user.CreatedDate);
        Assert.Equal(expectedModifiedDate, user.ModifiedDate);
        Assert.Equal(expectedIsActive, user.IsActive);
        Assert.Equal(expectedCreatedBy, user.CreatedBy);
        Assert.Equal(expectedModifiedBy, user.ModifiedBy);
    }
}
