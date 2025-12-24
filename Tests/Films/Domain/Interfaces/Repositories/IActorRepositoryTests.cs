using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace Films.Tests.Domain.Interfaces.Repositories
{
    public class IActorRepositoryTests
    {
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllActors()
        {
            // Arrange
            var mockRepository = new Mock<IActorRepository>();
            var expectedActors = new List<Actor>
            {
                new Actor { Id = 1, Name = "Actor 1" },
                new Actor { Id = 2, Name = "Actor 2" }
            };
            mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedActors);

            // Act
            var result = await mockRepository.Object.GetAllAsync();

            // Assert
            Assert.Equal(expectedActors.Count, result.Count());
            Assert.Equal(expectedActors, result);
            mockRepository.Verify(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnActor()
        {
            // Arrange
            var mockRepository = new Mock<IActorRepository>();
            var expectedActor = new Actor { Id = 1, Name = "Test Actor" };
            mockRepository.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedActor);

            // Act
            var result = await mockRepository.Object.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedActor.Id, result.Id);
            Assert.Equal(expectedActor.Name, result.Name);
            mockRepository.Verify(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var mockRepository = new Mock<IActorRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Actor?)null);

            // Act
            var result = await mockRepository.Object.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
            mockRepository.Verify(repo => repo.GetByIdAsync(999, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldAddAndReturnActor()
        {
            // Arrange
            var mockRepository = new Mock<IActorRepository>();
            var actor = new Actor { Name = "New Actor" };
            var addedActor = new Actor { Id = 1, Name = "New Actor" };

            mockRepository.Setup(repo => repo.AddAsync(actor, It.IsAny<CancellationToken>()))
                .ReturnsAsync(addedActor);

            // Act
            var result = await mockRepository.Object.AddAsync(actor);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(actor.Name, result.Name);
            mockRepository.Verify(repo => repo.AddAsync(actor, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallRepositoryMethod()
        {
            // Arrange
            var mockRepository = new Mock<IActorRepository>();
            var actor = new Actor { Id = 1, Name = "Updated Actor" };

            mockRepository.Setup(repo => repo.UpdateAsync(actor, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await mockRepository.Object.UpdateAsync(actor);

            // Assert
            mockRepository.Verify(repo => repo.UpdateAsync(actor, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryMethod()
        {
            // Arrange
            var mockRepository = new Mock<IActorRepository>();
            mockRepository.Setup(repo => repo.DeleteAsync(1, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await mockRepository.Object.DeleteAsync(1);

            // Assert
            mockRepository.Verify(repo => repo.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ExistsAsync_WithExistingId_ShouldReturnTrue()
        {
            // Arrange
            var mockRepository = new Mock<IActorRepository>();
            mockRepository.Setup(repo => repo.ExistsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await mockRepository.Object.ExistsAsync(1);

            // Assert
            Assert.True(result);
            mockRepository.Verify(repo => repo.ExistsAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ExistsAsync_WithNonExistingId_ShouldReturnFalse()
        {
            // Arrange
            var mockRepository = new Mock<IActorRepository>();
            mockRepository.Setup(repo => repo.ExistsAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await mockRepository.Object.ExistsAsync(999);

            // Assert
            Assert.False(result);
            mockRepository.Verify(repo => repo.ExistsAsync(999, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMatchingActors()
        {
            // Arrange
            var mockRepository = new Mock<IActorRepository>();
            var searchTerm = "Smith";
            var expectedActors = new List<Actor>
            {
                new Actor { Id = 1, Name = "John Smith" },
                new Actor { Id = 2, Name = "Jane Smith" }
            };

            mockRepository.Setup(repo => repo.SearchAsync(searchTerm, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedActors);

            // Act
            var result = await mockRepository.Object.SearchAsync(searchTerm);

            // Assert
            Assert.Equal(expectedActors.Count, result.Count());
            Assert.Equal(expectedActors, result);
            mockRepository.Verify(repo => repo.SearchAsync(searchTerm, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CancellationToken_ShouldBePropagatedToRepository()
        {
            // Arrange
            var mockRepository = new Mock<IActorRepository>();
            var cancellationToken = new CancellationToken(true);

            // We're not asserting the result, just verifying the cancellation token is passed through
            mockRepository.Setup(repo => repo.GetAllAsync(cancellationToken))
                .ThrowsAsync(new OperationCanceledException(cancellationToken));

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                mockRepository.Object.GetAllAsync(cancellationToken));

            mockRepository.Verify(repo => repo.GetAllAsync(cancellationToken), Times.Once);
        }
    }
}