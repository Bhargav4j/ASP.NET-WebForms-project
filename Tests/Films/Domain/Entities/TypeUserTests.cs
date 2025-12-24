using System;
using System.Collections.Generic;
using Films.Domain.Entities;
using Xunit;

namespace Films.Tests.Domain.Entities
{
    public class TypeUserTests
    {
        [Fact]
        public void TypeUser_Constructor_InitializesWithDefaultValues()
        {
            // Act
            var typeUser = new TypeUser();

            // Assert
            Assert.Equal(0, typeUser.Id);
            Assert.Equal(string.Empty, typeUser.Name);
            Assert.Null(typeUser.Description);
            Assert.Equal(default(DateTime), typeUser.CreatedDate);
            Assert.Null(typeUser.ModifiedDate);
            Assert.True(typeUser.IsActive);
            Assert.Equal(string.Empty, typeUser.CreatedBy);
            Assert.Null(typeUser.ModifiedBy);
            Assert.NotNull(typeUser.Users);
            Assert.Empty(typeUser.Users);
        }

        [Fact]
        public void TypeUser_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var typeUser = new TypeUser();
            var now = DateTime.Now;
            var users = new List<User> { new User() };

            // Act
            typeUser.Id = 1;
            typeUser.Name = "Admin";
            typeUser.Description = "Administrator user type";
            typeUser.CreatedDate = now;
            typeUser.ModifiedDate = now.AddDays(1);
            typeUser.IsActive = false;
            typeUser.CreatedBy = "Test User";
            typeUser.ModifiedBy = "Another User";
            typeUser.Users = users;

            // Assert
            Assert.Equal(1, typeUser.Id);
            Assert.Equal("Admin", typeUser.Name);
            Assert.Equal("Administrator user type", typeUser.Description);
            Assert.Equal(now, typeUser.CreatedDate);
            Assert.Equal(now.AddDays(1), typeUser.ModifiedDate);
            Assert.False(typeUser.IsActive);
            Assert.Equal("Test User", typeUser.CreatedBy);
            Assert.Equal("Another User", typeUser.ModifiedBy);
            Assert.Same(users, typeUser.Users);
        }

        [Fact]
        public void TypeUser_NullableProperties_CanBeNull()
        {
            // Arrange
            var typeUser = new TypeUser
            {
                Description = "Test Description",
                ModifiedDate = DateTime.Now,
                ModifiedBy = "User"
            };

            // Act
            typeUser.Description = null;
            typeUser.ModifiedDate = null;
            typeUser.ModifiedBy = null;

            // Assert
            Assert.Null(typeUser.Description);
            Assert.Null(typeUser.ModifiedDate);
            Assert.Null(typeUser.ModifiedBy);
        }

        [Fact]
        public void TypeUser_UsersCollection_InitializedToEmptyList()
        {
            // Act
            var typeUser = new TypeUser();

            // Assert
            Assert.IsType<List<User>>(typeUser.Users);
            Assert.Empty(typeUser.Users);
        }
    }
}