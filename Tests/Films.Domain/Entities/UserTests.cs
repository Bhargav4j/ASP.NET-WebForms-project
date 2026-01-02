using System;
using System.Collections.Generic;
using Xunit;
using Films.Domain.Entities;

namespace Films.Tests.Domain.Entities;

public class UserTests
{
    [Fact]
    public void User_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal(0, user.Id);
        Assert.Equal(string.Empty, user.Username);
        Assert.Equal(string.Empty, user.Password);
        Assert.Equal(string.Empty, user.Email);
        Assert.Null(user.TypeUserId);
        Assert.False(user.IsActive);
        Assert.Equal(string.Empty, user.CreatedBy);
        Assert.Null(user.ModifiedBy);
        Assert.NotNull(user.UserRights);
    }

    [Fact]
    public void User_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var user = new User();
        var testDate = DateTime.UtcNow;

        // Act
        user.Id = 1;
        user.Username = "testuser";
        user.Password = "password123";
        user.Email = "test@example.com";
        user.TypeUserId = 1;
        user.CreatedDate = testDate;
        user.ModifiedDate = testDate;
        user.IsActive = true;
        user.CreatedBy = "TestUser";
        user.ModifiedBy = "ModifiedUser";

        // Assert
        Assert.Equal(1, user.Id);
        Assert.Equal("testuser", user.Username);
        Assert.Equal("password123", user.Password);
        Assert.Equal("test@example.com", user.Email);
        Assert.Equal(1, user.TypeUserId);
        Assert.Equal(testDate, user.CreatedDate);
        Assert.Equal(testDate, user.ModifiedDate);
        Assert.True(user.IsActive);
        Assert.Equal("TestUser", user.CreatedBy);
        Assert.Equal("ModifiedUser", user.ModifiedBy);
    }

    [Fact]
    public void User_UserRights_ShouldInitializeAsEmptyCollection()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.NotNull(user.UserRights);
        Assert.Empty(user.UserRights);
        Assert.IsAssignableFrom<ICollection<UserRight>>(user.UserRights);
    }

    [Fact]
    public void User_TypeUser_ShouldAcceptNull()
    {
        // Arrange
        var user = new User();

        // Act
        user.TypeUser = null;

        // Assert
        Assert.Null(user.TypeUser);
    }

    [Fact]
    public void User_TypeUserId_ShouldAcceptNull()
    {
        // Arrange
        var user = new User();

        // Act
        user.TypeUserId = null;

        // Assert
        Assert.Null(user.TypeUserId);
    }

    [Fact]
    public void User_ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var user = new User();

        // Act
        user.ModifiedDate = null;

        // Assert
        Assert.Null(user.ModifiedDate);
    }

    [Fact]
    public void User_Username_ShouldAcceptEmptyString()
    {
        // Arrange
        var user = new User();

        // Act
        user.Username = string.Empty;

        // Assert
        Assert.Equal(string.Empty, user.Username);
    }

    [Fact]
    public void User_Email_ShouldAcceptValidEmail()
    {
        // Arrange
        var user = new User();

        // Act
        user.Email = "valid@test.com";

        // Assert
        Assert.Equal("valid@test.com", user.Email);
    }
}
