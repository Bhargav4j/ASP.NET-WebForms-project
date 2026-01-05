using Films.Domain.Entities;
using Xunit;

namespace Films.UnitTests.Entities;

public class UserTests
{
    [Fact]
    public void User_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.NotNull(user);
        Assert.Equal(string.Empty, user.Username);
        Assert.Equal(string.Empty, user.Password);
        Assert.Equal(string.Empty, user.CreatedBy);
        Assert.NotNull(user.UserRights);
        Assert.Empty(user.UserRights);
    }

    [Fact]
    public void User_SetId_ShouldSetCorrectly()
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
    public void User_SetUsername_ShouldSetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedUsername = "john.doe";

        // Act
        user.Username = expectedUsername;

        // Assert
        Assert.Equal(expectedUsername, user.Username);
    }

    [Fact]
    public void User_SetPassword_ShouldSetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedPassword = "hashedpassword123";

        // Act
        user.Password = expectedPassword;

        // Assert
        Assert.Equal(expectedPassword, user.Password);
    }

    [Fact]
    public void User_SetSecretQuestion_ShouldSetCorrectly()
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
    public void User_SetSecretAnswer_ShouldSetCorrectly()
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
    public void User_SetFirstName_ShouldSetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedFirstName = "John";

        // Act
        user.FirstName = expectedFirstName;

        // Assert
        Assert.Equal(expectedFirstName, user.FirstName);
    }

    [Fact]
    public void User_SetLastName_ShouldSetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedLastName = "Doe";

        // Act
        user.LastName = expectedLastName;

        // Assert
        Assert.Equal(expectedLastName, user.LastName);
    }

    [Fact]
    public void User_SetSexId_ShouldSetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedSexId = 1;

        // Act
        user.SexId = expectedSexId;

        // Assert
        Assert.Equal(expectedSexId, user.SexId);
    }

    [Fact]
    public void User_SetBirthDate_ShouldSetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedBirthDate = new DateTime(1990, 1, 1);

        // Act
        user.BirthDate = expectedBirthDate;

        // Assert
        Assert.Equal(expectedBirthDate, user.BirthDate);
    }

    [Fact]
    public void User_SetPhoneNumber_ShouldSetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedPhoneNumber = "555-1234";

        // Act
        user.PhoneNumber = expectedPhoneNumber;

        // Assert
        Assert.Equal(expectedPhoneNumber, user.PhoneNumber);
    }

    [Fact]
    public void User_SetTypeUserId_ShouldSetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedTypeUserId = 2;

        // Act
        user.TypeUserId = expectedTypeUserId;

        // Assert
        Assert.Equal(expectedTypeUserId, user.TypeUserId);
    }

    [Fact]
    public void User_SetCreatedDate_ShouldSetCorrectly()
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
    public void User_SetModifiedDate_ShouldSetCorrectly()
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
    public void User_SetIsActive_ShouldSetCorrectly()
    {
        // Arrange
        var user = new User();

        // Act
        user.IsActive = true;

        // Assert
        Assert.True(user.IsActive);
    }

    [Fact]
    public void User_SetCreatedBy_ShouldSetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedUser = "admin";

        // Act
        user.CreatedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, user.CreatedBy);
    }

    [Fact]
    public void User_SetModifiedBy_ShouldSetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedUser = "admin";

        // Act
        user.ModifiedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, user.ModifiedBy);
    }

    [Fact]
    public void User_SetSex_ShouldSetCorrectly()
    {
        // Arrange
        var user = new User();
        var sex = new Sex { Id = 1, Name = "Male" };

        // Act
        user.Sex = sex;

        // Assert
        Assert.NotNull(user.Sex);
        Assert.Equal(1, user.Sex.Id);
        Assert.Equal("Male", user.Sex.Name);
    }

    [Fact]
    public void User_SetTypeUser_ShouldSetCorrectly()
    {
        // Arrange
        var user = new User();
        var typeUser = new TypeUser { Id = 1, Name = "Admin" };

        // Act
        user.TypeUser = typeUser;

        // Assert
        Assert.NotNull(user.TypeUser);
        Assert.Equal(1, user.TypeUser.Id);
        Assert.Equal("Admin", user.TypeUser.Name);
    }

    [Fact]
    public void User_SetUserRights_ShouldSetCorrectly()
    {
        // Arrange
        var user = new User();
        var userRights = new List<UserRight> { new UserRight { Id = 1 } };

        // Act
        user.UserRights = userRights;

        // Assert
        Assert.Equal(userRights, user.UserRights);
        Assert.Single(user.UserRights);
    }

    [Fact]
    public void User_AllProperties_ShouldWorkTogether()
    {
        // Arrange & Act
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Password = "hashed123",
            SecretQuestion = "Pet name?",
            SecretAnswer = "Fluffy",
            FirstName = "Test",
            LastName = "User",
            SexId = 1,
            BirthDate = new DateTime(1990, 5, 15),
            PhoneNumber = "555-9876",
            TypeUserId = 1,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "system",
            ModifiedBy = "admin"
        };

        // Assert
        Assert.Equal(1, user.Id);
        Assert.Equal("testuser", user.Username);
        Assert.Equal("hashed123", user.Password);
        Assert.Equal("Pet name?", user.SecretQuestion);
        Assert.Equal("Fluffy", user.SecretAnswer);
        Assert.Equal("Test", user.FirstName);
        Assert.Equal("User", user.LastName);
        Assert.Equal(1, user.SexId);
        Assert.Equal(new DateTime(1990, 5, 15), user.BirthDate);
        Assert.Equal("555-9876", user.PhoneNumber);
        Assert.Equal(1, user.TypeUserId);
        Assert.True(user.IsActive);
        Assert.Equal("system", user.CreatedBy);
        Assert.Equal("admin", user.ModifiedBy);
    }
}
