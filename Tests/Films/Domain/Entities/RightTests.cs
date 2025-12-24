using System;
using System.Collections.Generic;
using Films.Domain.Entities;
using Xunit;

namespace Films.Tests.Domain.Entities
{
    public class RightTests
    {
        [Fact]
        public void Right_Constructor_InitializesWithDefaultValues()
        {
            // Act
            var right = new Right();

            // Assert
            Assert.Equal(0, right.Id);
            Assert.Equal(string.Empty, right.Name);
            Assert.Null(right.Description);
            Assert.Equal(default(DateTime), right.CreatedDate);
            Assert.Null(right.ModifiedDate);
            Assert.True(right.IsActive);
            Assert.Equal(string.Empty, right.CreatedBy);
            Assert.Null(right.ModifiedBy);
            Assert.NotNull(right.UserRights);
            Assert.Empty(right.UserRights);
        }

        [Fact]
        public void Right_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var right = new Right();
            var now = DateTime.Now;
            var userRights = new List<UserRight> { new UserRight() };

            // Act
            right.Id = 1;
            right.Name = "CanViewFilms";
            right.Description = "Permission to view films";
            right.CreatedDate = now;
            right.ModifiedDate = now.AddDays(1);
            right.IsActive = false;
            right.CreatedBy = "Test User";
            right.ModifiedBy = "Another User";
            right.UserRights = userRights;

            // Assert
            Assert.Equal(1, right.Id);
            Assert.Equal("CanViewFilms", right.Name);
            Assert.Equal("Permission to view films", right.Description);
            Assert.Equal(now, right.CreatedDate);
            Assert.Equal(now.AddDays(1), right.ModifiedDate);
            Assert.False(right.IsActive);
            Assert.Equal("Test User", right.CreatedBy);
            Assert.Equal("Another User", right.ModifiedBy);
            Assert.Same(userRights, right.UserRights);
        }

        [Fact]
        public void Right_NullableProperties_CanBeNull()
        {
            // Arrange
            var right = new Right
            {
                Description = "Test Description",
                ModifiedDate = DateTime.Now,
                ModifiedBy = "User"
            };

            // Act
            right.Description = null;
            right.ModifiedDate = null;
            right.ModifiedBy = null;

            // Assert
            Assert.Null(right.Description);
            Assert.Null(right.ModifiedDate);
            Assert.Null(right.ModifiedBy);
        }

        [Fact]
        public void Right_UserRightsCollection_InitializedToEmptyList()
        {
            // Act
            var right = new Right();

            // Assert
            Assert.IsType<List<UserRight>>(right.UserRights);
            Assert.Empty(right.UserRights);
        }
    }
}