using Films.Domain.Entities;
using Xunit;

namespace Films.UnitTests.Entities;

public class SexTests
{
    [Fact]
    public void Sex_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var sex = new Sex();

        // Assert
        Assert.NotNull(sex);
        Assert.Equal(string.Empty, sex.Name);
        Assert.Equal(string.Empty, sex.CreatedBy);
        Assert.NotNull(sex.Actors);
        Assert.NotNull(sex.Directors);
        Assert.NotNull(sex.Users);
        Assert.Empty(sex.Actors);
        Assert.Empty(sex.Directors);
        Assert.Empty(sex.Users);
    }

    [Fact]
    public void Sex_SetId_ShouldSetCorrectly()
    {
        // Arrange
        var sex = new Sex();
        var expectedId = 1;

        // Act
        sex.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, sex.Id);
    }

    [Fact]
    public void Sex_SetName_ShouldSetCorrectly()
    {
        // Arrange
        var sex = new Sex();
        var expectedName = "Male";

        // Act
        sex.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, sex.Name);
    }

    [Fact]
    public void Sex_SetDescription_ShouldSetCorrectly()
    {
        // Arrange
        var sex = new Sex();
        var expectedDescription = "Male gender";

        // Act
        sex.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, sex.Description);
    }

    [Fact]
    public void Sex_SetCreatedDate_ShouldSetCorrectly()
    {
        // Arrange
        var sex = new Sex();
        var expectedDate = DateTime.UtcNow;

        // Act
        sex.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, sex.CreatedDate);
    }

    [Fact]
    public void Sex_SetModifiedDate_ShouldSetCorrectly()
    {
        // Arrange
        var sex = new Sex();
        var expectedDate = DateTime.UtcNow;

        // Act
        sex.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, sex.ModifiedDate);
    }

    [Fact]
    public void Sex_SetIsActive_ShouldSetCorrectly()
    {
        // Arrange
        var sex = new Sex();

        // Act
        sex.IsActive = true;

        // Assert
        Assert.True(sex.IsActive);
    }

    [Fact]
    public void Sex_SetCreatedBy_ShouldSetCorrectly()
    {
        // Arrange
        var sex = new Sex();
        var expectedUser = "admin";

        // Act
        sex.CreatedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, sex.CreatedBy);
    }

    [Fact]
    public void Sex_SetModifiedBy_ShouldSetCorrectly()
    {
        // Arrange
        var sex = new Sex();
        var expectedUser = "admin";

        // Act
        sex.ModifiedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, sex.ModifiedBy);
    }

    [Fact]
    public void Sex_SetActors_ShouldSetCorrectly()
    {
        // Arrange
        var sex = new Sex();
        var actors = new List<Actor> { new Actor { Id = 1, Name = "Test Actor" } };

        // Act
        sex.Actors = actors;

        // Assert
        Assert.Equal(actors, sex.Actors);
        Assert.Single(sex.Actors);
    }

    [Fact]
    public void Sex_SetDirectors_ShouldSetCorrectly()
    {
        // Arrange
        var sex = new Sex();
        var directors = new List<Director> { new Director { Id = 1, Name = "Test Director" } };

        // Act
        sex.Directors = directors;

        // Assert
        Assert.Equal(directors, sex.Directors);
        Assert.Single(sex.Directors);
    }

    [Fact]
    public void Sex_SetUsers_ShouldSetCorrectly()
    {
        // Arrange
        var sex = new Sex();
        var users = new List<User> { new User { Id = 1, Username = "testuser" } };

        // Act
        sex.Users = users;

        // Assert
        Assert.Equal(users, sex.Users);
        Assert.Single(sex.Users);
    }

    [Fact]
    public void Sex_AllProperties_ShouldWorkTogether()
    {
        // Arrange & Act
        var sex = new Sex
        {
            Id = 1,
            Name = "Female",
            Description = "Female gender",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "system",
            ModifiedBy = "admin"
        };

        // Assert
        Assert.Equal(1, sex.Id);
        Assert.Equal("Female", sex.Name);
        Assert.Equal("Female gender", sex.Description);
        Assert.True(sex.IsActive);
        Assert.Equal("system", sex.CreatedBy);
        Assert.Equal("admin", sex.ModifiedBy);
    }
}
