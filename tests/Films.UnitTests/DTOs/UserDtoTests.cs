using Films.Domain.DTOs;
using Xunit;

namespace Films.UnitTests.DTOs;

public class UserDtoTests
{
    [Fact]
    public void UserDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var dto = new UserDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.Username);
    }

    [Fact]
    public void UserDto_AllProperties_ShouldSetCorrectly()
    {
        // Arrange & Act
        var dto = new UserDto
        {
            Id = 1,
            Username = "testuser",
            FirstName = "John",
            LastName = "Doe",
            SexId = 1,
            SexName = "Male",
            BirthDate = new DateTime(1990, 5, 10),
            PhoneNumber = "555-1234",
            TypeUserId = 1,
            TypeUserName = "Admin",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true
        };

        // Assert
        Assert.Equal(1, dto.Id);
        Assert.Equal("testuser", dto.Username);
        Assert.Equal("John", dto.FirstName);
        Assert.Equal("Doe", dto.LastName);
        Assert.Equal(1, dto.SexId);
        Assert.Equal("Male", dto.SexName);
        Assert.Equal(new DateTime(1990, 5, 10), dto.BirthDate);
        Assert.Equal("555-1234", dto.PhoneNumber);
        Assert.Equal(1, dto.TypeUserId);
        Assert.Equal("Admin", dto.TypeUserName);
        Assert.True(dto.IsActive);
    }

    [Fact]
    public void UserCreateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var dto = new UserCreateDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.Username);
        Assert.Equal(string.Empty, dto.Password);
    }

    [Fact]
    public void UserCreateDto_AllProperties_ShouldSetCorrectly()
    {
        // Arrange & Act
        var dto = new UserCreateDto
        {
            Username = "newuser",
            Password = "password123",
            SecretQuestion = "Pet name?",
            SecretAnswer = "Fluffy",
            FirstName = "Jane",
            LastName = "Smith",
            SexId = 2,
            BirthDate = new DateTime(1995, 8, 25),
            PhoneNumber = "555-5678"
        };

        // Assert
        Assert.Equal("newuser", dto.Username);
        Assert.Equal("password123", dto.Password);
        Assert.Equal("Pet name?", dto.SecretQuestion);
        Assert.Equal("Fluffy", dto.SecretAnswer);
        Assert.Equal("Jane", dto.FirstName);
        Assert.Equal("Smith", dto.LastName);
        Assert.Equal(2, dto.SexId);
        Assert.Equal(new DateTime(1995, 8, 25), dto.BirthDate);
        Assert.Equal("555-5678", dto.PhoneNumber);
    }

    [Fact]
    public void UserUpdateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var dto = new UserUpdateDto();

        // Assert
        Assert.NotNull(dto);
    }

    [Fact]
    public void UserUpdateDto_AllProperties_ShouldSetCorrectly()
    {
        // Arrange & Act
        var dto = new UserUpdateDto
        {
            FirstName = "Updated",
            LastName = "Name",
            SexId = 1,
            BirthDate = new DateTime(1992, 3, 18),
            PhoneNumber = "555-9999"
        };

        // Assert
        Assert.Equal("Updated", dto.FirstName);
        Assert.Equal("Name", dto.LastName);
        Assert.Equal(1, dto.SexId);
        Assert.Equal(new DateTime(1992, 3, 18), dto.BirthDate);
        Assert.Equal("555-9999", dto.PhoneNumber);
    }
}
