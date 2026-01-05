using Films.Domain.Entities;
using Xunit;

namespace Films.UnitTests.Entities;

public class RightTests
{
    [Fact]
    public void Right_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var right = new Right();

        // Assert
        Assert.NotNull(right);
        Assert.Equal(string.Empty, right.Name);
        Assert.Equal(string.Empty, right.CreatedBy);
        Assert.NotNull(right.UserRights);
        Assert.Empty(right.UserRights);
    }

    [Fact]
    public void Right_SetId_ShouldSetCorrectly()
    {
        // Arrange
        var right = new Right();
        var expectedId = 1;

        // Act
        right.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, right.Id);
    }

    [Fact]
    public void Right_SetName_ShouldSetCorrectly()
    {
        // Arrange
        var right = new Right();
        var expectedName = "Read";

        // Act
        right.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, right.Name);
    }

    [Fact]
    public void Right_SetDescription_ShouldSetCorrectly()
    {
        // Arrange
        var right = new Right();
        var expectedDescription = "Read permission";

        // Act
        right.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, right.Description);
    }

    [Fact]
    public void Right_SetCreatedDate_ShouldSetCorrectly()
    {
        // Arrange
        var right = new Right();
        var expectedDate = DateTime.UtcNow;

        // Act
        right.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, right.CreatedDate);
    }

    [Fact]
    public void Right_SetModifiedDate_ShouldSetCorrectly()
    {
        // Arrange
        var right = new Right();
        var expectedDate = DateTime.UtcNow;

        // Act
        right.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, right.ModifiedDate);
    }

    [Fact]
    public void Right_SetIsActive_ShouldSetCorrectly()
    {
        // Arrange
        var right = new Right();

        // Act
        right.IsActive = true;

        // Assert
        Assert.True(right.IsActive);
    }

    [Fact]
    public void Right_SetCreatedBy_ShouldSetCorrectly()
    {
        // Arrange
        var right = new Right();
        var expectedUser = "admin";

        // Act
        right.CreatedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, right.CreatedBy);
    }

    [Fact]
    public void Right_SetModifiedBy_ShouldSetCorrectly()
    {
        // Arrange
        var right = new Right();
        var expectedUser = "admin";

        // Act
        right.ModifiedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, right.ModifiedBy);
    }

    [Fact]
    public void Right_SetUserRights_ShouldSetCorrectly()
    {
        // Arrange
        var right = new Right();
        var userRights = new List<UserRight> { new UserRight { Id = 1 } };

        // Act
        right.UserRights = userRights;

        // Assert
        Assert.Equal(userRights, right.UserRights);
        Assert.Single(right.UserRights);
    }

    [Fact]
    public void Right_AllProperties_ShouldWorkTogether()
    {
        // Arrange & Act
        var right = new Right
        {
            Id = 1,
            Name = "Write",
            Description = "Write permission",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "system",
            ModifiedBy = "admin"
        };

        // Assert
        Assert.Equal(1, right.Id);
        Assert.Equal("Write", right.Name);
        Assert.Equal("Write permission", right.Description);
        Assert.True(right.IsActive);
        Assert.Equal("system", right.CreatedBy);
        Assert.Equal("admin", right.ModifiedBy);
    }
}
