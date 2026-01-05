using Films.Domain.DTOs;
using Xunit;

namespace Films.UnitTests.DTOs;

public class ActorDtoTests
{
    [Fact]
    public void ActorDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var dto = new ActorDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.Name);
    }

    [Fact]
    public void ActorDto_AllProperties_ShouldSetCorrectly()
    {
        // Arrange & Act
        var dto = new ActorDto
        {
            Id = 1,
            Name = "Test Actor",
            Description = "Test Description",
            SexId = 1,
            SexName = "Male",
            BirthDate = new DateTime(1980, 1, 1),
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true
        };

        // Assert
        Assert.Equal(1, dto.Id);
        Assert.Equal("Test Actor", dto.Name);
        Assert.Equal("Test Description", dto.Description);
        Assert.Equal(1, dto.SexId);
        Assert.Equal("Male", dto.SexName);
        Assert.Equal(new DateTime(1980, 1, 1), dto.BirthDate);
        Assert.True(dto.IsActive);
    }

    [Fact]
    public void ActorCreateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var dto = new ActorCreateDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.Name);
    }

    [Fact]
    public void ActorCreateDto_AllProperties_ShouldSetCorrectly()
    {
        // Arrange & Act
        var dto = new ActorCreateDto
        {
            Name = "New Actor",
            Description = "New Description",
            SexId = 1,
            BirthDate = new DateTime(1990, 5, 15)
        };

        // Assert
        Assert.Equal("New Actor", dto.Name);
        Assert.Equal("New Description", dto.Description);
        Assert.Equal(1, dto.SexId);
        Assert.Equal(new DateTime(1990, 5, 15), dto.BirthDate);
    }

    [Fact]
    public void ActorUpdateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var dto = new ActorUpdateDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.Name);
    }

    [Fact]
    public void ActorUpdateDto_AllProperties_ShouldSetCorrectly()
    {
        // Arrange & Act
        var dto = new ActorUpdateDto
        {
            Name = "Updated Actor",
            Description = "Updated Description",
            SexId = 2,
            BirthDate = new DateTime(1985, 10, 20)
        };

        // Assert
        Assert.Equal("Updated Actor", dto.Name);
        Assert.Equal("Updated Description", dto.Description);
        Assert.Equal(2, dto.SexId);
        Assert.Equal(new DateTime(1985, 10, 20), dto.BirthDate);
    }
}
