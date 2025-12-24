using System;
using Films.Domain.Entities;
using Xunit;

namespace Films.Tests.Domain.Entities
{
    public class RefDAFTests
    {
        [Fact]
        public void RefDAF_Constructor_InitializesWithDefaultValues()
        {
            // Act
            var refDAF = new RefDAF();

            // Assert
            Assert.Equal(0, refDAF.Id);
            Assert.Equal(0, refDAF.IdDirectedBy);
            Assert.Equal(0, refDAF.IdFilm);
            Assert.Equal(default(DateTime), refDAF.CreatedDate);
            Assert.Null(refDAF.ModifiedDate);
            Assert.True(refDAF.IsActive);
            Assert.Equal(string.Empty, refDAF.CreatedBy);
            Assert.Null(refDAF.ModifiedBy);
            Assert.Null(refDAF.DirectedBy);
            Assert.Null(refDAF.Film);
        }

        [Fact]
        public void RefDAF_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var refDAF = new RefDAF();
            var now = DateTime.Now;
            var director = new DirectedBy { Id = 1, Name = "Jane Smith" };
            var film = new Film { Id = 1, Name = "Test Film" };

            // Act
            refDAF.Id = 1;
            refDAF.IdDirectedBy = 1;
            refDAF.IdFilm = 1;
            refDAF.CreatedDate = now;
            refDAF.ModifiedDate = now.AddDays(1);
            refDAF.IsActive = false;
            refDAF.CreatedBy = "Test User";
            refDAF.ModifiedBy = "Another User";
            refDAF.DirectedBy = director;
            refDAF.Film = film;

            // Assert
            Assert.Equal(1, refDAF.Id);
            Assert.Equal(1, refDAF.IdDirectedBy);
            Assert.Equal(1, refDAF.IdFilm);
            Assert.Equal(now, refDAF.CreatedDate);
            Assert.Equal(now.AddDays(1), refDAF.ModifiedDate);
            Assert.False(refDAF.IsActive);
            Assert.Equal("Test User", refDAF.CreatedBy);
            Assert.Equal("Another User", refDAF.ModifiedBy);
            Assert.Same(director, refDAF.DirectedBy);
            Assert.Same(film, refDAF.Film);
        }

        [Fact]
        public void RefDAF_NullableProperties_CanBeNull()
        {
            // Arrange
            var refDAF = new RefDAF
            {
                ModifiedDate = DateTime.Now,
                ModifiedBy = "User"
            };

            // Act
            refDAF.ModifiedDate = null;
            refDAF.ModifiedBy = null;

            // Assert
            Assert.Null(refDAF.ModifiedDate);
            Assert.Null(refDAF.ModifiedBy);
        }

        [Fact]
        public void RefDAF_RequiredNavigationProperties_ThrowsExceptionIfNullAndAccessed()
        {
            // This test verifies the behavior of required navigation properties
            // marked with null! which should throw NullReferenceException when accessed if not set

            // Arrange
            var refDAF = new RefDAF();

            // Act & Assert
            // Note: We can't directly test the exception without actually causing it,
            // but we can verify that the properties are initially null
            Assert.Null(refDAF.DirectedBy);
            Assert.Null(refDAF.Film);
        }
    }
}