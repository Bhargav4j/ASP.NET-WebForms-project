using Xunit;
using Films.Domain.DTOs;

namespace Films.Domain.Tests;

public class UserDtoTests
{
    [Fact]
    public void UserDto_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var userDto = new UserDto();

        // Assert
        Assert.NotNull(userDto);
        Assert.Equal(0, userDto.Id);
        Assert.Equal(string.Empty, userDto.Username);
        Assert.Equal(string.Empty, userDto.Email);
        Assert.Null(userDto.FirstName);
        Assert.Null(userDto.LastName);
        Assert.Null(userDto.TypeUserId);
        Assert.Null(userDto.TypeUserName);
        Assert.Equal(default(DateTime), userDto.CreatedDate);
        Assert.Null(userDto.ModifiedDate);
        Assert.False(userDto.IsActive);
    }

    [Fact]
    public void UserDto_SetProperties_ReturnsCorrectValues()
    {
        // Arrange
        var userDto = new UserDto();
        var testDate = DateTime.Now;

        // Act
        userDto.Id = 1;
        userDto.Username = "testuser";
        userDto.Email = "test@example.com";
        userDto.FirstName = "John";
        userDto.LastName = "Doe";
        userDto.TypeUserId = 1;
        userDto.TypeUserName = "Admin";
        userDto.CreatedDate = testDate;
        userDto.ModifiedDate = testDate;
        userDto.IsActive = true;

        // Assert
        Assert.Equal(1, userDto.Id);
        Assert.Equal("testuser", userDto.Username);
        Assert.Equal("test@example.com", userDto.Email);
        Assert.Equal("John", userDto.FirstName);
        Assert.Equal("Doe", userDto.LastName);
        Assert.Equal(1, userDto.TypeUserId);
        Assert.Equal("Admin", userDto.TypeUserName);
        Assert.Equal(testDate, userDto.CreatedDate);
        Assert.Equal(testDate, userDto.ModifiedDate);
        Assert.True(userDto.IsActive);
    }

    [Fact]
    public void UserDto_NullableFields_CanBeNull()
    {
        // Arrange & Act
        var userDto = new UserDto
        {
            FirstName = null,
            LastName = null,
            TypeUserId = null,
            TypeUserName = null,
            ModifiedDate = null
        };

        // Assert
        Assert.Null(userDto.FirstName);
        Assert.Null(userDto.LastName);
        Assert.Null(userDto.TypeUserId);
        Assert.Null(userDto.TypeUserName);
        Assert.Null(userDto.ModifiedDate);
    }
}

public class UserCreateDtoTests
{
    [Fact]
    public void UserCreateDto_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var userCreateDto = new UserCreateDto();

        // Assert
        Assert.NotNull(userCreateDto);
        Assert.Equal(string.Empty, userCreateDto.Username);
        Assert.Equal(string.Empty, userCreateDto.Email);
        Assert.Equal(string.Empty, userCreateDto.Password);
        Assert.Null(userCreateDto.FirstName);
        Assert.Null(userCreateDto.LastName);
        Assert.Null(userCreateDto.TypeUserId);
    }

    [Fact]
    public void UserCreateDto_SetProperties_ReturnsCorrectValues()
    {
        // Arrange
        var userCreateDto = new UserCreateDto();

        // Act
        userCreateDto.Username = "newuser";
        userCreateDto.Email = "newuser@example.com";
        userCreateDto.Password = "securepassword123";
        userCreateDto.FirstName = "Jane";
        userCreateDto.LastName = "Smith";
        userCreateDto.TypeUserId = 2;

        // Assert
        Assert.Equal("newuser", userCreateDto.Username);
        Assert.Equal("newuser@example.com", userCreateDto.Email);
        Assert.Equal("securepassword123", userCreateDto.Password);
        Assert.Equal("Jane", userCreateDto.FirstName);
        Assert.Equal("Smith", userCreateDto.LastName);
        Assert.Equal(2, userCreateDto.TypeUserId);
    }

    [Fact]
    public void UserCreateDto_NullableFields_CanBeNull()
    {
        // Arrange & Act
        var userCreateDto = new UserCreateDto
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "password",
            FirstName = null,
            LastName = null,
            TypeUserId = null
        };

        // Assert
        Assert.Null(userCreateDto.FirstName);
        Assert.Null(userCreateDto.LastName);
        Assert.Null(userCreateDto.TypeUserId);
    }

    [Fact]
    public void UserCreateDto_EmptyStrings_AreAccepted()
    {
        // Arrange & Act
        var userCreateDto = new UserCreateDto
        {
            Username = "",
            Email = "",
            Password = ""
        };

        // Assert
        Assert.Equal(string.Empty, userCreateDto.Username);
        Assert.Equal(string.Empty, userCreateDto.Email);
        Assert.Equal(string.Empty, userCreateDto.Password);
    }
}

public class UserUpdateDtoTests
{
    [Fact]
    public void UserUpdateDto_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var userUpdateDto = new UserUpdateDto();

        // Assert
        Assert.NotNull(userUpdateDto);
        Assert.Equal(string.Empty, userUpdateDto.Username);
        Assert.Equal(string.Empty, userUpdateDto.Email);
        Assert.Null(userUpdateDto.FirstName);
        Assert.Null(userUpdateDto.LastName);
        Assert.Null(userUpdateDto.TypeUserId);
    }

    [Fact]
    public void UserUpdateDto_SetProperties_ReturnsCorrectValues()
    {
        // Arrange
        var userUpdateDto = new UserUpdateDto();

        // Act
        userUpdateDto.Username = "updateduser";
        userUpdateDto.Email = "updated@example.com";
        userUpdateDto.FirstName = "UpdatedFirstName";
        userUpdateDto.LastName = "UpdatedLastName";
        userUpdateDto.TypeUserId = 3;

        // Assert
        Assert.Equal("updateduser", userUpdateDto.Username);
        Assert.Equal("updated@example.com", userUpdateDto.Email);
        Assert.Equal("UpdatedFirstName", userUpdateDto.FirstName);
        Assert.Equal("UpdatedLastName", userUpdateDto.LastName);
        Assert.Equal(3, userUpdateDto.TypeUserId);
    }

    [Fact]
    public void UserUpdateDto_NullableFields_CanBeNull()
    {
        // Arrange & Act
        var userUpdateDto = new UserUpdateDto
        {
            Username = "testuser",
            Email = "test@example.com",
            FirstName = null,
            LastName = null,
            TypeUserId = null
        };

        // Assert
        Assert.Null(userUpdateDto.FirstName);
        Assert.Null(userUpdateDto.LastName);
        Assert.Null(userUpdateDto.TypeUserId);
    }

    [Fact]
    public void UserUpdateDto_EmptyStrings_AreAccepted()
    {
        // Arrange & Act
        var userUpdateDto = new UserUpdateDto
        {
            Username = "",
            Email = ""
        };

        // Assert
        Assert.Equal(string.Empty, userUpdateDto.Username);
        Assert.Equal(string.Empty, userUpdateDto.Email);
    }
}
