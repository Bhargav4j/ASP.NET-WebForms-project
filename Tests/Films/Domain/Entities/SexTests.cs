using System;
using System.Collections.Generic;
using Films.Domain.Entities;
using Xunit;

namespace Films.Tests.Domain.Entities
{
    public class SexTests
    {
        [Fact]
        public void Sex_Constructor_InitializesWithDefaultValues()
        {
            // Act
            var sex = new Sex();

            // Assert
            Assert.Equal(0, sex.Id);
            Assert.Equal(string.Empty, sex.Name);
            Assert.Equal(default(DateTime), sex.CreatedDate);
            Assert.Null(sex.ModifiedDate);
            Assert.True(sex.IsActive);
            Assert.Equal(string.Empty, sex.CreatedBy);
            Assert.Null(sex.ModifiedBy);
            Assert.NotNull(sex.Actors);
            Assert.Empty(sex.Actors);
            Assert.NotNull(sex.DirectedBys);
            Assert.Empty(sex.DirectedBys);
            Assert.NotNull(sex.Users);
            Assert.Empty(sex.Users);
        }

        [Fact]
        public void Sex_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var sex = new Sex();
            var now = DateTime.Now;
            var actors = new List<Actor> { new Actor() };
            var directors = new List<DirectedBy> { new DirectedBy() };
            var users = new List<User> { new User() };

            // Act
            sex.Id = 1;
            sex.Name = "Female";
            sex.CreatedDate = now;
            sex.ModifiedDate = now.AddDays(1);
            sex.IsActive = false;
            sex.CreatedBy = "Test User";
            sex.ModifiedBy = "Another User";
            sex.Actors = actors;
            sex.DirectedBys = directors;
            sex.Users = users;

            // Assert
            Assert.Equal(1, sex.Id);
            Assert.Equal("Female", sex.Name);
            Assert.Equal(now, sex.CreatedDate);
            Assert.Equal(now.AddDays(1), sex.ModifiedDate);
            Assert.False(sex.IsActive);
            Assert.Equal("Test User", sex.CreatedBy);
            Assert.Equal("Another User", sex.ModifiedBy);
            Assert.Same(actors, sex.Actors);
            Assert.Same(directors, sex.DirectedBys);
            Assert.Same(users, sex.Users);
        }

        [Fact]
        public void Sex_NullableProperties_CanBeNull()
        {
            // Arrange
            var sex = new Sex
            {
                ModifiedDate = DateTime.Now,
                ModifiedBy = "User"
            };

            // Act
            sex.ModifiedDate = null;
            sex.ModifiedBy = null;

            // Assert
            Assert.Null(sex.ModifiedDate);
            Assert.Null(sex.ModifiedBy);
        }

        [Fact]
        public void Sex_CollectionProperties_InitializedToEmptyLists()
        {
            // Act
            var sex = new Sex();

            // Assert
            Assert.IsType<List<Actor>>(sex.Actors);
            Assert.IsType<List<DirectedBy>>(sex.DirectedBys);
            Assert.IsType<List<User>>(sex.Users);
        }
    }
}