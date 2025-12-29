using Xunit;
using Films.Domain.DTOs;

namespace Films.Domain.Tests;

public class ActorDtoTests
{
    [Fact]
    public void ActorDto_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var actorDto = new ActorDto();

        // Assert
        Assert.NotNull(actorDto);
        Assert.Equal(0, actorDto.Id);
        Assert.Equal(string.Empty, actorDto.Name);
        Assert.Null(actorDto.Description);
        Assert.Null(actorDto.SexId);
        Assert.Null(actorDto.SexName);
        Assert.Equal(default(DateTime), actorDto.CreatedDate);
        Assert.Null(actorDto.ModifiedDate);
        Assert.False(actorDto.IsActive);
    }

    [Fact]
    public void ActorDto_SetProperties_ReturnsCorrectValues()
    {
        // Arrange
        var actorDto = new ActorDto();
        var testDate = DateTime.Now;

        // Act
        actorDto.Id = 1;
        actorDto.Name = "Tom Hanks";
        actorDto.Description = "Academy Award-winning actor";
        actorDto.SexId = 1;
        actorDto.SexName = "Male";
        actorDto.CreatedDate = testDate;
        actorDto.ModifiedDate = testDate;
        actorDto.IsActive = true;

        // Assert
        Assert.Equal(1, actorDto.Id);
        Assert.Equal("Tom Hanks", actorDto.Name);
        Assert.Equal("Academy Award-winning actor", actorDto.Description);
        Assert.Equal(1, actorDto.SexId);
        Assert.Equal("Male", actorDto.SexName);
        Assert.Equal(testDate, actorDto.CreatedDate);
        Assert.Equal(testDate, actorDto.ModifiedDate);
        Assert.True(actorDto.IsActive);
    }

    [Fact]
    public void ActorDto_NullableFields_CanBeNull()
    {
        // Arrange & Act
        var actorDto = new ActorDto
        {
            Description = null,
            SexId = null,
            SexName = null,
            ModifiedDate = null
        };

        // Assert
        Assert.Null(actorDto.Description);
        Assert.Null(actorDto.SexId);
        Assert.Null(actorDto.SexName);
        Assert.Null(actorDto.ModifiedDate);
    }

    [Fact]
    public void ActorDto_SexId_AcceptsValidValues()
    {
        // Arrange
        var actorDto = new ActorDto();

        // Act
        actorDto.SexId = 1;
        Assert.Equal(1, actorDto.SexId);

        actorDto.SexId = 2;
        Assert.Equal(2, actorDto.SexId);
    }
}

public class ActorCreateDtoTests
{
    [Fact]
    public void ActorCreateDto_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var actorCreateDto = new ActorCreateDto();

        // Assert
        Assert.NotNull(actorCreateDto);
        Assert.Equal(string.Empty, actorCreateDto.Name);
        Assert.Null(actorCreateDto.Description);
        Assert.Null(actorCreateDto.SexId);
    }

    [Fact]
    public void ActorCreateDto_SetProperties_ReturnsCorrectValues()
    {
        // Arrange
        var actorCreateDto = new ActorCreateDto();

        // Act
        actorCreateDto.Name = "Meryl Streep";
        actorCreateDto.Description = "Three-time Oscar winner";
        actorCreateDto.SexId = 2;

        // Assert
        Assert.Equal("Meryl Streep", actorCreateDto.Name);
        Assert.Equal("Three-time Oscar winner", actorCreateDto.Description);
        Assert.Equal(2, actorCreateDto.SexId);
    }

    [Fact]
    public void ActorCreateDto_NullableFields_CanBeNull()
    {
        // Arrange & Act
        var actorCreateDto = new ActorCreateDto
        {
            Name = "Unknown Actor",
            Description = null,
            SexId = null
        };

        // Assert
        Assert.Null(actorCreateDto.Description);
        Assert.Null(actorCreateDto.SexId);
    }

    [Fact]
    public void ActorCreateDto_EmptyName_IsAccepted()
    {
        // Arrange & Act
        var actorCreateDto = new ActorCreateDto
        {
            Name = ""
        };

        // Assert
        Assert.Equal(string.Empty, actorCreateDto.Name);
    }

    [Fact]
    public void ActorCreateDto_WithMinimalData_IsValid()
    {
        // Arrange & Act
        var actorCreateDto = new ActorCreateDto
        {
            Name = "New Actor"
        };

        // Assert
        Assert.Equal("New Actor", actorCreateDto.Name);
        Assert.Null(actorCreateDto.Description);
        Assert.Null(actorCreateDto.SexId);
    }
}

public class ActorUpdateDtoTests
{
    [Fact]
    public void ActorUpdateDto_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var actorUpdateDto = new ActorUpdateDto();

        // Assert
        Assert.NotNull(actorUpdateDto);
        Assert.Equal(string.Empty, actorUpdateDto.Name);
        Assert.Null(actorUpdateDto.Description);
        Assert.Null(actorUpdateDto.SexId);
    }

    [Fact]
    public void ActorUpdateDto_SetProperties_ReturnsCorrectValues()
    {
        // Arrange
        var actorUpdateDto = new ActorUpdateDto();

        // Act
        actorUpdateDto.Name = "Robert De Niro";
        actorUpdateDto.Description = "Two-time Oscar winner";
        actorUpdateDto.SexId = 1;

        // Assert
        Assert.Equal("Robert De Niro", actorUpdateDto.Name);
        Assert.Equal("Two-time Oscar winner", actorUpdateDto.Description);
        Assert.Equal(1, actorUpdateDto.SexId);
    }

    [Fact]
    public void ActorUpdateDto_NullableFields_CanBeNull()
    {
        // Arrange & Act
        var actorUpdateDto = new ActorUpdateDto
        {
            Name = "Updated Actor",
            Description = null,
            SexId = null
        };

        // Assert
        Assert.Null(actorUpdateDto.Description);
        Assert.Null(actorUpdateDto.SexId);
    }

    [Fact]
    public void ActorUpdateDto_EmptyName_IsAccepted()
    {
        // Arrange & Act
        var actorUpdateDto = new ActorUpdateDto
        {
            Name = ""
        };

        // Assert
        Assert.Equal(string.Empty, actorUpdateDto.Name);
    }

    [Fact]
    public void ActorUpdateDto_PartialUpdate_IsSupported()
    {
        // Arrange & Act
        var actorUpdateDto = new ActorUpdateDto
        {
            Name = "Updated Name",
            SexId = 1
            // Description remains null
        };

        // Assert
        Assert.Equal("Updated Name", actorUpdateDto.Name);
        Assert.Equal(1, actorUpdateDto.SexId);
        Assert.Null(actorUpdateDto.Description);
    }
}
