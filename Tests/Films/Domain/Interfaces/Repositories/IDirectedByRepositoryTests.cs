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
    public class IDirectedByRepositoryTests
    {
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllDirectors()
        {
            // Arrange
            var mockRepository = new Mock<IDirectedByRepository>();
            var expectedDirectors = new List<DirectedBy>
            {
                new DirectedBy { Id = 1, Name = "Director 1" },
                new DirectedBy { Id = 2, Name = "Director 2" }
            };
            mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedDirectors);

            // Act
            var result = await mockRepository.Object.GetAllAsync();

            // Assert
            Assert.Equal(expectedDirectors.Count, result.Count());
            Assert.Equal(expectedDirectors, result);
            mockRepository.Verify(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnDirector()
        {
            // Arrange
            var mockRepository = new Mock<IDirectedByRepository>();
            var expectedDirector = new DirectedBy { Id = 1, Name = "Test Director" };
            mockRepository.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedDirector);

            // Act
            var result = await mockRepository.Object.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDirector.Id, result.Id);
            Assert.Equal(expectedDirector.Name, result.Name);
            mockRepository.Verify(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var mockRepository = new Mock<IDirectedByRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((DirectedBy?)null);

            // Act
            var result = await mockRepository.Object.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
            mockRepository.Verify(repo => repo.GetByIdAsync(999, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldAddAndReturnDirector()
        {
            // Arrange
            var mockRepository = new Mock<IDirectedByRepository>();
            var director = new DirectedBy { Name = "New Director" };
            var addedDirector = new DirectedBy { Id = 1, Name = "New Director" };

            mockRepository.Setup(repo => repo.AddAsync(director, It.IsAny<CancellationToken>()))
                .ReturnsAsync(addedDirector);

            // Act
            var result = await mockRepository.Object.AddAsync(director);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(director.Name, result.Name);
            mockRepository.Verify(repo => repo.AddAsync(director, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallRepositoryMethod()
        {
            // Arrange
            var mockRepository = new Mock<IDirectedByRepository>();
            var director = new DirectedBy { Id = 1, Name = "Updated Director" };

            mockRepository.Setup(repo => repo.UpdateAsync(director, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await mockRepository.Object.UpdateAsync(director);

            // Assert
            mockRepository.Verify(repo => repo.UpdateAsync(director, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryMethod()
        {
            // Arrange
            var mockRepository = new Mock<IDirectedByRepository>();
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
            var mockRepository = new Mock<IDirectedByRepository>();
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
            var mockRepository = new Mock<IDirectedByRepository>();
            mockRepository.Setup(repo => repo.ExistsAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await mockRepository.Object.ExistsAsync(999);

            // Assert
            Assert.False(result);
            mockRepository.Verify(repo => repo.ExistsAsync(999, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMatchingDirectors()
        {
            // Arrange
            var mockRepository = new Mock<IDirectedByRepository>();
            var searchTerm = "Berg";
            var expectedDirectors = new List<DirectedBy>
            {
                new DirectedBy { Id = 1, Name = "Steven Spielberg" },
                new DirectedBy { Id = 2, Name = "Peter Jackson" }
            };

            mockRepository.Setup(repo => repo.SearchAsync(searchTerm, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedDirectors);

            // Act
            var result = await mockRepository.Object.SearchAsync(searchTerm);

            // Assert
            Assert.Equal(expectedDirectors.Count, result.Count());
            Assert.Equal(expectedDirectors, result);
            mockRepository.Verify(repo => repo.SearchAsync(searchTerm, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CancellationToken_ShouldBePropagatedToRepository()
        {
            // Arrange
            var mockRepository = new Mock<IDirectedByRepository>();
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