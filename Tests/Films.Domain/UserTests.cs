using Xunit;
using Films.Domain.Entities;

namespace Films.Domain.Tests;

public class UserTests
{
    [Fact]
    public void User_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal(0, user.Id);
        Assert.Equal(string.Empty, user.Username);
        Assert.Equal(string.Empty, user.Password);
        Assert.Equal(string.Empty, user.Email);
        Assert.Null(user.TypeUserId);
        Assert.Null(user.PhoneNumber);
        Assert.Null(user.SecretQuestion);
        Assert.Null(user.SecretAnswer);
        Assert.True(user.IsActive);
        Assert.Equal("System", user.CreatedBy);
        Assert.Null(user.ModifiedBy);
        Assert.Null(user.ModifiedDate);
        Assert.Null(user.TypeUser);
        Assert.NotNull(user.UserRights);
        Assert.Empty(user.UserRights);
    }

    [Fact]
    public void User_SetProperties_SetsCorrectly()
    {
        // Arrange
        var user = new User();
        var now = DateTime.UtcNow;

        // Act
        user.Id = 1;
        user.Username = "testuser";
        user.Password = "password123";
        user.Email = "test@example.com";
        user.TypeUserId = 1;
        user.PhoneNumber = "1234567890";
        user.SecretQuestion = "What is your pet's name?";
        user.SecretAnswer = "Fluffy";
        user.CreatedDate = now;
        user.ModifiedDate = now;
        user.IsActive = false;
        user.CreatedBy = "Admin";
        user.ModifiedBy = "System";

        // Assert
        Assert.Equal(1, user.Id);
        Assert.Equal("testuser", user.Username);
        Assert.Equal("password123", user.Password);
        Assert.Equal("test@example.com", user.Email);
        Assert.Equal(1, user.TypeUserId);
        Assert.Equal("1234567890", user.PhoneNumber);
        Assert.Equal("What is your pet's name?", user.SecretQuestion);
        Assert.Equal("Fluffy", user.SecretAnswer);
        Assert.Equal(now, user.CreatedDate);
        Assert.Equal(now, user.ModifiedDate);
        Assert.False(user.IsActive);
        Assert.Equal("Admin", user.CreatedBy);
        Assert.Equal("System", user.ModifiedBy);
    }

    [Fact]
    public void User_Username_CanBeSetToEmptyString()
    {
        // Arrange
        var user = new User { Username = "Initial" };

        // Act
        user.Username = string.Empty;

        // Assert
        Assert.Equal(string.Empty, user.Username);
    }

    [Fact]
    public void User_Password_CanBeSetToEmptyString()
    {
        // Arrange
        var user = new User { Password = "Initial" };

        // Act
        user.Password = string.Empty;

        // Assert
        Assert.Equal(string.Empty, user.Password);
    }

    [Fact]
    public void User_Email_CanBeSetToEmptyString()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };

        // Act
        user.Email = string.Empty;

        // Assert
        Assert.Equal(string.Empty, user.Email);
    }

    [Fact]
    public void User_TypeUserId_CanBeNull()
    {
        // Arrange
        var user = new User { TypeUserId = 1 };

        // Act
        user.TypeUserId = null;

        // Assert
        Assert.Null(user.TypeUserId);
    }

    [Fact]
    public void User_PhoneNumber_CanBeNull()
    {
        // Arrange
        var user = new User { PhoneNumber = "1234567890" };

        // Act
        user.PhoneNumber = null;

        // Assert
        Assert.Null(user.PhoneNumber);
    }

    [Fact]
    public void User_UserRights_CanAddItems()
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
    public void User_IsActive_DefaultsToTrue()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.True(user.IsActive);
    }

    [Fact]
    public void User_TypeUser_NavigationProperty_CanBeSet()
    {
        // Arrange
        var user = new User();
        var typeUser = new TypeUser { Id = 1, Name = "Admin" };

        // Act
        user.TypeUser = typeUser;
        user.TypeUserId = typeUser.Id;

        // Assert
        Assert.NotNull(user.TypeUser);
        Assert.Equal(1, user.TypeUser.Id);
        Assert.Equal("Admin", user.TypeUser.Name);
        Assert.Equal(1, user.TypeUserId);
    }

    [Theory]
    [InlineData("user1", "pass1", "user1@test.com")]
    [InlineData("admin", "admin123", "admin@test.com")]
    [InlineData("test", "testpass", "test@example.com")]
    public void User_Credentials_AcceptsVariousValues(string username, string password, string email)
    {
        // Arrange
        var user = new User();

        // Act
        user.Username = username;
        user.Password = password;
        user.Email = email;

        // Assert
        Assert.Equal(username, user.Username);
        Assert.Equal(password, user.Password);
        Assert.Equal(email, user.Email);
    }

    [Fact]
    public void User_SecretQuestion_CanBeNull()
    {
        // Arrange
        var user = new User { SecretQuestion = "Question?" };

        // Act
        user.SecretQuestion = null;

        // Assert
        Assert.Null(user.SecretQuestion);
    }

    [Fact]
    public void User_SecretAnswer_CanBeNull()
    {
        // Arrange
        var user = new User { SecretAnswer = "Answer" };

        // Act
        user.SecretAnswer = null;

        // Assert
        Assert.Null(user.SecretAnswer);
    }
}
