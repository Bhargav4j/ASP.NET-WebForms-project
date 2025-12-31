using Xunit;
using Films.Domain.DTOs;
using System;

namespace Films.Domain.DTOs.Tests;

public class ActorDtoTests
{
    [Fact]
    public void ActorDto_SetId_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new ActorDto();
        var expectedId = 1;

        // Act
        dto.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, dto.Id);
    }

    [Fact]
    public void ActorDto_SetName_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new ActorDto();
        var expectedName = "Tom Hanks";

        // Act
        dto.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, dto.Name);
    }

    [Fact]
    public void ActorDto_DefaultName_IsEmptyString()
    {
        // Arrange & Act
        var dto = new ActorDto();

        // Assert
        Assert.Equal(string.Empty, dto.Name);
    }

    [Fact]
    public void ActorDto_SetDescription_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new ActorDto();
        var expectedDescription = "Famous actor";

        // Act
        dto.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, dto.Description);
    }

    [Fact]
    public void ActorDto_SetSexId_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new ActorDto();
        var expectedSexId = 1;

        // Act
        dto.SexId = expectedSexId;

        // Assert
        Assert.Equal(expectedSexId, dto.SexId);
    }

    [Fact]
    public void ActorDto_SetSexName_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new ActorDto();
        var expectedSexName = "Male";

        // Act
        dto.SexName = expectedSexName;

        // Assert
        Assert.Equal(expectedSexName, dto.SexName);
    }

    [Fact]
    public void ActorDto_SetIsActive_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new ActorDto();

        // Act
        dto.IsActive = true;

        // Assert
        Assert.True(dto.IsActive);
    }

    [Fact]
    public void ActorDto_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var dto = new ActorDto();
        var expectedId = 10;
        var expectedName = "Brad Pitt";
        var expectedDescription = "Award-winning actor";
        var expectedSexId = 1;
        var expectedSexName = "Male";
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedIsActive = true;

        // Act
        dto.Id = expectedId;
        dto.Name = expectedName;
        dto.Description = expectedDescription;
        dto.SexId = expectedSexId;
        dto.SexName = expectedSexName;
        dto.CreatedDate = expectedCreatedDate;
        dto.IsActive = expectedIsActive;

        // Assert
        Assert.Equal(expectedId, dto.Id);
        Assert.Equal(expectedName, dto.Name);
        Assert.Equal(expectedDescription, dto.Description);
        Assert.Equal(expectedSexId, dto.SexId);
        Assert.Equal(expectedSexName, dto.SexName);
        Assert.Equal(expectedCreatedDate, dto.CreatedDate);
        Assert.Equal(expectedIsActive, dto.IsActive);
    }

    [Fact]
    public void ActorCreateDto_SetName_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new ActorCreateDto();
        var expectedName = "New Actor";

        // Act
        dto.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, dto.Name);
    }

    [Fact]
    public void ActorCreateDto_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var dto = new ActorCreateDto();
        var expectedName = "New Actor";
        var expectedDescription = "New Description";
        var expectedSexId = 2;

        // Act
        dto.Name = expectedName;
        dto.Description = expectedDescription;
        dto.SexId = expectedSexId;

        // Assert
        Assert.Equal(expectedName, dto.Name);
        Assert.Equal(expectedDescription, dto.Description);
        Assert.Equal(expectedSexId, dto.SexId);
    }

    [Fact]
    public void ActorUpdateDto_SetName_ReturnsCorrectValue()
    {
        // Arrange
        var dto = new ActorUpdateDto();
        var expectedName = "Updated Actor";

        // Act
        dto.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, dto.Name);
    }

    [Fact]
    public void ActorUpdateDto_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var dto = new ActorUpdateDto();
        var expectedName = "Updated Actor";
        var expectedDescription = "Updated Description";
        var expectedSexId = 1;

        // Act
        dto.Name = expectedName;
        dto.Description = expectedDescription;
        dto.SexId = expectedSexId;

        // Assert
        Assert.Equal(expectedName, dto.Name);
        Assert.Equal(expectedDescription, dto.Description);
        Assert.Equal(expectedSexId, dto.SexId);
    }
}
