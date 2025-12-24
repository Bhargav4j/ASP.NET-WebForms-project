using System;
using AutoMapper;
using Films.Application.DTOs;
using Films.Application.Mappings;
using Films.Domain.Entities;
using Xunit;

namespace Films.Tests.Application.Mappings
{
    public class MappingProfileTests
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;

        public MappingProfileTests()
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _mapperConfiguration.CreateMapper();
        }

        [Fact]
        public void MappingConfiguration_IsValid()
        {
            // This will throw if there are any mapping configuration issues
            _mapperConfiguration.AssertConfigurationIsValid();
        }

        [Fact]
        public void Map_Film_To_FilmDto_MapsCorrectly()
        {
            // Arrange
            var film = new Film
            {
                Id = 1,
                Name = "Test Film",
                Description = "Test Description",
                CreatedDate = new DateTime(2023, 1, 1),
                ModifiedDate = new DateTime(2023, 2, 1),
                IsActive = true,
                CreatedBy = "Test User",
                ModifiedBy = "Another User"
            };

            // Act
            var filmDto = _mapper.Map<FilmDto>(film);

            // Assert
            Assert.Equal(film.Id, filmDto.Id);
            Assert.Equal(film.Name, filmDto.Name);
            Assert.Equal(film.Description, filmDto.Description);
            Assert.Equal(film.CreatedDate, filmDto.CreatedDate);
            Assert.Equal(film.ModifiedDate, filmDto.ModifiedDate);
            Assert.Equal(film.IsActive, filmDto.IsActive);
        }

        [Fact]
        public void Map_FilmCreateDto_To_Film_MapsCorrectly()
        {
            // Arrange
            var filmCreateDto = new FilmCreateDto
            {
                Name = "New Film",
                Description = "New Film Description"
            };

            // Act
            var film = _mapper.Map<Film>(filmCreateDto);

            // Assert
            Assert.Equal(filmCreateDto.Name, film.Name);
            Assert.Equal(filmCreateDto.Description, film.Description);
            Assert.Equal(0, film.Id); // Should be ignored in mapping
            Assert.Equal(default(DateTime), film.CreatedDate); // Should be ignored in mapping
            Assert.Null(film.ModifiedDate); // Should be ignored in mapping
            Assert.True(film.IsActive); // Default value from Film entity
            Assert.Equal(string.Empty, film.CreatedBy); // Default value from Film entity
            Assert.Null(film.ModifiedBy); // Default value from Film entity
            Assert.NotNull(film.RefAFs); // Default collection from Film entity
            Assert.Empty(film.RefAFs); // Default empty collection from Film entity
            Assert.NotNull(film.RefDAFs); // Default collection from Film entity
            Assert.Empty(film.RefDAFs); // Default empty collection from Film entity
        }

        [Fact]
        public void Map_Actor_To_ActorDto_MapsCorrectly()
        {
            // Arrange
            var sex = new Sex { Id = 1, Name = "Male" };
            var actor = new Actor
            {
                Id = 1,
                Name = "John",
                Surname = "Doe",
                IdSex = 1,
                Sex = sex,
                CreatedDate = new DateTime(2023, 1, 1),
                ModifiedDate = new DateTime(2023, 2, 1),
                IsActive = true,
                CreatedBy = "Test User",
                ModifiedBy = "Another User"
            };

            // Act
            var actorDto = _mapper.Map<ActorDto>(actor);

            // Assert
            Assert.Equal(actor.Id, actorDto.Id);
            Assert.Equal(actor.Name, actorDto.Name);
            Assert.Equal(actor.Surname, actorDto.Surname);
            Assert.Equal(actor.IdSex, actorDto.IdSex);
            Assert.Equal("Male", actorDto.SexName); // From navigation property
            Assert.Equal(actor.CreatedDate, actorDto.CreatedDate);
            Assert.Equal(actor.ModifiedDate, actorDto.ModifiedDate);
            Assert.Equal(actor.IsActive, actorDto.IsActive);
        }

        [Fact]
        public void Map_Actor_To_ActorDto_WithNullSex_MapsCorrectly()
        {
            // Arrange
            var actor = new Actor
            {
                Id = 1,
                Name = "John",
                Surname = "Doe",
                IdSex = null,
                Sex = null,
                CreatedDate = new DateTime(2023, 1, 1),
                IsActive = true,
                CreatedBy = "Test User"
            };

            // Act
            var actorDto = _mapper.Map<ActorDto>(actor);

            // Assert
            Assert.Equal(actor.Id, actorDto.Id);
            Assert.Equal(actor.Name, actorDto.Name);
            Assert.Equal(actor.Surname, actorDto.Surname);
            Assert.Null(actorDto.IdSex);
            Assert.Null(actorDto.SexName); // Should be null when Sex is null
        }

        [Fact]
        public void Map_ActorCreateDto_To_Actor_MapsCorrectly()
        {
            // Arrange
            var actorCreateDto = new ActorCreateDto
            {
                Name = "New Actor",
                Surname = "Lastname",
                IdSex = 2
            };

            // Act
            var actor = _mapper.Map<Actor>(actorCreateDto);

            // Assert
            Assert.Equal(actorCreateDto.Name, actor.Name);
            Assert.Equal(actorCreateDto.Surname, actor.Surname);
            Assert.Equal(actorCreateDto.IdSex, actor.IdSex);
            Assert.Equal(0, actor.Id); // Should be ignored in mapping
            Assert.Equal(default(DateTime), actor.CreatedDate); // Should be ignored in mapping
            Assert.Null(actor.ModifiedDate); // Should be ignored in mapping
            Assert.True(actor.IsActive); // Default value from Actor entity
            Assert.Equal(string.Empty, actor.CreatedBy); // Default value from Actor entity
            Assert.Null(actor.ModifiedBy); // Default value from Actor entity
            Assert.Null(actor.Sex); // Should be ignored in mapping
            Assert.NotNull(actor.RefAFs); // Default collection from Actor entity
            Assert.Empty(actor.RefAFs); // Default empty collection from Actor entity
        }

        [Fact]
        public void Map_DirectedBy_To_ActorDto_MapsCorrectly()
        {
            // Arrange
            var sex = new Sex { Id = 1, Name = "Female" };
            var director = new DirectedBy
            {
                Id = 1,
                Name = "Jane",
                Surname = "Smith",
                IdSex = 1,
                Sex = sex,
                CreatedDate = new DateTime(2023, 1, 1),
                ModifiedDate = new DateTime(2023, 2, 1),
                IsActive = true,
                CreatedBy = "Test User",
                ModifiedBy = "Another User"
            };

            // Act
            var actorDto = _mapper.Map<ActorDto>(director);

            // Assert
            Assert.Equal(director.Id, actorDto.Id);
            Assert.Equal(director.Name, actorDto.Name);
            Assert.Equal(director.Surname, actorDto.Surname);
            Assert.Equal(director.IdSex, actorDto.IdSex);
            Assert.Equal("Female", actorDto.SexName); // From navigation property
            Assert.Equal(director.CreatedDate, actorDto.CreatedDate);
            Assert.Equal(director.ModifiedDate, actorDto.ModifiedDate);
            Assert.Equal(director.IsActive, actorDto.IsActive);
        }

        [Fact]
        public void Map_DirectedBy_To_ActorDto_WithNullSex_MapsCorrectly()
        {
            // Arrange
            var director = new DirectedBy
            {
                Id = 1,
                Name = "Jane",
                Surname = "Smith",
                IdSex = null,
                Sex = null,
                CreatedDate = new DateTime(2023, 1, 1),
                IsActive = true,
                CreatedBy = "Test User"
            };

            // Act
            var actorDto = _mapper.Map<ActorDto>(director);

            // Assert
            Assert.Equal(director.Id, actorDto.Id);
            Assert.Equal(director.Name, actorDto.Name);
            Assert.Equal(director.Surname, actorDto.Surname);
            Assert.Null(actorDto.IdSex);
            Assert.Null(actorDto.SexName); // Should be null when Sex is null
        }
    }
}