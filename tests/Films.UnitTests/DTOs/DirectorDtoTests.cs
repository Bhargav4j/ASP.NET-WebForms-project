using Films.Domain.DTOs;
using Xunit;

namespace Films.UnitTests.DTOs;

public class DirectorDtoTests
{
    [Fact]
    public void DirectorDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var dto = new DirectorDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.Name);
    }

    [Fact]
    public void DirectorDto_AllProperties_ShouldSetCorrectly()
    {
        // Arrange & Act
        var dto = new DirectorDto
        {
            Id = 1,
            Name = "Test Director",
            Description = "Test Description",
            SexId = 1,
            SexName = "Male",
            BirthDate = new DateTime(1975, 3, 15),
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true
        };

        // Assert
        Assert.Equal(1, dto.Id);
        Assert.Equal("Test Director", dto.Name);
        Assert.Equal("Test Description", dto.Description);
        Assert.Equal(1, dto.SexId);
        Assert.Equal("Male", dto.SexName);
        Assert.Equal(new DateTime(1975, 3, 15), dto.BirthDate);
        Assert.True(dto.IsActive);
    }

    [Fact]
    public void DirectorCreateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var dto = new DirectorCreateDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.Name);
    }

    [Fact]
    public void DirectorCreateDto_AllProperties_ShouldSetCorrectly()
    {
        // Arrange & Act
        var dto = new DirectorCreateDto
        {
            Name = "New Director",
            Description = "New Description",
            SexId = 1,
            BirthDate = new DateTime(1980, 7, 20)
        };

        // Assert
        Assert.Equal("New Director", dto.Name);
        Assert.Equal("New Description", dto.Description);
        Assert.Equal(1, dto.SexId);
        Assert.Equal(new DateTime(1980, 7, 20), dto.BirthDate);
    }

    [Fact]
    public void DirectorUpdateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var dto = new DirectorUpdateDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.Name);
    }

    [Fact]
    public void DirectorUpdateDto_AllProperties_ShouldSetCorrectly()
    {
        // Arrange & Act
        var dto = new DirectorUpdateDto
        {
            Name = "Updated Director",
            Description = "Updated Description",
            SexId = 2,
            BirthDate = new DateTime(1970, 11, 30)
        };

        // Assert
        Assert.Equal("Updated Director", dto.Name);
        Assert.Equal("Updated Description", dto.Description);
        Assert.Equal(2, dto.SexId);
        Assert.Equal(new DateTime(1970, 11, 30), dto.BirthDate);
    }
}
