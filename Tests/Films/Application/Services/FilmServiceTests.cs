using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Films.Application.DTOs;
using Films.Application.Mappings;
using Films.Application.Services;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Films.Tests.Application.Services
{
    public class FilmServiceTests
    {
        private readonly Mock<IFilmRepository> _mockRepository;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<FilmService>> _mockLogger;
        private readonly FilmService _service;

        public FilmServiceTests()
        {
            _mockRepository = new Mock<IFilmRepository>();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mapperConfig.CreateMapper();
            _mockLogger = new Mock<ILogger<FilmService>>();
            _service = new FilmService(_mockRepository.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllFilmDtos()
        {
            // Arrange
            var films = new List<Film>
            {
                new Film { Id = 1, Name = "Film 1", Description = "Description 1", IsActive = true },
                new Film { Id = 2, Name = "Film 2", Description = "Description 2", IsActive = false }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(films);

            // Act
            var result = await _service.GetAllAsync();
            var resultList = result.ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(1, resultList[0].Id);
            Assert.Equal("Film 1", resultList[0].Name);
            Assert.Equal("Description 1", resultList[0].Description);
            Assert.True(resultList[0].IsActive);

            Assert.Equal(2, resultList[1].Id);
            Assert.Equal("Film 2", resultList[1].Name);
            Assert.Equal("Description 2", resultList[1].Description);
            Assert.False(resultList[1].IsActive);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnFilmDto()
        {
            // Arrange
            var film = new Film
            {
                Id = 1,
                Name = "Test Film",
                Description = "Test Description",
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(film);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(film.Id, result.Id);
            Assert.Equal(film.Name, result.Name);
            Assert.Equal(film.Description, result.Description);
            Assert.Equal(film.CreatedDate, result.CreatedDate);
            Assert.Equal(film.IsActive, result.IsActive);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Film?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateAndReturnFilmDto()
        {
            // Arrange
            var createDto = new FilmCreateDto
            {
                Name = "New Film",
                Description = "New Description"
            };

            var film = new Film
            {
                Name = "New Film",
                Description = "New Description"
            };

            var createdFilm = new Film
            {
                Id = 1,
                Name = "New Film",
                Description = "New Description",
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "system"
            };

            _mockRepository.Setup(repo => repo.AddAsync(
                It.Is<Film>(f => f.Name == createDto.Name && f.Description == createDto.Description),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdFilm);

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(createDto.Name, result.Name);
            Assert.Equal(createDto.Description, result.Description);
            Assert.True(result.IsActive);

            _mockRepository.Verify(repo => repo.AddAsync(
                It.Is<Film>(f =>
                    f.Name == createDto.Name &&
                    f.Description == createDto.Description &&
                    f.IsActive == true &&
                    f.CreatedBy == "system" &&
                    f.CreatedDate > DateTime.UtcNow.AddMinutes(-1)),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithValidId_ShouldUpdateFilm()
        {
            // Arrange
            var existingFilm = new Film
            {
                Id = 1,
                Name = "Original Name",
                Description = "Original Description",
                IsActive = true,
                CreatedDate = DateTime.UtcNow.AddDays(-1),
                CreatedBy = "system"
            };

            var updateDto = new FilmUpdateDto
            {
                Name = "Updated Name",
                Description = "Updated Description",
                IsActive = false
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingFilm);

            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Film>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(1, updateDto);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateAsync(
                It.Is<Film>(f =>
                    f.Id == 1 &&
                    f.Name == updateDto.Name &&
                    f.Description == updateDto.Description &&
                    f.IsActive == updateDto.IsActive &&
                    f.ModifiedBy == "system" &&
                    f.ModifiedDate > DateTime.UtcNow.AddMinutes(-1)),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Film?)null);

            var updateDto = new FilmUpdateDto
            {
                Name = "Updated Name",
                Description = "Updated Description",
                IsActive = true
            };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.UpdateAsync(999, updateDto));
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDeleteMethod()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.DeleteAsync(1, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAsync(1);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMatchingFilmDtos()
        {
            // Arrange
            var searchTerm = "Action";
            var films = new List<Film>
            {
                new Film { Id = 1, Name = "Action Film 1", Description = "Action packed", IsActive = true },
                new Film { Id = 2, Name = "Action Film 2", Description = "More action", IsActive = true }
            };

            _mockRepository.Setup(repo => repo.SearchAsync(searchTerm, It.IsAny<CancellationToken>()))
                .ReturnsAsync(films);

            // Act
            var result = await _service.SearchAsync(searchTerm);
            var resultList = result.ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(1, resultList[0].Id);
            Assert.Equal("Action Film 1", resultList[0].Name);
            Assert.Equal(2, resultList[1].Id);
            Assert.Equal("Action Film 2", resultList[1].Name);
        }

        [Fact]
        public async Task CancellationToken_ShouldBePropagatedToRepository()
        {
            // Arrange
            var cancellationToken = new CancellationToken(true);
            _mockRepository.Setup(repo => repo.GetAllAsync(cancellationToken))
                .ThrowsAsync(new OperationCanceledException(cancellationToken));

            // Act & Assert
            // Should throw an exception when the token is canceled
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await _service.GetAllAsync(cancellationToken));
        }
    }
}