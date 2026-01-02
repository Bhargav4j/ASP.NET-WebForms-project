using System;
using System.Collections.Generic;
using Xunit;
using Films.Domain.Entities;

namespace Films.Tests.Domain.Entities;

public class DirectorTests
{
    [Fact]
    public void Director_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var director = new Director();

        // Assert
        Assert.Equal(0, director.Id);
        Assert.Equal(string.Empty, director.FirstName);
        Assert.Equal(string.Empty, director.LastName);
        Assert.Null(director.SexId);
        Assert.False(director.IsActive);
        Assert.Equal(string.Empty, director.CreatedBy);
        Assert.Null(director.ModifiedBy);
        Assert.NotNull(director.RefDAFs);
    }

    [Fact]
    public void Director_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var director = new Director();
        var testDate = DateTime.UtcNow;

        // Act
        director.Id = 1;
        director.FirstName = "Jane";
        director.LastName = "Smith";
        director.SexId = 2;
        director.CreatedDate = testDate;
        director.ModifiedDate = testDate;
        director.IsActive = true;
        director.CreatedBy = "TestUser";
        director.ModifiedBy = "ModifiedUser";

        // Assert
        Assert.Equal(1, director.Id);
        Assert.Equal("Jane", director.FirstName);
        Assert.Equal("Smith", director.LastName);
        Assert.Equal(2, director.SexId);
        Assert.Equal(testDate, director.CreatedDate);
        Assert.Equal(testDate, director.ModifiedDate);
        Assert.True(director.IsActive);
        Assert.Equal("TestUser", director.CreatedBy);
        Assert.Equal("ModifiedUser", director.ModifiedBy);
    }

    [Fact]
    public void Director_RefDAFs_ShouldInitializeAsEmptyCollection()
    {
        // Arrange & Act
        var director = new Director();

        // Assert
        Assert.NotNull(director.RefDAFs);
        Assert.Empty(director.RefDAFs);
        Assert.IsAssignableFrom<ICollection<RefDAF>>(director.RefDAFs);
    }

    [Fact]
    public void Director_Sex_ShouldAcceptNull()
    {
        // Arrange
        var director = new Director();

        // Act
        director.Sex = null;

        // Assert
        Assert.Null(director.Sex);
    }

    [Fact]
    public void Director_SexId_ShouldAcceptNull()
    {
        // Arrange
        var director = new Director();

        // Act
        director.SexId = null;

        // Assert
        Assert.Null(director.SexId);
    }

    [Fact]
    public void Director_ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var director = new Director();

        // Act
        director.ModifiedDate = null;

        // Assert
        Assert.Null(director.ModifiedDate);
    }

    [Fact]
    public void Director_FirstName_ShouldAcceptEmptyString()
    {
        // Arrange
        var director = new Director();

        // Act
        director.FirstName = string.Empty;

        // Assert
        Assert.Equal(string.Empty, director.FirstName);
    }

    [Fact]
    public void Director_LastName_ShouldAcceptEmptyString()
    {
        // Arrange
        var director = new Director();

        // Act
        director.LastName = string.Empty;

        // Assert
        Assert.Equal(string.Empty, director.LastName);
    }
}
