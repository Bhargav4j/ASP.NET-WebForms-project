using System;
using Xunit;
using Films.Application.DTOs;

namespace Films.Tests.Application.DTOs;

public class ActorDtoTests
{
    [Fact]
    public void ActorDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var actorDto = new ActorDto();

        // Assert
        Assert.Equal(0, actorDto.Id);
        Assert.Equal(string.Empty, actorDto.FirstName);
        Assert.Equal(string.Empty, actorDto.LastName);
        Assert.Null(actorDto.SexId);
        Assert.Null(actorDto.SexName);
        Assert.False(actorDto.IsActive);
    }

    [Fact]
    public void ActorDto_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var actorDto = new ActorDto();
        var testDate = DateTime.UtcNow;

        // Act
        actorDto.Id = 1;
        actorDto.FirstName = "John";
        actorDto.LastName = "Doe";
        actorDto.SexId = 1;
        actorDto.SexName = "Male";
        actorDto.CreatedDate = testDate;
        actorDto.ModifiedDate = testDate;
        actorDto.IsActive = true;

        // Assert
        Assert.Equal(1, actorDto.Id);
        Assert.Equal("John", actorDto.FirstName);
        Assert.Equal("Doe", actorDto.LastName);
        Assert.Equal(1, actorDto.SexId);
        Assert.Equal("Male", actorDto.SexName);
        Assert.Equal(testDate, actorDto.CreatedDate);
        Assert.Equal(testDate, actorDto.ModifiedDate);
        Assert.True(actorDto.IsActive);
    }

    [Fact]
    public void ActorCreateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var actorCreateDto = new ActorCreateDto();

        // Assert
        Assert.Equal(string.Empty, actorCreateDto.FirstName);
        Assert.Equal(string.Empty, actorCreateDto.LastName);
        Assert.Null(actorCreateDto.SexId);
    }

    [Fact]
    public void ActorCreateDto_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var actorCreateDto = new ActorCreateDto();

        // Act
        actorCreateDto.FirstName = "New";
        actorCreateDto.LastName = "Actor";
        actorCreateDto.SexId = 2;

        // Assert
        Assert.Equal("New", actorCreateDto.FirstName);
        Assert.Equal("Actor", actorCreateDto.LastName);
        Assert.Equal(2, actorCreateDto.SexId);
    }

    [Fact]
    public void ActorUpdateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var actorUpdateDto = new ActorUpdateDto();

        // Assert
        Assert.Equal(string.Empty, actorUpdateDto.FirstName);
        Assert.Equal(string.Empty, actorUpdateDto.LastName);
        Assert.Null(actorUpdateDto.SexId);
        Assert.False(actorUpdateDto.IsActive);
    }

    [Fact]
    public void ActorUpdateDto_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var actorUpdateDto = new ActorUpdateDto();

        // Act
        actorUpdateDto.FirstName = "Updated";
        actorUpdateDto.LastName = "Actor";
        actorUpdateDto.SexId = 1;
        actorUpdateDto.IsActive = true;

        // Assert
        Assert.Equal("Updated", actorUpdateDto.FirstName);
        Assert.Equal("Actor", actorUpdateDto.LastName);
        Assert.Equal(1, actorUpdateDto.SexId);
        Assert.True(actorUpdateDto.IsActive);
    }
}
