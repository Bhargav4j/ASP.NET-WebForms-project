using System;
using System.Collections.Generic;
using Xunit;
using Films.Domain.Entities;

namespace Films.Tests.Domain.Entities;

public class TypeUserTests
{
    [Fact]
    public void TypeUser_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var typeUser = new TypeUser();

        // Assert
        Assert.Equal(0, typeUser.Id);
        Assert.Equal(string.Empty, typeUser.Name);
        Assert.False(typeUser.IsActive);
        Assert.Equal(string.Empty, typeUser.CreatedBy);
        Assert.Null(typeUser.ModifiedBy);
        Assert.NotNull(typeUser.Users);
    }

    [Fact]
    public void TypeUser_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var typeUser = new TypeUser();
        var testDate = DateTime.UtcNow;

        // Act
        typeUser.Id = 1;
        typeUser.Name = "Admin";
        typeUser.CreatedDate = testDate;
        typeUser.ModifiedDate = testDate;
        typeUser.IsActive = true;
        typeUser.CreatedBy = "TestUser";
        typeUser.ModifiedBy = "ModifiedUser";

        // Assert
        Assert.Equal(1, typeUser.Id);
        Assert.Equal("Admin", typeUser.Name);
        Assert.Equal(testDate, typeUser.CreatedDate);
        Assert.Equal(testDate, typeUser.ModifiedDate);
        Assert.True(typeUser.IsActive);
        Assert.Equal("TestUser", typeUser.CreatedBy);
        Assert.Equal("ModifiedUser", typeUser.ModifiedBy);
    }

    [Fact]
    public void TypeUser_Users_ShouldInitializeAsEmptyCollection()
    {
        // Arrange & Act
        var typeUser = new TypeUser();

        // Assert
        Assert.NotNull(typeUser.Users);
        Assert.Empty(typeUser.Users);
        Assert.IsAssignableFrom<ICollection<User>>(typeUser.Users);
    }

    [Fact]
    public void TypeUser_Name_ShouldAcceptEmptyString()
    {
        // Arrange
        var typeUser = new TypeUser();

        // Act
        typeUser.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, typeUser.Name);
    }

    [Fact]
    public void TypeUser_ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var typeUser = new TypeUser();

        // Act
        typeUser.ModifiedDate = null;

        // Assert
        Assert.Null(typeUser.ModifiedDate);
    }
}
