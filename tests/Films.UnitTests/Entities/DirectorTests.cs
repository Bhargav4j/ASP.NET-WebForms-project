using Films.Domain.Entities;
using Xunit;

namespace Films.UnitTests.Entities;

public class DirectorTests
{
    [Fact]
    public void Director_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var director = new Director();

        // Assert
        Assert.NotNull(director);
        Assert.Equal(string.Empty, director.Name);
        Assert.Equal(string.Empty, director.CreatedBy);
        Assert.NotNull(director.RefDAFs);
        Assert.Empty(director.RefDAFs);
    }

    [Fact]
    public void Director_SetId_ShouldSetCorrectly()
    {
        // Arrange
        var director = new Director();
        var expectedId = 5;

        // Act
        director.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, director.Id);
    }

    [Fact]
    public void Director_SetName_ShouldSetCorrectly()
    {
        // Arrange
        var director = new Director();
        var expectedName = "Christopher Nolan";

        // Act
        director.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, director.Name);
    }

    [Fact]
    public void Director_SetDescription_ShouldSetCorrectly()
    {
        // Arrange
        var director = new Director();
        var expectedDescription = "Renowned film director";

        // Act
        director.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, director.Description);
    }

    [Fact]
    public void Director_SetSexId_ShouldSetCorrectly()
    {
        // Arrange
        var director = new Director();
        var expectedSexId = 1;

        // Act
        director.SexId = expectedSexId;

        // Assert
        Assert.Equal(expectedSexId, director.SexId);
    }

    [Fact]
    public void Director_SetBirthDate_ShouldSetCorrectly()
    {
        // Arrange
        var director = new Director();
        var expectedBirthDate = new DateTime(1970, 7, 30);

        // Act
        director.BirthDate = expectedBirthDate;

        // Assert
        Assert.Equal(expectedBirthDate, director.BirthDate);
    }

    [Fact]
    public void Director_SetCreatedDate_ShouldSetCorrectly()
    {
        // Arrange
        var director = new Director();
        var expectedDate = DateTime.UtcNow;

        // Act
        director.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, director.CreatedDate);
    }

    [Fact]
    public void Director_SetModifiedDate_ShouldSetCorrectly()
    {
        // Arrange
        var director = new Director();
        var expectedDate = DateTime.UtcNow;

        // Act
        director.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, director.ModifiedDate);
    }

    [Fact]
    public void Director_SetIsActive_ShouldSetCorrectly()
    {
        // Arrange
        var director = new Director();

        // Act
        director.IsActive = true;

        // Assert
        Assert.True(director.IsActive);
    }

    [Fact]
    public void Director_SetCreatedBy_ShouldSetCorrectly()
    {
        // Arrange
        var director = new Director();
        var expectedUser = "admin";

        // Act
        director.CreatedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, director.CreatedBy);
    }

    [Fact]
    public void Director_SetModifiedBy_ShouldSetCorrectly()
    {
        // Arrange
        var director = new Director();
        var expectedUser = "admin";

        // Act
        director.ModifiedBy = expectedUser;

        // Assert
        Assert.Equal(expectedUser, director.ModifiedBy);
    }

    [Fact]
    public void Director_SetSex_ShouldSetCorrectly()
    {
        // Arrange
        var director = new Director();
        var sex = new Sex { Id = 1, Name = "Male" };

        // Act
        director.Sex = sex;

        // Assert
        Assert.NotNull(director.Sex);
        Assert.Equal(1, director.Sex.Id);
        Assert.Equal("Male", director.Sex.Name);
    }

    [Fact]
    public void Director_SetRefDAFs_ShouldSetCorrectly()
    {
        // Arrange
        var director = new Director();
        var refDAFs = new List<RefDAF> { new RefDAF { Id = 1 } };

        // Act
        director.RefDAFs = refDAFs;

        // Assert
        Assert.Equal(refDAFs, director.RefDAFs);
        Assert.Single(director.RefDAFs);
    }

    [Fact]
    public void Director_AllProperties_ShouldWorkTogether()
    {
        // Arrange & Act
        var director = new Director
        {
            Id = 1,
            Name = "Steven Spielberg",
            Description = "Legendary director",
            SexId = 1,
            BirthDate = new DateTime(1946, 12, 18),
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "system",
            ModifiedBy = "admin"
        };

        // Assert
        Assert.Equal(1, director.Id);
        Assert.Equal("Steven Spielberg", director.Name);
        Assert.Equal("Legendary director", director.Description);
        Assert.Equal(1, director.SexId);
        Assert.Equal(new DateTime(1946, 12, 18), director.BirthDate);
        Assert.True(director.IsActive);
        Assert.Equal("system", director.CreatedBy);
        Assert.Equal("admin", director.ModifiedBy);
    }
}
