using Xunit;
using Films.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Films.Domain.Tests;

public class DirectorTests
{
    [Fact]
    public void Director_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var director = new Director();

        // Assert
        Assert.Equal(0, director.Id);
        Assert.Equal(string.Empty, director.Name);
        Assert.Null(director.Surname);
        Assert.Null(director.SexId);
        Assert.Null(director.Country);
        Assert.Equal(default(DateTime), director.CreatedDate);
        Assert.Null(director.ModifiedDate);
        Assert.False(director.IsActive);
        Assert.Equal(string.Empty, director.CreatedBy);
        Assert.Null(director.ModifiedBy);
        Assert.Null(director.Sex);
        Assert.NotNull(director.RefDAFs);
        Assert.Empty(director.RefDAFs);
    }

    [Fact]
    public void Director_Id_CanBeSetAndRetrieved()
    {
        // Arrange
        var director = new Director();
        var expectedId = 999;

        // Act
        director.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, director.Id);
    }

    [Fact]
    public void Director_Name_CanBeSetAndRetrieved()
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
    public void Director_Name_CanBeSetToEmptyString()
    {
        // Arrange
        var director = new Director();

        // Act
        director.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, director.Name);
    }

    [Fact]
    public void Director_Surname_CanBeSetAndRetrieved()
    {
        // Arrange
        var director = new Director();
        var expectedSurname = "Nolan";

        // Act
        director.Surname = expectedSurname;

        // Assert
        Assert.Equal(expectedSurname, director.Surname);
    }

    [Fact]
    public void Director_Surname_CanBeNull()
    {
        // Arrange
        var director = new Director();

        // Act
        director.Surname = null;

        // Assert
        Assert.Null(director.Surname);
    }

    [Fact]
    public void Director_SexId_CanBeSetAndRetrieved()
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
    public void Director_SexId_CanBeNull()
    {
        // Arrange
        var director = new Director();

        // Act
        director.SexId = null;

        // Assert
        Assert.Null(director.SexId);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Director_SexId_AcceptsVariousValues(int sexId)
    {
        // Arrange
        var director = new Director();

        // Act
        director.SexId = sexId;

        // Assert
        Assert.Equal(sexId, director.SexId);
    }

    [Fact]
    public void Director_Country_CanBeSetAndRetrieved()
    {
        // Arrange
        var director = new Director();
        var expectedCountry = "United Kingdom";

        // Act
        director.Country = expectedCountry;

        // Assert
        Assert.Equal(expectedCountry, director.Country);
    }

    [Fact]
    public void Director_Country_CanBeNull()
    {
        // Arrange
        var director = new Director();

        // Act
        director.Country = null;

        // Assert
        Assert.Null(director.Country);
    }

    [Fact]
    public void Director_CreatedDate_CanBeSetAndRetrieved()
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
    public void Director_ModifiedDate_CanBeSetAndRetrieved()
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
    public void Director_ModifiedDate_CanBeNull()
    {
        // Arrange
        var director = new Director();

        // Act
        director.ModifiedDate = null;

        // Assert
        Assert.Null(director.ModifiedDate);
    }

    [Fact]
    public void Director_IsActive_CanBeSetToTrue()
    {
        // Arrange
        var director = new Director();

        // Act
        director.IsActive = true;

        // Assert
        Assert.True(director.IsActive);
    }

    [Fact]
    public void Director_IsActive_CanBeSetToFalse()
    {
        // Arrange
        var director = new Director();

        // Act
        director.IsActive = false;

        // Assert
        Assert.False(director.IsActive);
    }

    [Fact]
    public void Director_CreatedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var director = new Director();
        var expectedCreatedBy = "admin";

        // Act
        director.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, director.CreatedBy);
    }

    [Fact]
    public void Director_ModifiedBy_CanBeSetAndRetrieved()
    {
        // Arrange
        var director = new Director();
        var expectedModifiedBy = "user123";

        // Act
        director.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, director.ModifiedBy);
    }

    [Fact]
    public void Director_ModifiedBy_CanBeNull()
    {
        // Arrange
        var director = new Director();

        // Act
        director.ModifiedBy = null;

        // Assert
        Assert.Null(director.ModifiedBy);
    }

    [Fact]
    public void Director_Sex_CanBeSetAndRetrieved()
    {
        // Arrange
        var director = new Director();
        var expectedSex = new Sex();

        // Act
        director.Sex = expectedSex;

        // Assert
        Assert.Equal(expectedSex, director.Sex);
    }

    [Fact]
    public void Director_Sex_CanBeNull()
    {
        // Arrange
        var director = new Director();

        // Act
        director.Sex = null;

        // Assert
        Assert.Null(director.Sex);
    }

    [Fact]
    public void Director_RefDAFs_CanBePopulated()
    {
        // Arrange
        var director = new Director();
        var refDAF1 = new RefDAF();
        var refDAF2 = new RefDAF();

        // Act
        director.RefDAFs = new List<RefDAF> { refDAF1, refDAF2 };

        // Assert
        Assert.Equal(2, director.RefDAFs.Count);
        Assert.Contains(refDAF1, director.RefDAFs);
        Assert.Contains(refDAF2, director.RefDAFs);
    }

    [Fact]
    public void Director_AllProperties_CanBeSetTogether()
    {
        // Arrange
        var director = new Director();
        var expectedId = 555;
        var expectedName = "Steven";
        var expectedSurname = "Spielberg";
        var expectedSexId = 1;
        var expectedCountry = "USA";
        var expectedCreatedDate = DateTime.Now;
        var expectedModifiedDate = DateTime.Now.AddDays(10);
        var expectedIsActive = true;
        var expectedCreatedBy = "admin";
        var expectedModifiedBy = "moderator";
        var expectedSex = new Sex { Id = 1 };

        // Act
        director.Id = expectedId;
        director.Name = expectedName;
        director.Surname = expectedSurname;
        director.SexId = expectedSexId;
        director.Country = expectedCountry;
        director.CreatedDate = expectedCreatedDate;
        director.ModifiedDate = expectedModifiedDate;
        director.IsActive = expectedIsActive;
        director.CreatedBy = expectedCreatedBy;
        director.ModifiedBy = expectedModifiedBy;
        director.Sex = expectedSex;

        // Assert
        Assert.Equal(expectedId, director.Id);
        Assert.Equal(expectedName, director.Name);
        Assert.Equal(expectedSurname, director.Surname);
        Assert.Equal(expectedSexId, director.SexId);
        Assert.Equal(expectedCountry, director.Country);
        Assert.Equal(expectedCreatedDate, director.CreatedDate);
        Assert.Equal(expectedModifiedDate, director.ModifiedDate);
        Assert.Equal(expectedIsActive, director.IsActive);
        Assert.Equal(expectedCreatedBy, director.CreatedBy);
        Assert.Equal(expectedModifiedBy, director.ModifiedBy);
        Assert.Equal(expectedSex, director.Sex);
    }
}
