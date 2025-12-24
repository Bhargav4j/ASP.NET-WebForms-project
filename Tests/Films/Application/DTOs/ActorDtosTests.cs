using System;
using Films.Application.DTOs;
using Xunit;

namespace Films.Tests.Application.DTOs
{
    public class ActorDtosTests
    {
        [Fact]
        public void ActorDto_Constructor_InitializesWithDefaultValues()
        {
            // Act
            var actorDto = new ActorDto();

            // Assert
            Assert.Equal(0, actorDto.Id);
            Assert.Equal(string.Empty, actorDto.Name);
            Assert.Null(actorDto.Surname);
            Assert.Null(actorDto.IdSex);
            Assert.Null(actorDto.SexName);
            Assert.Equal(default(DateTime), actorDto.CreatedDate);
            Assert.Null(actorDto.ModifiedDate);
            Assert.False(actorDto.IsActive);
        }

        [Fact]
        public void ActorDto_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var actorDto = new ActorDto();
            var now = DateTime.Now;

            // Act
            actorDto.Id = 1;
            actorDto.Name = "John";
            actorDto.Surname = "Doe";
            actorDto.IdSex = 1;
            actorDto.SexName = "Male";
            actorDto.CreatedDate = now;
            actorDto.ModifiedDate = now.AddDays(1);
            actorDto.IsActive = true;

            // Assert
            Assert.Equal(1, actorDto.Id);
            Assert.Equal("John", actorDto.Name);
            Assert.Equal("Doe", actorDto.Surname);
            Assert.Equal(1, actorDto.IdSex);
            Assert.Equal("Male", actorDto.SexName);
            Assert.Equal(now, actorDto.CreatedDate);
            Assert.Equal(now.AddDays(1), actorDto.ModifiedDate);
            Assert.True(actorDto.IsActive);
        }

        [Fact]
        public void ActorCreateDto_Constructor_InitializesWithDefaultValues()
        {
            // Act
            var actorCreateDto = new ActorCreateDto();

            // Assert
            Assert.Equal(string.Empty, actorCreateDto.Name);
            Assert.Null(actorCreateDto.Surname);
            Assert.Null(actorCreateDto.IdSex);
        }

        [Fact]
        public void ActorCreateDto_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var actorCreateDto = new ActorCreateDto();

            // Act
            actorCreateDto.Name = "New Actor";
            actorCreateDto.Surname = "Lastname";
            actorCreateDto.IdSex = 2;

            // Assert
            Assert.Equal("New Actor", actorCreateDto.Name);
            Assert.Equal("Lastname", actorCreateDto.Surname);
            Assert.Equal(2, actorCreateDto.IdSex);
        }

        [Fact]
        public void ActorUpdateDto_Constructor_InitializesWithDefaultValues()
        {
            // Act
            var actorUpdateDto = new ActorUpdateDto();

            // Assert
            Assert.Equal(string.Empty, actorUpdateDto.Name);
            Assert.Null(actorUpdateDto.Surname);
            Assert.Null(actorUpdateDto.IdSex);
            Assert.False(actorUpdateDto.IsActive);
        }

        [Fact]
        public void ActorUpdateDto_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var actorUpdateDto = new ActorUpdateDto();

            // Act
            actorUpdateDto.Name = "Updated Actor";
            actorUpdateDto.Surname = "Updated Lastname";
            actorUpdateDto.IdSex = 1;
            actorUpdateDto.IsActive = true;

            // Assert
            Assert.Equal("Updated Actor", actorUpdateDto.Name);
            Assert.Equal("Updated Lastname", actorUpdateDto.Surname);
            Assert.Equal(1, actorUpdateDto.IdSex);
            Assert.True(actorUpdateDto.IsActive);
        }

        [Fact]
        public void ActorDto_NullableProperties_CanBeNull()
        {
            // Arrange
            var actorDto = new ActorDto
            {
                Surname = "Doe",
                IdSex = 1,
                SexName = "Male",
                ModifiedDate = DateTime.Now
            };

            // Act
            actorDto.Surname = null;
            actorDto.IdSex = null;
            actorDto.SexName = null;
            actorDto.ModifiedDate = null;

            // Assert
            Assert.Null(actorDto.Surname);
            Assert.Null(actorDto.IdSex);
            Assert.Null(actorDto.SexName);
            Assert.Null(actorDto.ModifiedDate);
        }

        [Fact]
        public void ActorCreateDto_NullableProperties_CanBeNull()
        {
            // Arrange
            var actorCreateDto = new ActorCreateDto
            {
                Surname = "Smith",
                IdSex = 1
            };

            // Act
            actorCreateDto.Surname = null;
            actorCreateDto.IdSex = null;

            // Assert
            Assert.Null(actorCreateDto.Surname);
            Assert.Null(actorCreateDto.IdSex);
        }

        [Fact]
        public void ActorUpdateDto_NullableProperties_CanBeNull()
        {
            // Arrange
            var actorUpdateDto = new ActorUpdateDto
            {
                Surname = "Johnson",
                IdSex = 2
            };

            // Act
            actorUpdateDto.Surname = null;
            actorUpdateDto.IdSex = null;

            // Assert
            Assert.Null(actorUpdateDto.Surname);
            Assert.Null(actorUpdateDto.IdSex);
        }
    }
}