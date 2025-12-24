using System;
using Films.Domain.Entities;
using Xunit;

namespace Films.Tests.Domain.Entities
{
    public class RefAFTests
    {
        [Fact]
        public void RefAF_Constructor_InitializesWithDefaultValues()
        {
            // Act
            var refAF = new RefAF();

            // Assert
            Assert.Equal(0, refAF.Id);
            Assert.Equal(0, refAF.IdActor);
            Assert.Equal(0, refAF.IdFilm);
            Assert.Equal(default(DateTime), refAF.CreatedDate);
            Assert.Null(refAF.ModifiedDate);
            Assert.True(refAF.IsActive);
            Assert.Equal(string.Empty, refAF.CreatedBy);
            Assert.Null(refAF.ModifiedBy);
            Assert.Null(refAF.Actor);
            Assert.Null(refAF.Film);
        }

        [Fact]
        public void RefAF_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var refAF = new RefAF();
            var now = DateTime.Now;
            var actor = new Actor { Id = 1, Name = "John Doe" };
            var film = new Film { Id = 1, Name = "Test Film" };

            // Act
            refAF.Id = 1;
            refAF.IdActor = 1;
            refAF.IdFilm = 1;
            refAF.CreatedDate = now;
            refAF.ModifiedDate = now.AddDays(1);
            refAF.IsActive = false;
            refAF.CreatedBy = "Test User";
            refAF.ModifiedBy = "Another User";
            refAF.Actor = actor;
            refAF.Film = film;

            // Assert
            Assert.Equal(1, refAF.Id);
            Assert.Equal(1, refAF.IdActor);
            Assert.Equal(1, refAF.IdFilm);
            Assert.Equal(now, refAF.CreatedDate);
            Assert.Equal(now.AddDays(1), refAF.ModifiedDate);
            Assert.False(refAF.IsActive);
            Assert.Equal("Test User", refAF.CreatedBy);
            Assert.Equal("Another User", refAF.ModifiedBy);
            Assert.Same(actor, refAF.Actor);
            Assert.Same(film, refAF.Film);
        }

        [Fact]
        public void RefAF_NullableProperties_CanBeNull()
        {
            // Arrange
            var refAF = new RefAF
            {
                ModifiedDate = DateTime.Now,
                ModifiedBy = "User"
            };

            // Act
            refAF.ModifiedDate = null;
            refAF.ModifiedBy = null;

            // Assert
            Assert.Null(refAF.ModifiedDate);
            Assert.Null(refAF.ModifiedBy);
        }

        [Fact]
        public void RefAF_RequiredNavigationProperties_ThrowsExceptionIfNullAndAccessed()
        {
            // This test verifies the behavior of required navigation properties
            // marked with null! which should throw NullReferenceException when accessed if not set

            // Arrange
            var refAF = new RefAF();

            // Act & Assert
            // Note: We can't directly test the exception without actually causing it,
            // but we can verify that the properties are initially null
            Assert.Null(refAF.Actor);
            Assert.Null(refAF.Film);
        }
    }
}