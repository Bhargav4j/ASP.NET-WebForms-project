using Xunit;
using Films.Domain.Entities;

namespace Films.Domain.Tests;

public class DirectedByTests
{
    [Fact]
    public void DirectedBy_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var director = new DirectedBy();

        // Assert
        Assert.Equal(0, director.Id);
        Assert.Equal(string.Empty, director.Name);
        Assert.Null(director.Surname);
        Assert.True(director.IsActive);
        Assert.Equal("System", director.CreatedBy);
        Assert.Null(director.ModifiedBy);
        Assert.Null(director.ModifiedDate);
        Assert.NotNull(director.RefDAFs);
        Assert.Empty(director.RefDAFs);
    }

    [Fact]
    public void DirectedBy_SetProperties_SetsCorrectly()
    {
        // Arrange
        var director = new DirectedBy();
        var now = DateTime.UtcNow;

        // Act
        director.Id = 1;
        director.Name = "Steven";
        director.Surname = "Spielberg";
        director.CreatedDate = now;
        director.ModifiedDate = now;
        director.IsActive = false;
        director.CreatedBy = "Admin";
        director.ModifiedBy = "User";

        // Assert
        Assert.Equal(1, director.Id);
        Assert.Equal("Steven", director.Name);
        Assert.Equal("Spielberg", director.Surname);
        Assert.Equal(now, director.CreatedDate);
        Assert.Equal(now, director.ModifiedDate);
        Assert.False(director.IsActive);
        Assert.Equal("Admin", director.CreatedBy);
        Assert.Equal("User", director.ModifiedBy);
    }

    [Fact]
    public void DirectedBy_Name_CanBeSetToEmptyString()
    {
        // Arrange
        var director = new DirectedBy { Name = "Initial" };

        // Act
        director.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, director.Name);
    }

    [Fact]
    public void DirectedBy_Surname_CanBeNull()
    {
        // Arrange
        var director = new DirectedBy { Surname = "Test" };

        // Act
        director.Surname = null;

        // Assert
        Assert.Null(director.Surname);
    }

    [Fact]
    public void DirectedBy_RefDAFs_CanAddItems()
    {
        // Arrange
        var director = new DirectedBy();
        var refDAF = new RefDAF { Id = 1, DirectedById = 1, FilmId = 1 };

        // Act
        director.RefDAFs.Add(refDAF);

        // Assert
        Assert.Single(director.RefDAFs);
        Assert.Contains(refDAF, director.RefDAFs);
    }

    [Fact]
    public void DirectedBy_IsActive_DefaultsToTrue()
    {
        // Arrange & Act
        var director = new DirectedBy();

        // Assert
        Assert.True(director.IsActive);
    }

    [Theory]
    [InlineData("Steven", "Spielberg")]
    [InlineData("Christopher", "Nolan")]
    [InlineData("Quentin", "Tarantino")]
    [InlineData("Test", null)]
    public void DirectedBy_NameAndSurname_AcceptsVariousValues(string name, string? surname)
    {
        // Arrange
        var director = new DirectedBy();

        // Act
        director.Name = name;
        director.Surname = surname;

        // Assert
        Assert.Equal(name, director.Name);
        Assert.Equal(surname, director.Surname);
    }

    [Fact]
    public void DirectedBy_CreatedDate_CanBeSet()
    {
        // Arrange
        var director = new DirectedBy();
        var date = new DateTime(2020, 1, 1);

        // Act
        director.CreatedDate = date;

        // Assert
        Assert.Equal(date, director.CreatedDate);
    }

    [Fact]
    public void DirectedBy_ModifiedDate_CanBeNull()
    {
        // Arrange
        var director = new DirectedBy { ModifiedDate = DateTime.UtcNow };

        // Act
        director.ModifiedDate = null;

        // Assert
        Assert.Null(director.ModifiedDate);
    }
}
