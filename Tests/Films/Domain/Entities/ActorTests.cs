using System;
using System.Collections.Generic;
using Films.Domain.Entities;
using Xunit;

namespace Films.Tests.Domain.Entities
{
    public class ActorTests
    {
        [Fact]
        public void Actor_Constructor_InitializesWithDefaultValues()
        {
            // Act
            var actor = new Actor();

            // Assert
            Assert.Equal(0, actor.Id);
            Assert.Equal(string.Empty, actor.Name);
            Assert.Null(actor.Surname);
            Assert.Null(actor.IdSex);
            Assert.Equal(default(DateTime), actor.CreatedDate);
            Assert.Null(actor.ModifiedDate);
            Assert.True(actor.IsActive);
            Assert.Equal(string.Empty, actor.CreatedBy);
            Assert.Null(actor.ModifiedBy);
            Assert.Null(actor.Sex);
            Assert.NotNull(actor.RefAFs);
            Assert.Empty(actor.RefAFs);
        }

        [Fact]
        public void Actor_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var actor = new Actor();
            var now = DateTime.Now;
            var sex = new Sex { Id = 1, Name = "Male" };
            var refAFCollection = new List<RefAF> { new RefAF() };

            // Act
            actor.Id = 1;
            actor.Name = "John";
            actor.Surname = "Doe";
            actor.IdSex = 1;
            actor.CreatedDate = now;
            actor.ModifiedDate = now.AddDays(1);
            actor.IsActive = false;
            actor.CreatedBy = "Test User";
            actor.ModifiedBy = "Another User";
            actor.Sex = sex;
            actor.RefAFs = refAFCollection;

            // Assert
            Assert.Equal(1, actor.Id);
            Assert.Equal("John", actor.Name);
            Assert.Equal("Doe", actor.Surname);
            Assert.Equal(1, actor.IdSex);
            Assert.Equal(now, actor.CreatedDate);
            Assert.Equal(now.AddDays(1), actor.ModifiedDate);
            Assert.False(actor.IsActive);
            Assert.Equal("Test User", actor.CreatedBy);
            Assert.Equal("Another User", actor.ModifiedBy);
            Assert.Same(sex, actor.Sex);
            Assert.Same(refAFCollection, actor.RefAFs);
        }

        [Fact]
        public void Actor_NullableProperties_CanBeNull()
        {
            // Arrange
            var actor = new Actor
            {
                Surname = "Doe",
                IdSex = 1,
                ModifiedDate = DateTime.Now,
                ModifiedBy = "User",
                Sex = new Sex()
            };

            // Act
            actor.Surname = null;
            actor.IdSex = null;
            actor.ModifiedDate = null;
            actor.ModifiedBy = null;
            actor.Sex = null;

            // Assert
            Assert.Null(actor.Surname);
            Assert.Null(actor.IdSex);
            Assert.Null(actor.ModifiedDate);
            Assert.Null(actor.ModifiedBy);
            Assert.Null(actor.Sex);
        }
    }
}