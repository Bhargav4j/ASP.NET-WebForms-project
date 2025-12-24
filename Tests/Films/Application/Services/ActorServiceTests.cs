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
    public class ActorServiceTests
    {
        private readonly Mock<IActorRepository> _mockRepository;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ActorService>> _mockLogger;
        private readonly ActorService _service;

        public ActorServiceTests()
        {
            _mockRepository = new Mock<IActorRepository>();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mapperConfig.CreateMapper();
            _mockLogger = new Mock<ILogger<ActorService>>();
            _service = new ActorService(_mockRepository.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllActorDtos()
        {
            // Arrange
            var sex = new Sex { Id = 1, Name = "Male" };
            var actors = new List<Actor>
            {
                new Actor { Id = 1, Name = "John", Surname = "Doe", IdSex = 1, Sex = sex, IsActive = true },
                new Actor { Id = 2, Name = "Jane", Surname = "Smith", IdSex = 2, IsActive = false }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(actors);

            // Act
            var result = await _service.GetAllAsync();
            var resultList = result.ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(1, resultList[0].Id);
            Assert.Equal("John", resultList[0].Name);
            Assert.Equal("Doe", resultList[0].Surname);
            Assert.Equal(1, resultList[0].IdSex);
            Assert.Equal("Male", resultList[0].SexName);
            Assert.True(resultList[0].IsActive);

            Assert.Equal(2, resultList[1].Id);
            Assert.Equal("Jane", resultList[1].Name);
            Assert.Equal("Smith", resultList[1].Surname);
            Assert.Equal(2, resultList[1].IdSex);
            Assert.Null(resultList[1].SexName);
            Assert.False(resultList[1].IsActive);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnActorDto()
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
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(actor);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(actor.Id, result.Id);
            Assert.Equal(actor.Name, result.Name);
            Assert.Equal(actor.Surname, result.Surname);
            Assert.Equal(actor.IdSex, result.IdSex);
            Assert.Equal(actor.Sex.Name, result.SexName);
            Assert.Equal(actor.CreatedDate, result.CreatedDate);
            Assert.Equal(actor.IsActive, result.IsActive);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Actor?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateAndReturnActorDto()
        {
            // Arrange
            var createDto = new ActorCreateDto
            {
                Name = "New Actor",
                Surname = "Lastname",
                IdSex = 1
            };

            var createdActor = new Actor
            {
                Id = 1,
                Name = "New Actor",
                Surname = "Lastname",
                IdSex = 1,
                Sex = new Sex { Id = 1, Name = "Male" },
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "system"
            };

            _mockRepository.Setup(repo => repo.AddAsync(
                It.Is<Actor>(a => a.Name == createDto.Name && a.Surname == createDto.Surname && a.IdSex == createDto.IdSex),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdActor);

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(createDto.Name, result.Name);
            Assert.Equal(createDto.Surname, result.Surname);
            Assert.Equal(createDto.IdSex, result.IdSex);
            Assert.Equal("Male", result.SexName);
            Assert.True(result.IsActive);

            _mockRepository.Verify(repo => repo.AddAsync(
                It.Is<Actor>(a =>
                    a.Name == createDto.Name &&
                    a.Surname == createDto.Surname &&
                    a.IdSex == createDto.IdSex &&
                    a.IsActive == true &&
                    a.CreatedBy == "system" &&
                    a.CreatedDate > DateTime.UtcNow.AddMinutes(-1)),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithValidId_ShouldUpdateActor()
        {
            // Arrange
            var existingActor = new Actor
            {
                Id = 1,
                Name = "Original Name",
                Surname = "Original Surname",
                IdSex = 1,
                IsActive = true,
                CreatedDate = DateTime.UtcNow.AddDays(-1),
                CreatedBy = "system"
            };

            var updateDto = new ActorUpdateDto
            {
                Name = "Updated Name",
                Surname = "Updated Surname",
                IdSex = 2,
                IsActive = false
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingActor);

            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Actor>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(1, updateDto);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateAsync(
                It.Is<Actor>(a =>
                    a.Id == 1 &&
                    a.Name == updateDto.Name &&
                    a.Surname == updateDto.Surname &&
                    a.IdSex == updateDto.IdSex &&
                    a.IsActive == updateDto.IsActive &&
                    a.ModifiedBy == "system" &&
                    a.ModifiedDate > DateTime.UtcNow.AddMinutes(-1)),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Actor?)null);

            var updateDto = new ActorUpdateDto
            {
                Name = "Updated Name",
                Surname = "Updated Surname",
                IdSex = 1,
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
        public async Task SearchAsync_ShouldReturnMatchingActorDtos()
        {
            // Arrange
            var searchTerm = "Smith";
            var actors = new List<Actor>
            {
                new Actor { Id = 1, Name = "John", Surname = "Smith", IsActive = true },
                new Actor { Id = 2, Name = "Jane", Surname = "Smith", IsActive = true }
            };

            _mockRepository.Setup(repo => repo.SearchAsync(searchTerm, It.IsAny<CancellationToken>()))
                .ReturnsAsync(actors);

            // Act
            var result = await _service.SearchAsync(searchTerm);
            var resultList = result.ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(1, resultList[0].Id);
            Assert.Equal("John", resultList[0].Name);
            Assert.Equal("Smith", resultList[0].Surname);
            Assert.Equal(2, resultList[1].Id);
            Assert.Equal("Jane", resultList[1].Name);
            Assert.Equal("Smith", resultList[1].Surname);
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