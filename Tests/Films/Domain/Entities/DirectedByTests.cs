using System;
using System.Collections.Generic;
using Films.Domain.Entities;
using Xunit;

namespace Films.Tests.Domain.Entities
{
    public class DirectedByTests
    {
        [Fact]
        public void DirectedBy_Constructor_InitializesWithDefaultValues()
        {
            // Act
            var director = new DirectedBy();

            // Assert
            Assert.Equal(0, director.Id);
            Assert.Equal(string.Empty, director.Name);
            Assert.Null(director.Surname);
            Assert.Null(director.IdSex);
            Assert.Equal(default(DateTime), director.CreatedDate);
            Assert.Null(director.ModifiedDate);
            Assert.True(director.IsActive);
            Assert.Equal(string.Empty, director.CreatedBy);
            Assert.Null(director.ModifiedBy);
            Assert.Null(director.Sex);
            Assert.NotNull(director.RefDAFs);
            Assert.Empty(director.RefDAFs);
        }

        [Fact]
        public void DirectedBy_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var director = new DirectedBy();
            var now = DateTime.Now;
            var sex = new Sex { Id = 1, Name = "Female" };
            var refDAFCollection = new List<RefDAF> { new RefDAF() };

            // Act
            director.Id = 1;
            director.Name = "Jane";
            director.Surname = "Smith";
            director.IdSex = 2;
            director.CreatedDate = now;
            director.ModifiedDate = now.AddDays(1);
            director.IsActive = false;
            director.CreatedBy = "Test User";
            director.ModifiedBy = "Another User";
            director.Sex = sex;
            director.RefDAFs = refDAFCollection;

            // Assert
            Assert.Equal(1, director.Id);
            Assert.Equal("Jane", director.Name);
            Assert.Equal("Smith", director.Surname);
            Assert.Equal(2, director.IdSex);
            Assert.Equal(now, director.CreatedDate);
            Assert.Equal(now.AddDays(1), director.ModifiedDate);
            Assert.False(director.IsActive);
            Assert.Equal("Test User", director.CreatedBy);
            Assert.Equal("Another User", director.ModifiedBy);
            Assert.Same(sex, director.Sex);
            Assert.Same(refDAFCollection, director.RefDAFs);
        }

        [Fact]
        public void DirectedBy_NullableProperties_CanBeNull()
        {
            // Arrange
            var director = new DirectedBy
            {
                Surname = "Smith",
                IdSex = 1,
                ModifiedDate = DateTime.Now,
                ModifiedBy = "User",
                Sex = new Sex()
            };

            // Act
            director.Surname = null;
            director.IdSex = null;
            director.ModifiedDate = null;
            director.ModifiedBy = null;
            director.Sex = null;

            // Assert
            Assert.Null(director.Surname);
            Assert.Null(director.IdSex);
            Assert.Null(director.ModifiedDate);
            Assert.Null(director.ModifiedBy);
            Assert.Null(director.Sex);
        }
    }
}