using System;
using Films.Application.DTOs;
using Xunit;

namespace Films.Tests.Application.DTOs
{
    public class FilmDtosTests
    {
        [Fact]
        public void FilmDto_Constructor_InitializesWithDefaultValues()
        {
            // Act
            var filmDto = new FilmDto();

            // Assert
            Assert.Equal(0, filmDto.Id);
            Assert.Equal(string.Empty, filmDto.Name);
            Assert.Null(filmDto.Description);
            Assert.Equal(default(DateTime), filmDto.CreatedDate);
            Assert.Null(filmDto.ModifiedDate);
            Assert.False(filmDto.IsActive);
        }

        [Fact]
        public void FilmDto_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var filmDto = new FilmDto();
            var now = DateTime.Now;

            // Act
            filmDto.Id = 1;
            filmDto.Name = "Test Film";
            filmDto.Description = "Test Description";
            filmDto.CreatedDate = now;
            filmDto.ModifiedDate = now.AddDays(1);
            filmDto.IsActive = true;

            // Assert
            Assert.Equal(1, filmDto.Id);
            Assert.Equal("Test Film", filmDto.Name);
            Assert.Equal("Test Description", filmDto.Description);
            Assert.Equal(now, filmDto.CreatedDate);
            Assert.Equal(now.AddDays(1), filmDto.ModifiedDate);
            Assert.True(filmDto.IsActive);
        }

        [Fact]
        public void FilmCreateDto_Constructor_InitializesWithDefaultValues()
        {
            // Act
            var filmCreateDto = new FilmCreateDto();

            // Assert
            Assert.Equal(string.Empty, filmCreateDto.Name);
            Assert.Null(filmCreateDto.Description);
        }

        [Fact]
        public void FilmCreateDto_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var filmCreateDto = new FilmCreateDto();

            // Act
            filmCreateDto.Name = "New Film";
            filmCreateDto.Description = "New Film Description";

            // Assert
            Assert.Equal("New Film", filmCreateDto.Name);
            Assert.Equal("New Film Description", filmCreateDto.Description);
        }

        [Fact]
        public void FilmUpdateDto_Constructor_InitializesWithDefaultValues()
        {
            // Act
            var filmUpdateDto = new FilmUpdateDto();

            // Assert
            Assert.Equal(string.Empty, filmUpdateDto.Name);
            Assert.Null(filmUpdateDto.Description);
            Assert.False(filmUpdateDto.IsActive);
        }

        [Fact]
        public void FilmUpdateDto_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var filmUpdateDto = new FilmUpdateDto();

            // Act
            filmUpdateDto.Name = "Updated Film";
            filmUpdateDto.Description = "Updated Film Description";
            filmUpdateDto.IsActive = true;

            // Assert
            Assert.Equal("Updated Film", filmUpdateDto.Name);
            Assert.Equal("Updated Film Description", filmUpdateDto.Description);
            Assert.True(filmUpdateDto.IsActive);
        }

        [Fact]
        public void FilmDto_NullableProperties_CanBeNull()
        {
            // Arrange
            var filmDto = new FilmDto
            {
                Description = "Description",
                ModifiedDate = DateTime.Now
            };

            // Act
            filmDto.Description = null;
            filmDto.ModifiedDate = null;

            // Assert
            Assert.Null(filmDto.Description);
            Assert.Null(filmDto.ModifiedDate);
        }

        [Fact]
        public void FilmCreateDto_DescriptionProperty_CanBeNull()
        {
            // Arrange
            var filmCreateDto = new FilmCreateDto
            {
                Description = "Description"
            };

            // Act
            filmCreateDto.Description = null;

            // Assert
            Assert.Null(filmCreateDto.Description);
        }

        [Fact]
        public void FilmUpdateDto_DescriptionProperty_CanBeNull()
        {
            // Arrange
            var filmUpdateDto = new FilmUpdateDto
            {
                Description = "Description"
            };

            // Act
            filmUpdateDto.Description = null;

            // Assert
            Assert.Null(filmUpdateDto.Description);
        }
    }
}