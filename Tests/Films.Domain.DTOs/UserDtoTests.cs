using Xunit;
using Films.Domain.DTOs;
using System;

namespace Films.Domain.DTOs.Tests;

public class UserDtoTests
{
    [Fact]
    public void UserDto_SetId_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new UserDto();
        var expectedId = 1;

        // Act
        dto.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, dto.Id);
    }

    [Fact]
    public void UserDto_SetUsername_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new UserDto();
        var expectedUsername = "johndoe";

        // Act
        dto.Username = expectedUsername;

        // Assert
        Assert.Equal(expectedUsername, dto.Username);
    }

    [Fact]
    public void UserDto_DefaultUsername_IsEmptyString()
    {
        // Arrange & Act
        var dto = new UserDto();

        // Assert
        Assert.Equal(string.Empty, dto.Username);
    }

    [Fact]
    public void UserDto_SetEmail_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new UserDto();
        var expectedEmail = "john@example.com";

        // Act
        dto.Email = expectedEmail;

        // Assert
        Assert.Equal(expectedEmail, dto.Email);
    }

    [Fact]
    public void UserDto_DefaultEmail_IsEmptyString()
    {
        // Arrange & Act
        var dto = new UserDto();

        // Assert
        Assert.Equal(string.Empty, dto.Email);
    }

    [Fact]
    public void UserDto_SetTypeUserId_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new UserDto();
        var expectedTypeUserId = 1;

        // Act
        dto.TypeUserId = expectedTypeUserId;

        // Assert
        Assert.Equal(expectedTypeUserId, dto.TypeUserId);
    }

    [Fact]
    public void UserDto_SetTypeUserName_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new UserDto();
        var expectedTypeName = "Admin";

        // Act
        dto.TypeUserName = expectedTypeName;

        // Assert
        Assert.Equal(expectedTypeName, dto.TypeUserName);
    }

    [Fact]
    public void UserDto_SetIsActive_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new UserDto();

        // Act
        dto.IsActive = true;

        // Assert
        Assert.True(dto.IsActive);
    }

    [Fact]
    public void UserDto_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var dto = new UserDto();
        var expectedId = 10;
        var expectedUsername = "testuser";
        var expectedEmail = "test@example.com";
        var expectedTypeUserId = 2;
        var expectedTypeUserName = "Manager";
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedIsActive = true;

        // Act
        dto.Id = expectedId;
        dto.Username = expectedUsername;
        dto.Email = expectedEmail;
        dto.TypeUserId = expectedTypeUserId;
        dto.TypeUserName = expectedTypeUserName;
        dto.CreatedDate = expectedCreatedDate;
        dto.IsActive = expectedIsActive;

        // Assert
        Assert.Equal(expectedId, dto.Id);
        Assert.Equal(expectedUsername, dto.Username);
        Assert.Equal(expectedEmail, dto.Email);
        Assert.Equal(expectedTypeUserId, dto.TypeUserId);
        Assert.Equal(expectedTypeUserName, dto.TypeUserName);
        Assert.Equal(expectedCreatedDate, dto.CreatedDate);
        Assert.Equal(expectedIsActive, dto.IsActive);
    }

    [Fact]
    public void UserCreateDto_SetUsername_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new UserCreateDto();
        var expectedUsername = "newuser";

        // Act
        dto.Username = expectedUsername;

        // Assert
        Assert.Equal(expectedUsername, dto.Username);
    }

    [Fact]
    public void UserCreateDto_SetPassword_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new UserCreateDto();
        var expectedPassword = "password123";

        // Act
        dto.Password = expectedPassword;

        // Assert
        Assert.Equal(expectedPassword, dto.Password);
    }

    [Fact]
    public void UserCreateDto_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var dto = new UserCreateDto();
        var expectedUsername = "newuser";
        var expectedEmail = "new@example.com";
        var expectedPassword = "password123";
        var expectedTypeUserId = 1;

        // Act
        dto.Username = expectedUsername;
        dto.Email = expectedEmail;
        dto.Password = expectedPassword;
        dto.TypeUserId = expectedTypeUserId;

        // Assert
        Assert.Equal(expectedUsername, dto.Username);
        Assert.Equal(expectedEmail, dto.Email);
        Assert.Equal(expectedPassword, dto.Password);
        Assert.Equal(expectedTypeUserId, dto.TypeUserId);
    }

    [Fact]
    public void UserUpdateDto_SetUsername_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new UserUpdateDto();
        var expectedUsername = "updateduser";

        // Act
        dto.Username = expectedUsername;

        // Assert
        Assert.Equal(expectedUsername, dto.Username);
    }

    [Fact]
    public void UserUpdateDto_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var dto = new UserUpdateDto();
        var expectedUsername = "updateduser";
        var expectedEmail = "updated@example.com";
        var expectedTypeUserId = 2;

        // Act
        dto.Username = expectedUsername;
        dto.Email = expectedEmail;
        dto.TypeUserId = expectedTypeUserId;

        // Assert
        Assert.Equal(expectedUsername, dto.Username);
        Assert.Equal(expectedEmail, dto.Email);
        Assert.Equal(expectedTypeUserId, dto.TypeUserId);
    }
}
