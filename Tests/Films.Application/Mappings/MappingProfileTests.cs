using System;
using Xunit;
using AutoMapper;
using Films.Application.DTOs;
using Films.Application.Mappings;
using Films.Domain.Entities;

namespace Films.Tests.Application.Mappings;

public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void MappingProfile_ShouldBeValid()
    {
        // Arrange
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        // Act & Assert
        config.AssertConfigurationIsValid();
    }

    [Fact]
    public void Film_To_FilmDto_ShouldMapCorrectly()
    {
        // Arrange
        var film = new Film
        {
            Id = 1,
            Name = "Test Film",
            Description = "Description",
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        // Act
        var filmDto = _mapper.Map<FilmDto>(film);

        // Assert
        Assert.NotNull(filmDto);
        Assert.Equal(film.Id, filmDto.Id);
        Assert.Equal(film.Name, filmDto.Name);
        Assert.Equal(film.Description, filmDto.Description);
        Assert.Equal(film.IsActive, filmDto.IsActive);
    }

    [Fact]
    public void FilmCreateDto_To_Film_ShouldMapCorrectly()
    {
        // Arrange
        var filmCreateDto = new FilmCreateDto
        {
            Name = "New Film",
            Description = "New Description"
        };

        // Act
        var film = _mapper.Map<Film>(filmCreateDto);

        // Assert
        Assert.NotNull(film);
        Assert.Equal(filmCreateDto.Name, film.Name);
        Assert.Equal(filmCreateDto.Description, film.Description);
        Assert.True(film.IsActive);
        Assert.Equal("System", film.CreatedBy);
    }

    [Fact]
    public void Actor_To_ActorDto_ShouldMapCorrectly()
    {
        // Arrange
        var actor = new Actor
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            SexId = 1,
            Sex = new Sex { Name = "Male" },
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        // Act
        var actorDto = _mapper.Map<ActorDto>(actor);

        // Assert
        Assert.NotNull(actorDto);
        Assert.Equal(actor.Id, actorDto.Id);
        Assert.Equal(actor.FirstName, actorDto.FirstName);
        Assert.Equal(actor.LastName, actorDto.LastName);
        Assert.Equal(actor.SexId, actorDto.SexId);
        Assert.Equal("Male", actorDto.SexName);
        Assert.Equal(actor.IsActive, actorDto.IsActive);
    }

    [Fact]
    public void ActorCreateDto_To_Actor_ShouldMapCorrectly()
    {
        // Arrange
        var actorCreateDto = new ActorCreateDto
        {
            FirstName = "New",
            LastName = "Actor",
            SexId = 1
        };

        // Act
        var actor = _mapper.Map<Actor>(actorCreateDto);

        // Assert
        Assert.NotNull(actor);
        Assert.Equal(actorCreateDto.FirstName, actor.FirstName);
        Assert.Equal(actorCreateDto.LastName, actor.LastName);
        Assert.Equal(actorCreateDto.SexId, actor.SexId);
        Assert.True(actor.IsActive);
        Assert.Equal("System", actor.CreatedBy);
    }

    [Fact]
    public void Director_To_DirectorDto_ShouldMapCorrectly()
    {
        // Arrange
        var director = new Director
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Smith",
            SexId = 2,
            Sex = new Sex { Name = "Female" },
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        // Act
        var directorDto = _mapper.Map<DirectorDto>(director);

        // Assert
        Assert.NotNull(directorDto);
        Assert.Equal(director.Id, directorDto.Id);
        Assert.Equal(director.FirstName, directorDto.FirstName);
        Assert.Equal(director.LastName, directorDto.LastName);
        Assert.Equal(director.SexId, directorDto.SexId);
        Assert.Equal("Female", directorDto.SexName);
        Assert.Equal(director.IsActive, directorDto.IsActive);
    }

    [Fact]
    public void DirectorCreateDto_To_Director_ShouldMapCorrectly()
    {
        // Arrange
        var directorCreateDto = new DirectorCreateDto
        {
            FirstName = "New",
            LastName = "Director",
            SexId = 1
        };

        // Act
        var director = _mapper.Map<Director>(directorCreateDto);

        // Assert
        Assert.NotNull(director);
        Assert.Equal(directorCreateDto.FirstName, director.FirstName);
        Assert.Equal(directorCreateDto.LastName, director.LastName);
        Assert.Equal(directorCreateDto.SexId, director.SexId);
        Assert.True(director.IsActive);
        Assert.Equal("System", director.CreatedBy);
    }
}
