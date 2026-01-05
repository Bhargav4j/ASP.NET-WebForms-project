using Films.Domain.Entities;
using Xunit;

namespace Films.UnitTests.Entities;

public class ActorTests
{
    [Fact]
    public void Actor_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var actor = new Actor();

        // Assert
        Assert.NotNull(actor);
        Assert.Equal(string.Empty, actor.Name);
        Assert.Equal(string.Empty, actor.CreatedBy);
        Assert.NotNull(actor.RefAFs);
        Assert.Empty(actor.RefAFs);
    }

    [Fact]
    public void Actor_SetId_ShouldSetCorrectly()
    {
        // Arrange
        var actor = new Actor();
        var expectedId = 10;

        // Act
        actor.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, actor.Id);
    }

    [Fact]
    public void Actor_SetName_ShouldSetCorrectly()
    {
        // Arrange
        var actor = new Actor();
        var expectedName = "Leonardo DiCaprio";

        // Act
        actor.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, actor.Name);
    }

    [Fact]
    public void Actor_SetDescription_ShouldSetCorrectly()
    {
        // Arrange
        var actor = new Actor();
        var expectedDescription = "Award-winning actor";

        // Act
        actor.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, actor.Description);
    }

    [Fact]
    public void Actor_SetSexId_ShouldSetCorrectly()
    {
        // Arrange
        var actor = new Actor();
        var expectedSexId = 1;

        // Act
        actor.SexId = expectedSexId;

        // Assert
        Assert.Equal(expectedSexId, actor.SexId);
    }

    [Fact]
    public void Actor_SetBirthDate_ShouldSetCorrectly()
    {
        // Arrange
        var actor = new Actor();
        var expectedBirthDate = new DateTime(1974, 11, 11);

        // Act
        actor.BirthDate = expectedBirthDate;

        // Assert
        Assert.Equal(expectedBirthDate, actor.BirthDate);
    }

    [Fact]
    public void Actor_SetCreatedDate_ShouldSetCorrectly()
    {
        // Arrange
        var actor = new Actor();
        var expectedDate = DateTime.UtcNow;

        // Act
        actor.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, actor.CreatedDate);
    }

    [Fact]
    public void Actor_SetModifiedDate_ShouldSetCorrectly()
    {
        // Arrange
        var actor = new Actor();
        var expectedDate = DateTime.UtcNow;

        // Act
        actor.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, actor.ModifiedDate);
    }

    [Fact]
    public void Actor_SetIsActive_ShouldSetCorrectly()
    {
        // Arrange
        var actor = new Actor();

        // Act
        actor.IsActive = true;

        // Assert
        Assert.True(actor.IsActive);
    }

    [Fact]
    public void Actor_SetCreatedBy_ShouldSetCorrectly()
    {
        // Arrange
        var actor = new Actor();
        var expectedUser = "admin";

        // Act
        actor.CreatedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, actor.CreatedBy);
    }

    [Fact]
    public void Actor_SetModifiedBy_ShouldSetCorrectly()
    {
        // Arrange
        var actor = new Actor();
        var expectedUser = "admin";

        // Act
        actor.ModifiedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, actor.ModifiedBy);
    }

    [Fact]
    public void Actor_SetSex_ShouldSetCorrectly()
    {
        // Arrange
        var actor = new Actor();
        var sex = new Sex { Id = 1, Name = "Male" };

        // Act
        actor.Sex = sex;

        // Assert
        Assert.NotNull(actor.Sex);
        Assert.Equal(1, actor.Sex.Id);
        Assert.Equal("Male", actor.Sex.Name);
    }

    [Fact]
    public void Actor_SetRefAFs_ShouldSetCorrectly()
    {
        // Arrange
        var actor = new Actor();
        var refAFs = new List<RefAF> { new RefAF { Id = 1 } };

        // Act
        actor.RefAFs = refAFs;

        // Assert
        Assert.Equal(refAFs, actor.RefAFs);
        Assert.Single(actor.RefAFs);
    }

    [Fact]
    public void Actor_AllProperties_ShouldWorkTogether()
    {
        // Arrange & Act
        var actor = new Actor
        {
            Id = 1,
            Name = "Tom Hanks",
            Description = "Acclaimed actor",
            SexId = 1,
            BirthDate = new DateTime(1956, 7, 9),
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "system",
            ModifiedBy = "admin"
        };

        // Assert
        Assert.Equal(1, actor.Id);
        Assert.Equal("Tom Hanks", actor.Name);
        Assert.Equal("Acclaimed actor", actor.Description);
        Assert.Equal(1, actor.SexId);
        Assert.Equal(new DateTime(1956, 7, 9), actor.BirthDate);
        Assert.True(actor.IsActive);
        Assert.Equal("system", actor.CreatedBy);
        Assert.Equal("admin", actor.ModifiedBy);
    }
}
