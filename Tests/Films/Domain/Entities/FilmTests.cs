using System;
using System.Collections.Generic;
using Films.Domain.Entities;
using Xunit;

namespace Films.Tests.Domain.Entities
{
    public class FilmTests
    {
        [Fact]
        public void Film_Constructor_InitializesWithDefaultValues()
        {
            // Act
            var film = new Film();

            // Assert
            Assert.Equal(0, film.Id);
            Assert.Equal(string.Empty, film.Name);
            Assert.Null(film.Description);
            Assert.Equal(default(DateTime), film.CreatedDate);
            Assert.Null(film.ModifiedDate);
            Assert.True(film.IsActive);
            Assert.Equal(string.Empty, film.CreatedBy);
            Assert.Null(film.ModifiedBy);
            Assert.NotNull(film.RefAFs);
            Assert.Empty(film.RefAFs);
            Assert.NotNull(film.RefDAFs);
            Assert.Empty(film.RefDAFs);
        }

        [Fact]
        public void Film_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var film = new Film();
            var now = DateTime.Now;
            var refAFCollection = new List<RefAF> { new RefAF() };
            var refDAFCollection = new List<RefDAF> { new RefDAF() };

            // Act
            film.Id = 1;
            film.Name = "Test Film";
            film.Description = "Test Description";
            film.CreatedDate = now;
            film.ModifiedDate = now.AddDays(1);
            film.IsActive = false;
            film.CreatedBy = "Test User";
            film.ModifiedBy = "Another User";
            film.RefAFs = refAFCollection;
            film.RefDAFs = refDAFCollection;

            // Assert
            Assert.Equal(1, film.Id);
            Assert.Equal("Test Film", film.Name);
            Assert.Equal("Test Description", film.Description);
            Assert.Equal(now, film.CreatedDate);
            Assert.Equal(now.AddDays(1), film.ModifiedDate);
            Assert.False(film.IsActive);
            Assert.Equal("Test User", film.CreatedBy);
            Assert.Equal("Another User", film.ModifiedBy);
            Assert.Same(refAFCollection, film.RefAFs);
            Assert.Same(refDAFCollection, film.RefDAFs);
        }

        [Fact]
        public void Film_NullableProperties_CanBeNull()
        {
            // Arrange
            var film = new Film
            {
                Description = "Test",
                ModifiedDate = DateTime.Now,
                ModifiedBy = "User"
            };

            // Act
            film.Description = null;
            film.ModifiedDate = null;
            film.ModifiedBy = null;

            // Assert
            Assert.Null(film.Description);
            Assert.Null(film.ModifiedDate);
            Assert.Null(film.ModifiedBy);
        }
    }
}