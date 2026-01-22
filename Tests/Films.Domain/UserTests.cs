using Xunit;
using Films.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Films.Domain.Tests;

public class UserTests
{
    [Fact]
    public void User_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal(0, user.Id);
        Assert.Equal(string.Empty, user.Username);
        Assert.Equal(string.Empty, user.Email);
        Assert.Null(user.Phone);
        Assert.Null(user.SecretQuestion);
        Assert.Null(user.SecretAnswer);
        Assert.Equal(0, user.TypeUserId);
        Assert.Equal(default(DateTime), user.CreatedDate);
        Assert.Null(user.ModifiedDate);
        Assert.False(user.IsActive);
        Assert.Equal(string.Empty, user.CreatedBy);
        Assert.Null(user.ModifiedBy);
        Assert.Null(user.TypeUser);
        Assert.NotNull(user.UserRights);
        Assert.Empty(user.UserRights);
    }

    [Fact]
    public void User_Id_CanBeSetAndRetrieved()
    {
        // Arrange
        var user = new User();
        var expectedId = 100;

        // Act
        user.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, user.Id);
    }

    [Fact]
    public void User_Username_CanBeSetAndRetrieved()
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
    public void User_Username_CanBeSetToEmptyString()
    {
        // Arrange
        var user = new User();

        // Act
        user.Username = string.Empty;

        // Assert
        Assert.Equal(string.Empty, user.Username);
    }

    [Fact]
    public void User_Email_CanBeSetAndRetrieved()
    {
        // Arrange
        var user = new User();
        var expectedEmail = "john.doe@example.com";

        // Act
        user.Email = expectedEmail;

        // Assert
        Assert.Equal(expectedEmail, user.Email);
    }

    [Fact]
    public void User_Email_CanBeSetToEmptyString()
    {
        // Arrange
        var user = new User();

        // Act
        user.Email = string.Empty;

        // Assert
        Assert.Equal(string.Empty, user.Email);
    }

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name@domain.co.uk")]
    [InlineData("admin@company.org")]
    public void User_Email_AcceptsVariousFormats(string email)
    {
        // Arrange
        var user = new User();

        // Act
        user.Email = email;

        // Assert
        Assert.Equal(email, user.Email);
    }

    [Fact]
    public void User_Phone_CanBeSetAndRetrieved()
    {
        // Arrange
        var user = new User();
        var expectedPhone = "123-456-7890";

        // Act
        user.Phone = expectedPhone;

        // Assert
        Assert.Equal(expectedPhone, user.Phone);
    }

    [Fact]
    public void User_Phone_CanBeNull()
    {
        // Arrange
        var user = new User();

        // Act
        user.Phone = null;

        // Assert
        Assert.Null(user.Phone);
    }

    [Fact]
    public void User_SecretQuestion_CanBeSetAndRetrieved()
    {
        // Arrange
        var user = new User();
        var expectedQuestion = "What is your favorite color?";

        // Act
        user.SecretQuestion = expectedQuestion;

        // Assert
        Assert.Equal(expectedQuestion, user.SecretQuestion);
    }

    [Fact]
    public void User_SecretQuestion_CanBeNull()
    {
        // Arrange
        var user = new User();

        // Act
        user.SecretQuestion = null;

        // Assert
        Assert.Null(user.SecretQuestion);
    }

    [Fact]
    public void User_SecretAnswer_CanBeSetAndRetrieved()
    {
        // Arrange
        var user = new User();
        var expectedAnswer = "Blue";

        // Act
        user.SecretAnswer = expectedAnswer;

        // Assert
        Assert.Equal(expectedAnswer, user.SecretAnswer);
    }

    [Fact]
    public void User_SecretAnswer_CanBeNull()
    {
        // Arrange
        var user = new User();

        // Act
        user.SecretAnswer = null;

        // Assert
        Assert.Null(user.SecretAnswer);
    }

    [Fact]
    public void User_TypeUserId_CanBeSetAndRetrieved()
    {
        // Arrange
        var user = new User();
        var expectedTypeUserId = 5;

        // Act
        user.TypeUserId = expectedTypeUserId;

        // Assert
        Assert.Equal(expectedTypeUserId, user.TypeUserId);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    public void User_TypeUserId_AcceptsVariousValues(int typeUserId)
    {
        // Arrange
        var user = new User();

        // Act
        user.TypeUserId = typeUserId;

        // Assert
        Assert.Equal(typeUserId, user.TypeUserId);
    }

    [Fact]
    public void User_CreatedDate_CanBeSetAndRetrieved()
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
    public void User_ModifiedDate_CanBeSetAndRetrieved()
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
    public void User_ModifiedDate_CanBeNull()
    {
        // Arrange
        var user = new User();

        // Act
        user.ModifiedDate = null;

        // Assert
        Assert.Null(user.ModifiedDate);
    }

    [Fact]
    public void User_IsActive_CanBeSetToTrue()
    {
        // Arrange
        var user = new User();

        // Act
        user.IsActive = true;

        // Assert
        Assert.True(user.IsActive);
    }

    [Fact]
    public void User_IsActive_CanBeSetToFalse()
    {
        // Arrange
        var user = new User();

        // Act
        user.IsActive = false;

        // Assert
        Assert.False(user.IsActive);
    }

    [Fact]
    public void User_CreatedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var user = new User();
        var expectedCreatedBy = "system";

        // Act
        user.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, user.CreatedBy);
    }

    [Fact]
    public void User_ModifiedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var user = new User();
        var expectedModifiedBy = "admin";

        // Act
        user.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, user.ModifiedBy);
    }

    [Fact]
    public void User_ModifiedBy_CanBeNull()
    {
        // Arrange
        var user = new User();

        // Act
        user.ModifiedBy = null;

        // Assert
        Assert.Null(user.ModifiedBy);
    }

    [Fact]
    public void User_TypeUser_CanBeSetAndRetrieved()
    {
        // Arrange
        var user = new User();
        var expectedTypeUser = new TypeUser();

        // Act
        user.TypeUser = expectedTypeUser;

        // Assert
        Assert.Equal(expectedTypeUser, user.TypeUser);
    }

    [Fact]
    public void User_TypeUser_CanBeNull()
    {
        // Arrange
        var user = new User();

        // Act
        user.TypeUser = null;

        // Assert
        Assert.Null(user.TypeUser);
    }

    [Fact]
    public void User_UserRights_CanBePopulated()
    {
        // Arrange
        var user = new User();
        var userRight1 = new UserRight();
        var userRight2 = new UserRight();

        // Act
        user.UserRights = new List<UserRight> { userRight1, userRight2 };

        // Assert
        Assert.Equal(2, user.UserRights.Count);
        Assert.Contains(userRight1, user.UserRights);
        Assert.Contains(userRight2, user.UserRights);
    }

    [Fact]
    public void User_AllProperties_CanBeSetTogether()
    {
        // Arrange
        var user = new User();
        var expectedId = 999;
        var expectedUsername = "testuser";
        var expectedEmail = "test@test.com";
        var expectedPhone = "555-1234";
        var expectedSecretQuestion = "Your pet?";
        var expectedSecretAnswer = "Cat";
        var expectedTypeUserId = 3;
        var expectedCreatedDate = DateTime.Now;
        var expectedModifiedDate = DateTime.Now.AddDays(2);
        var expectedIsActive = true;
        var expectedCreatedBy = "admin";
        var expectedModifiedBy = "manager";
        var expectedTypeUser = new TypeUser { Id = 3 };

        // Act
        user.Id = expectedId;
        user.Username = expectedUsername;
        user.Email = expectedEmail;
        user.Phone = expectedPhone;
        user.SecretQuestion = expectedSecretQuestion;
        user.SecretAnswer = expectedSecretAnswer;
        user.TypeUserId = expectedTypeUserId;
        user.CreatedDate = expectedCreatedDate;
        user.ModifiedDate = expectedModifiedDate;
        user.IsActive = expectedIsActive;
        user.CreatedBy = expectedCreatedBy;
        user.ModifiedBy = expectedModifiedBy;
        user.TypeUser = expectedTypeUser;

        // Assert
        Assert.Equal(expectedId, user.Id);
        Assert.Equal(expectedUsername, user.Username);
        Assert.Equal(expectedEmail, user.Email);
        Assert.Equal(expectedPhone, user.Phone);
        Assert.Equal(expectedSecretQuestion, user.SecretQuestion);
        Assert.Equal(expectedSecretAnswer, user.SecretAnswer);
        Assert.Equal(expectedTypeUserId, user.TypeUserId);
        Assert.Equal(expectedCreatedDate, user.CreatedDate);
        Assert.Equal(expectedModifiedDate, user.ModifiedDate);
        Assert.Equal(expectedIsActive, user.IsActive);
        Assert.Equal(expectedCreatedBy, user.CreatedBy);
        Assert.Equal(expectedModifiedBy, user.ModifiedBy);
        Assert.Equal(expectedTypeUser, user.TypeUser);
    }
}
