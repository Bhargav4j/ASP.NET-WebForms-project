using Xunit;
using Films.Domain.Entities;

namespace Films.Domain.Tests;

public class SexTests
{
    [Fact]
    public void Sex_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var sex = new Sex();

        // Assert
        Assert.Equal(0, sex.Id);
        Assert.Equal(string.Empty, sex.Name);
        Assert.True(sex.IsActive);
        Assert.Equal("System", sex.CreatedBy);
        Assert.Null(sex.ModifiedBy);
        Assert.Null(sex.ModifiedDate);
        Assert.NotNull(sex.Actors);
        Assert.Empty(sex.Actors);
    }

    [Fact]
    public void Sex_SetProperties_SetsCorrectly()
    {
        // Arrange
        var sex = new Sex();
        var now = DateTime.UtcNow;

        // Act
        sex.Id = 1;
        sex.Name = "Male";
        sex.CreatedDate = now;
        sex.ModifiedDate = now;
        sex.IsActive = false;
        sex.CreatedBy = "Admin";
        sex.ModifiedBy = "User";

        // Assert
        Assert.Equal(1, sex.Id);
        Assert.Equal("Male", sex.Name);
        Assert.Equal(now, sex.CreatedDate);
        Assert.Equal(now, sex.ModifiedDate);
        Assert.False(sex.IsActive);
        Assert.Equal("Admin", sex.CreatedBy);
        Assert.Equal("User", sex.ModifiedBy);
    }

    [Fact]
    public void Sex_Name_CanBeSetToEmptyString()
    {
        // Arrange
        var sex = new Sex { Name = "Initial" };

        // Act
        sex.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, sex.Name);
    }

    [Fact]
    public void Sex_Actors_CanAddItems()
    {
        // Arrange
        var sex = new Sex();
        var actor = new Actor { Id = 1, Name = "John", SexId = 1 };

        // Act
        sex.Actors.Add(actor);

        // Assert
        Assert.Single(sex.Actors);
        Assert.Contains(actor, sex.Actors);
    }

    [Fact]
    public void Sex_IsActive_DefaultsToTrue()
    {
        // Arrange & Act
        var sex = new Sex();

        // Assert
        Assert.True(sex.IsActive);
    }

    [Theory]
    [InlineData("Male")]
    [InlineData("Female")]
    [InlineData("Other")]
    [InlineData("")]
    public void Sex_Name_AcceptsVariousValues(string name)
    {
        // Arrange
        var sex = new Sex();

        // Act
        sex.Name = name;

        // Assert
        Assert.Equal(name, sex.Name);
    }

    [Fact]
    public void Sex_CreatedDate_CanBeSet()
    {
        // Arrange
        var sex = new Sex();
        var date = new DateTime(2020, 1, 1);

        // Act
        sex.CreatedDate = date;

        // Assert
        Assert.Equal(date, sex.CreatedDate);
    }

    [Fact]
    public void Sex_ModifiedDate_CanBeNull()
    {
        // Arrange
        var sex = new Sex { ModifiedDate = DateTime.UtcNow };

        // Act
        sex.ModifiedDate = null;

        // Assert
        Assert.Null(sex.ModifiedDate);
    }

    [Fact]
    public void Sex_ModifiedBy_CanBeNull()
    {
        // Arrange
        var sex = new Sex { ModifiedBy = "User" };

        // Act
        sex.ModifiedBy = null;

        // Assert
        Assert.Null(sex.ModifiedBy);
    }
}
