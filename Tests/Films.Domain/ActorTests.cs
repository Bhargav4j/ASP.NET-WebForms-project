using Xunit;
using Films.Domain.Entities;

namespace Films.Domain.Tests;

public class ActorTests
{
    [Fact]
    public void Actor_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var actor = new Actor();

        // Assert
        Assert.Equal(0, actor.Id);
        Assert.Equal(string.Empty, actor.Name);
        Assert.Null(actor.Surname);
        Assert.Null(actor.SexId);
        Assert.Null(actor.BirthDate);
        Assert.True(actor.IsActive);
        Assert.Equal("System", actor.CreatedBy);
        Assert.Null(actor.ModifiedBy);
        Assert.Null(actor.ModifiedDate);
        Assert.Null(actor.Sex);
        Assert.NotNull(actor.RefAFs);
        Assert.Empty(actor.RefAFs);
    }

    [Fact]
    public void Actor_SetProperties_SetsCorrectly()
    {
        // Arrange
        var actor = new Actor();
        var birthDate = new DateTime(1990, 5, 15);
        var now = DateTime.UtcNow;

        // Act
        actor.Id = 1;
        actor.Name = "John";
        actor.Surname = "Doe";
        actor.SexId = 1;
        actor.BirthDate = birthDate;
        actor.CreatedDate = now;
        actor.ModifiedDate = now;
        actor.IsActive = false;
        actor.CreatedBy = "Admin";
        actor.ModifiedBy = "User";

        // Assert
        Assert.Equal(1, actor.Id);
        Assert.Equal("John", actor.Name);
        Assert.Equal("Doe", actor.Surname);
        Assert.Equal(1, actor.SexId);
        Assert.Equal(birthDate, actor.BirthDate);
        Assert.Equal(now, actor.CreatedDate);
        Assert.Equal(now, actor.ModifiedDate);
        Assert.False(actor.IsActive);
        Assert.Equal("Admin", actor.CreatedBy);
        Assert.Equal("User", actor.ModifiedBy);
    }

    [Fact]
    public void Actor_Name_CanBeSetToEmptyString()
    {
        // Arrange
        var actor = new Actor { Name = "Initial" };

        // Act
        actor.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, actor.Name);
    }

    [Fact]
    public void Actor_Surname_CanBeNull()
    {
        // Arrange
        var actor = new Actor { Surname = "Test" };

        // Act
        actor.Surname = null;

        // Assert
        Assert.Null(actor.Surname);
    }

    [Fact]
    public void Actor_SexId_CanBeNull()
    {
        // Arrange
        var actor = new Actor { SexId = 1 };

        // Act
        actor.SexId = null;

        // Assert
        Assert.Null(actor.SexId);
    }

    [Fact]
    public void Actor_BirthDate_CanBeNull()
    {
        // Arrange
        var actor = new Actor { BirthDate = DateTime.UtcNow };

        // Act
        actor.BirthDate = null;

        // Assert
        Assert.Null(actor.BirthDate);
    }

    [Fact]
    public void Actor_RefAFs_CanAddItems()
    {
        // Arrange
        var actor = new Actor();
        var refAF = new RefAF { Id = 1, ActorId = 1, FilmId = 1 };

        // Act
        actor.RefAFs.Add(refAF);

        // Assert
        Assert.Single(actor.RefAFs);
        Assert.Contains(refAF, actor.RefAFs);
    }

    [Fact]
    public void Actor_IsActive_DefaultsToTrue()
    {
        // Arrange & Act
        var actor = new Actor();

        // Assert
        Assert.True(actor.IsActive);
    }

    [Fact]
    public void Actor_Sex_NavigationProperty_CanBeSet()
    {
        // Arrange
        var actor = new Actor();
        var sex = new Sex { Id = 1, Name = "Male" };

        // Act
        actor.Sex = sex;
        actor.SexId = sex.Id;

        // Assert
        Assert.NotNull(actor.Sex);
        Assert.Equal(1, actor.Sex.Id);
        Assert.Equal("Male", actor.Sex.Name);
        Assert.Equal(1, actor.SexId);
    }

    [Theory]
    [InlineData("John", "Doe")]
    [InlineData("Jane", "Smith")]
    [InlineData("Test", null)]
    public void Actor_NameAndSurname_AcceptsVariousValues(string name, string? surname)
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.Name = name;
        actor.Surname = surname;

        // Assert
        Assert.Equal(name, actor.Name);
        Assert.Equal(surname, actor.Surname);
    }
}
