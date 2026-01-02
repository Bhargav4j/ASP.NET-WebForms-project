using System;
using Xunit;
using Films.Application.DTOs;

namespace Films.Tests.Application.DTOs;

public class DirectorDtoTests
{
    [Fact]
    public void DirectorDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var directorDto = new DirectorDto();

        // Assert
        Assert.Equal(0, directorDto.Id);
        Assert.Equal(string.Empty, directorDto.FirstName);
        Assert.Equal(string.Empty, directorDto.LastName);
        Assert.Null(directorDto.SexId);
        Assert.Null(directorDto.SexName);
        Assert.False(directorDto.IsActive);
    }

    [Fact]
    public void DirectorDto_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var directorDto = new DirectorDto();
        var testDate = DateTime.UtcNow;

        // Act
        directorDto.Id = 1;
        directorDto.FirstName = "Jane";
        directorDto.LastName = "Smith";
        directorDto.SexId = 2;
        directorDto.SexName = "Female";
        directorDto.CreatedDate = testDate;
        directorDto.ModifiedDate = testDate;
        directorDto.IsActive = true;

        // Assert
        Assert.Equal(1, directorDto.Id);
        Assert.Equal("Jane", directorDto.FirstName);
        Assert.Equal("Smith", directorDto.LastName);
        Assert.Equal(2, directorDto.SexId);
        Assert.Equal("Female", directorDto.SexName);
        Assert.Equal(testDate, directorDto.CreatedDate);
        Assert.Equal(testDate, directorDto.ModifiedDate);
        Assert.True(directorDto.IsActive);
    }

    [Fact]
    public void DirectorCreateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var directorCreateDto = new DirectorCreateDto();

        // Assert
        Assert.Equal(string.Empty, directorCreateDto.FirstName);
        Assert.Equal(string.Empty, directorCreateDto.LastName);
        Assert.Null(directorCreateDto.SexId);
    }

    [Fact]
    public void DirectorCreateDto_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var directorCreateDto = new DirectorCreateDto();

        // Act
        directorCreateDto.FirstName = "New";
        directorCreateDto.LastName = "Director";
        directorCreateDto.SexId = 1;

        // Assert
        Assert.Equal("New", directorCreateDto.FirstName);
        Assert.Equal("Director", directorCreateDto.LastName);
        Assert.Equal(1, directorCreateDto.SexId);
    }

    [Fact]
    public void DirectorUpdateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var directorUpdateDto = new DirectorUpdateDto();

        // Assert
        Assert.Equal(string.Empty, directorUpdateDto.FirstName);
        Assert.Equal(string.Empty, directorUpdateDto.LastName);
        Assert.Null(directorUpdateDto.SexId);
        Assert.False(directorUpdateDto.IsActive);
    }

    [Fact]
    public void DirectorUpdateDto_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var directorUpdateDto = new DirectorUpdateDto();

        // Act
        directorUpdateDto.FirstName = "Updated";
        directorUpdateDto.LastName = "Director";
        directorUpdateDto.SexId = 2;
        directorUpdateDto.IsActive = true;

        // Assert
        Assert.Equal("Updated", directorUpdateDto.FirstName);
        Assert.Equal("Director", directorUpdateDto.LastName);
        Assert.Equal(2, directorUpdateDto.SexId);
        Assert.True(directorUpdateDto.IsActive);
    }
}
