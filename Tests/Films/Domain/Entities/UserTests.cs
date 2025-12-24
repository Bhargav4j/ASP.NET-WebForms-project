using System;
using System.Collections.Generic;
using Films.Domain.Entities;
using Xunit;

namespace Films.Tests.Domain.Entities
{
    public class UserTests
    {
        [Fact]
        public void User_Constructor_InitializesWithDefaultValues()
        {
            // Act
            var user = new User();

            // Assert
            Assert.Equal(0, user.Id);
            Assert.Equal(string.Empty, user.Name);
            Assert.Equal(string.Empty, user.Surname);
            Assert.Equal(string.Empty, user.Username);
            Assert.Equal(string.Empty, user.Password);
            Assert.Null(user.Email);
            Assert.Null(user.IdSex);
            Assert.Null(user.IdTypeUser);
            Assert.Equal(default(DateTime), user.CreatedDate);
            Assert.Null(user.ModifiedDate);
            Assert.True(user.IsActive);
            Assert.Equal(string.Empty, user.CreatedBy);
            Assert.Null(user.ModifiedBy);
            Assert.Null(user.Sex);
            Assert.Null(user.TypeUser);
            Assert.NotNull(user.UserRights);
            Assert.Empty(user.UserRights);
        }

        [Fact]
        public void User_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var user = new User();
            var now = DateTime.Now;
            var sex = new Sex { Id = 1, Name = "Male" };
            var typeUser = new TypeUser { Id = 1, Name = "Admin" };
            var userRights = new List<UserRight> { new UserRight() };

            // Act
            user.Id = 1;
            user.Name = "John";
            user.Surname = "Doe";
            user.Username = "johndoe";
            user.Password = "password123";
            user.Email = "john.doe@example.com";
            user.IdSex = 1;
            user.IdTypeUser = 1;
            user.CreatedDate = now;
            user.ModifiedDate = now.AddDays(1);
            user.IsActive = false;
            user.CreatedBy = "Test User";
            user.ModifiedBy = "Another User";
            user.Sex = sex;
            user.TypeUser = typeUser;
            user.UserRights = userRights;

            // Assert
            Assert.Equal(1, user.Id);
            Assert.Equal("John", user.Name);
            Assert.Equal("Doe", user.Surname);
            Assert.Equal("johndoe", user.Username);
            Assert.Equal("password123", user.Password);
            Assert.Equal("john.doe@example.com", user.Email);
            Assert.Equal(1, user.IdSex);
            Assert.Equal(1, user.IdTypeUser);
            Assert.Equal(now, user.CreatedDate);
            Assert.Equal(now.AddDays(1), user.ModifiedDate);
            Assert.False(user.IsActive);
            Assert.Equal("Test User", user.CreatedBy);
            Assert.Equal("Another User", user.ModifiedBy);
            Assert.Same(sex, user.Sex);
            Assert.Same(typeUser, user.TypeUser);
            Assert.Same(userRights, user.UserRights);
        }

        [Fact]
        public void User_NullableProperties_CanBeNull()
        {
            // Arrange
            var user = new User
            {
                Email = "test@example.com",
                IdSex = 1,
                IdTypeUser = 1,
                ModifiedDate = DateTime.Now,
                ModifiedBy = "User",
                Sex = new Sex(),
                TypeUser = new TypeUser()
            };

            // Act
            user.Email = null;
            user.IdSex = null;
            user.IdTypeUser = null;
            user.ModifiedDate = null;
            user.ModifiedBy = null;
            user.Sex = null;
            user.TypeUser = null;

            // Assert
            Assert.Null(user.Email);
            Assert.Null(user.IdSex);
            Assert.Null(user.IdTypeUser);
            Assert.Null(user.ModifiedDate);
            Assert.Null(user.ModifiedBy);
            Assert.Null(user.Sex);
            Assert.Null(user.TypeUser);
        }

        [Fact]
        public void User_RequiredStringProperties_CannotBeNull()
        {
            // Arrange & Act
            var user = new User();

            // Assert
            Assert.NotNull(user.Name);
            Assert.NotNull(user.Surname);
            Assert.NotNull(user.Username);
            Assert.NotNull(user.Password);
            Assert.NotNull(user.CreatedBy);
        }
    }
}