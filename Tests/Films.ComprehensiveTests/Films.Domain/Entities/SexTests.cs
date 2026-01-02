using System;
using System.Collections.Generic;
using Xunit;
using Films.Domain.Entities;

namespace Films.Tests.Domain.Entities;

public class SexTests
{
    [Fact]
    public void Sex_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var sex = new Sex();

        // Assert
        Assert.Equal(0, sex.Id);
        Assert.Equal(string.Empty, sex.Name);
        Assert.False(sex.IsActive);
        Assert.Equal(string.Empty, sex.CreatedBy);
        Assert.Null(sex.ModifiedBy);
        Assert.NotNull(sex.Actors);
        Assert.NotNull(sex.Directors);
    }

    [Fact]
    public void Sex_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var sex = new Sex();
        var testDate = DateTime.UtcNow;

        // Act
        sex.Id = 1;
        sex.Name = "Male";
        sex.CreatedDate = testDate;
        sex.ModifiedDate = testDate;
        sex.IsActive = true;
        sex.CreatedBy = "TestUser";
        sex.ModifiedBy = "ModifiedUser";

        // Assert
        Assert.Equal(1, sex.Id);
        Assert.Equal("Male", sex.Name);
        Assert.Equal(testDate, sex.CreatedDate);
        Assert.Equal(testDate, sex.ModifiedDate);
        Assert.True(sex.IsActive);
        Assert.Equal("TestUser", sex.CreatedBy);
        Assert.Equal("ModifiedUser", sex.ModifiedBy);
    }

    [Fact]
    public void Sex_Actors_ShouldInitializeAsEmptyCollection()
    {
        // Arrange & Act
        var sex = new Sex();

        // Assert
        Assert.NotNull(sex.Actors);
        Assert.Empty(sex.Actors);
        Assert.IsAssignableFrom<ICollection<Actor>>(sex.Actors);
    }

    [Fact]
    public void Sex_Directors_ShouldInitializeAsEmptyCollection()
    {
        // Arrange & Act
        var sex = new Sex();

        // Assert
        Assert.NotNull(sex.Directors);
        Assert.Empty(sex.Directors);
        Assert.IsAssignableFrom<ICollection<Director>>(sex.Directors);
    }

    [Fact]
    public void Sex_Name_ShouldAcceptEmptyString()
    {
        // Arrange
        var sex = new Sex();

        // Act
        sex.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, sex.Name);
    }

    [Fact]
    public void Sex_ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var sex = new Sex();

        // Act
        sex.ModifiedDate = null;

        // Assert
        Assert.Null(sex.ModifiedDate);
    }
}
