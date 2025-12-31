using Xunit;
using Films.Domain.Entities;
using System;

namespace Films.Domain.Entities.Tests;

public class TypeUserTests
{
    [Fact]
    public void TypeUser_Constructor_InitializesCollections()
    {
        // Arrange & Act
        var typeUser = new TypeUser();

        // Assert
        Assert.NotNull(typeUser.Users);
        Assert.Empty(typeUser.Users);
    }

    [Fact]
    public void TypeUser_SetId_ReturnsCorrectValue()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedId = 1;

        // Act
        typeUser.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, typeUser.Id);
    }

    [Fact]
    public void TypeUser_SetName_ReturnsCorrectValue()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedName = "Administrator";

        // Act
        typeUser.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, typeUser.Name);
    }

    [Fact]
    public void TypeUser_DefaultName_IsEmptyString()
    {
        // Arrange & Act
        var typeUser = new TypeUser();

        // Assert
        Assert.Equal(string.Empty, typeUser.Name);
    }

    [Fact]
    public void TypeUser_SetDescription_ReturnsCorrectValue()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedDescription = "System administrator role";

        // Act
        typeUser.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, typeUser.Description);
    }

    [Fact]
    public void TypeUser_SetDescription_AllowsNull()
    {
        // Arrange
        var typeUser = new TypeUser();

        // Act
        typeUser.Description = null;

        // Assert
        Assert.Null(typeUser.Description);
    }

    [Fact]
    public void TypeUser_SetIsActive_ReturnsCorrectValue()
    {
        // Arrange
        var typeUser = new TypeUser();

        // Act
        typeUser.IsActive = true;

        // Assert
        Assert.True(typeUser.IsActive);
    }

    [Fact]
    public void TypeUser_SetCreatedDate_ReturnsCorrectValue()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedDate = DateTime.UtcNow;

        // Act
        typeUser.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, typeUser.CreatedDate);
    }

    [Fact]
    public void TypeUser_SetModifiedDate_ReturnsCorrectValue()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedDate = DateTime.UtcNow;

        // Act
        typeUser.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, typeUser.ModifiedDate);
    }

    [Fact]
    public void TypeUser_SetCreatedBy_ReturnsCorrectValue()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedCreatedBy = "Admin";

        // Act
        typeUser.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, typeUser.CreatedBy);
    }

    [Fact]
    public void TypeUser_DefaultCreatedBy_IsEmptyString()
    {
        // Arrange & Act
        var typeUser = new TypeUser();

        // Assert
        Assert.Equal(string.Empty, typeUser.CreatedBy);
    }

    [Fact]
    public void TypeUser_AddUser_AddsToCollection()
    {
        // Arrange
        var typeUser = new TypeUser();
        var user = new User { Id = 1, Username = "johndoe" };

        // Act
        typeUser.Users.Add(user);

        // Assert
        Assert.Single(typeUser.Users);
        Assert.Contains(user, typeUser.Users);
    }

    [Fact]
    public void TypeUser_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var typeUser = new TypeUser();
        var expectedId = 2;
        var expectedName = "Manager";
        var expectedDescription = "Manager role with limited access";
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedModifiedDate = DateTime.UtcNow.AddDays(1);
        var expectedIsActive = true;
        var expectedCreatedBy = "System";
        var expectedModifiedBy = "Admin";

        // Act
        typeUser.Id = expectedId;
        typeUser.Name = expectedName;
        typeUser.Description = expectedDescription;
        typeUser.CreatedDate = expectedCreatedDate;
        typeUser.ModifiedDate = expectedModifiedDate;
        typeUser.IsActive = expectedIsActive;
        typeUser.CreatedBy = expectedCreatedBy;
        typeUser.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, typeUser.Id);
        Assert.Equal(expectedName, typeUser.Name);
        Assert.Equal(expectedDescription, typeUser.Description);
        Assert.Equal(expectedCreatedDate, typeUser.CreatedDate);
        Assert.Equal(expectedModifiedDate, typeUser.ModifiedDate);
        Assert.Equal(expectedIsActive, typeUser.IsActive);
        Assert.Equal(expectedCreatedBy, typeUser.CreatedBy);
        Assert.Equal(expectedModifiedBy, typeUser.ModifiedBy);
    }
}
