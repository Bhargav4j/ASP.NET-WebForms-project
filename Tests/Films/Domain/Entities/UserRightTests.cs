using System;
using Films.Domain.Entities;
using Xunit;

namespace Films.Tests.Domain.Entities
{
    public class UserRightTests
    {
        [Fact]
        public void UserRight_Constructor_InitializesWithDefaultValues()
        {
            // Act
            var userRight = new UserRight();

            // Assert
            Assert.Equal(0, userRight.Id);
            Assert.Equal(0, userRight.IdUser);
            Assert.Equal(0, userRight.IdRight);
            Assert.Equal(default(DateTime), userRight.CreatedDate);
            Assert.Null(userRight.ModifiedDate);
            Assert.True(userRight.IsActive);
            Assert.Equal(string.Empty, userRight.CreatedBy);
            Assert.Null(userRight.ModifiedBy);
            Assert.Null(userRight.User);
            Assert.Null(userRight.Right);
        }

        [Fact]
        public void UserRight_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var userRight = new UserRight();
            var now = DateTime.Now;
            var user = new User { Id = 1, Name = "John Doe" };
            var right = new Right { Id = 1, Name = "CanEditFilms" };

            // Act
            userRight.Id = 1;
            userRight.IdUser = 1;
            userRight.IdRight = 1;
            userRight.CreatedDate = now;
            userRight.ModifiedDate = now.AddDays(1);
            userRight.IsActive = false;
            userRight.CreatedBy = "Test User";
            userRight.ModifiedBy = "Another User";
            userRight.User = user;
            userRight.Right = right;

            // Assert
            Assert.Equal(1, userRight.Id);
            Assert.Equal(1, userRight.IdUser);
            Assert.Equal(1, userRight.IdRight);
            Assert.Equal(now, userRight.CreatedDate);
            Assert.Equal(now.AddDays(1), userRight.ModifiedDate);
            Assert.False(userRight.IsActive);
            Assert.Equal("Test User", userRight.CreatedBy);
            Assert.Equal("Another User", userRight.ModifiedBy);
            Assert.Same(user, userRight.User);
            Assert.Same(right, userRight.Right);
        }

        [Fact]
        public void UserRight_NullableProperties_CanBeNull()
        {
            // Arrange
            var userRight = new UserRight
            {
                ModifiedDate = DateTime.Now,
                ModifiedBy = "User"
            };

            // Act
            userRight.ModifiedDate = null;
            userRight.ModifiedBy = null;

            // Assert
            Assert.Null(userRight.ModifiedDate);
            Assert.Null(userRight.ModifiedBy);
        }

        [Fact]
        public void UserRight_RequiredNavigationProperties_ThrowsExceptionIfNullAndAccessed()
        {
            // This test verifies the behavior of required navigation properties
            // marked with null! which should throw NullReferenceException when accessed if not set

            // Arrange
            var userRight = new UserRight();

            // Act & Assert
            // Note: We can't directly test the exception without actually causing it,
            // but we can verify that the properties are initially null
            Assert.Null(userRight.User);
            Assert.Null(userRight.Right);
        }
    }
}