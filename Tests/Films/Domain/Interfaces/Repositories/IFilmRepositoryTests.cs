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
    public class IFilmRepositoryTests
    {
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllFilms()
        {
            // Arrange
            var mockRepository = new Mock<IFilmRepository>();
            var expectedFilms = new List<Film>
            {
                new Film { Id = 1, Name = "Film 1" },
                new Film { Id = 2, Name = "Film 2" }
            };
            mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedFilms);

            // Act
            var result = await mockRepository.Object.GetAllAsync();

            // Assert
            Assert.Equal(expectedFilms.Count, result.Count());
            Assert.Equal(expectedFilms, result);
            mockRepository.Verify(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnFilm()
        {
            // Arrange
            var mockRepository = new Mock<IFilmRepository>();
            var expectedFilm = new Film { Id = 1, Name = "Test Film" };
            mockRepository.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedFilm);

            // Act
            var result = await mockRepository.Object.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedFilm.Id, result.Id);
            Assert.Equal(expectedFilm.Name, result.Name);
            mockRepository.Verify(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var mockRepository = new Mock<IFilmRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Film?)null);

            // Act
            var result = await mockRepository.Object.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
            mockRepository.Verify(repo => repo.GetByIdAsync(999, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldAddAndReturnFilm()
        {
            // Arrange
            var mockRepository = new Mock<IFilmRepository>();
            var film = new Film { Name = "New Film" };
            var addedFilm = new Film { Id = 1, Name = "New Film" };

            mockRepository.Setup(repo => repo.AddAsync(film, It.IsAny<CancellationToken>()))
                .ReturnsAsync(addedFilm);

            // Act
            var result = await mockRepository.Object.AddAsync(film);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(film.Name, result.Name);
            mockRepository.Verify(repo => repo.AddAsync(film, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallRepositoryMethod()
        {
            // Arrange
            var mockRepository = new Mock<IFilmRepository>();
            var film = new Film { Id = 1, Name = "Updated Film" };

            mockRepository.Setup(repo => repo.UpdateAsync(film, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await mockRepository.Object.UpdateAsync(film);

            // Assert
            mockRepository.Verify(repo => repo.UpdateAsync(film, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryMethod()
        {
            // Arrange
            var mockRepository = new Mock<IFilmRepository>();
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
            var mockRepository = new Mock<IFilmRepository>();
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
            var mockRepository = new Mock<IFilmRepository>();
            mockRepository.Setup(repo => repo.ExistsAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await mockRepository.Object.ExistsAsync(999);

            // Assert
            Assert.False(result);
            mockRepository.Verify(repo => repo.ExistsAsync(999, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMatchingFilms()
        {
            // Arrange
            var mockRepository = new Mock<IFilmRepository>();
            var searchTerm = "Action";
            var expectedFilms = new List<Film>
            {
                new Film { Id = 1, Name = "Action Film 1" },
                new Film { Id = 2, Name = "Action Film 2" }
            };

            mockRepository.Setup(repo => repo.SearchAsync(searchTerm, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedFilms);

            // Act
            var result = await mockRepository.Object.SearchAsync(searchTerm);

            // Assert
            Assert.Equal(expectedFilms.Count, result.Count());
            Assert.Equal(expectedFilms, result);
            mockRepository.Verify(repo => repo.SearchAsync(searchTerm, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CancellationToken_ShouldBePropagatedToRepository()
        {
            // Arrange
            var mockRepository = new Mock<IFilmRepository>();
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