using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Films.Domain.Entities;
using Films.Infrastructure.Data;
using Films.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Films.Tests.Infrastructure.Repositories
{
    public class FilmRepositoryTests
    {
        private readonly DbContextOptions<FilmsDbContext> _options;
        private readonly Mock<ILogger<FilmRepository>> _mockLogger;

        public FilmRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<FilmsDbContext>()
                .UseInMemoryDatabase(databaseName: $"FilmsDbForTesting_{Guid.NewGuid()}")
                .Options;

            _mockLogger = new Mock<ILogger<FilmRepository>>();

            // Seed the database
            using (var context = new FilmsDbContext(_options))
            {
                context.Database.EnsureCreated();

                var films = new List<Film>
                {
                    new Film
                    {
                        Id = 1,
                        Name = "The Shawshank Redemption",
                        Description = "Two imprisoned men bond over a number of years.",
                        CreatedDate = DateTime.UtcNow.AddDays(-10),
                        IsActive = true,
                        CreatedBy = "system"
                    },
                    new Film
                    {
                        Id = 2,
                        Name = "The Godfather",
                        Description = "The aging patriarch of an organized crime dynasty.",
                        CreatedDate = DateTime.UtcNow.AddDays(-9),
                        IsActive = true,
                        CreatedBy = "system"
                    },
                    new Film
                    {
                        Id = 3,
                        Name = "The Dark Knight",
                        Description = "When the menace known as the Joker wreaks havoc.",
                        CreatedDate = DateTime.UtcNow.AddDays(-8),
                        IsActive = false,
                        CreatedBy = "system"
                    }
                };

                context.Films.AddRange(films);
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOnlyActiveFilms()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new FilmRepository(context, _mockLogger.Object);

            // Act
            var result = await repository.GetAllAsync();
            var films = result.ToList();

            // Assert
            Assert.Equal(2, films.Count);
            Assert.Contains(films, f => f.Id == 1);
            Assert.Contains(films, f => f.Id == 2);
            Assert.DoesNotContain(films, f => f.Id == 3); // Inactive film should not be returned
            Assert.All(films, film => Assert.True(film.IsActive));
        }

        [Fact]
        public async Task GetByIdAsync_WithValidIdAndActiveFilm_ShouldReturnFilm()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new FilmRepository(context, _mockLogger.Object);

            // Act
            var film = await repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(film);
            Assert.Equal(1, film.Id);
            Assert.Equal("The Shawshank Redemption", film.Name);
            Assert.True(film.IsActive);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidIdButInactiveFilm_ShouldReturnNull()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new FilmRepository(context, _mockLogger.Object);

            // Act
            var film = await repository.GetByIdAsync(3); // Id 3 exists but is inactive

            // Assert
            Assert.Null(film);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new FilmRepository(context, _mockLogger.Object);

            // Act
            var film = await repository.GetByIdAsync(999); // Non-existent id

            // Assert
            Assert.Null(film);
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewFilmAndReturnIt()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new FilmRepository(context, _mockLogger.Object);

            var newFilm = new Film
            {
                Name = "Inception",
                Description = "A thief who steals corporate secrets through dream-sharing technology.",
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "system"
            };

            // Act
            var result = await repository.AddAsync(newFilm);

            // Assert
            Assert.NotEqual(0, result.Id); // Should have an ID assigned

            // Verify it was actually added to the database
            var filmInDb = await context.Films.FindAsync(result.Id);
            Assert.NotNull(filmInDb);
            Assert.Equal("Inception", filmInDb.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingFilm()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new FilmRepository(context, _mockLogger.Object);

            // First retrieve the film
            var film = await context.Films.FindAsync(1);
            Assert.NotNull(film);

            // Modify it
            film.Name = "Updated Film Name";
            film.Description = "Updated description";
            film.ModifiedDate = DateTime.UtcNow;
            film.ModifiedBy = "test_user";

            // Act
            await repository.UpdateAsync(film);

            // Assert - Get a fresh instance from the context to verify the update
            context.Entry(film).State = EntityState.Detached; // Detach the entity so we can get a fresh copy
            var updatedFilm = await context.Films.FindAsync(1);

            Assert.NotNull(updatedFilm);
            Assert.Equal("Updated Film Name", updatedFilm.Name);
            Assert.Equal("Updated description", updatedFilm.Description);
            Assert.Equal("test_user", updatedFilm.ModifiedBy);
            Assert.NotNull(updatedFilm.ModifiedDate);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSetIsActiveToFalse()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new FilmRepository(context, _mockLogger.Object);

            // Act
            await repository.DeleteAsync(1);

            // Assert
            var film = await context.Films.FindAsync(1);
            Assert.NotNull(film);
            Assert.False(film.IsActive);
            Assert.NotNull(film.ModifiedDate);
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistentId_ShouldNotThrowException()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new FilmRepository(context, _mockLogger.Object);

            // Act & Assert - should not throw
            await repository.DeleteAsync(999);
        }

        [Fact]
        public async Task ExistsAsync_WithExistingActiveFilm_ShouldReturnTrue()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new FilmRepository(context, _mockLogger.Object);

            // Act
            var exists = await repository.ExistsAsync(1);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_WithExistingInactiveFilm_ShouldReturnFalse()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new FilmRepository(context, _mockLogger.Object);

            // Act
            var exists = await repository.ExistsAsync(3); // ID 3 exists but is inactive

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task ExistsAsync_WithNonExistentId_ShouldReturnFalse()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new FilmRepository(context, _mockLogger.Object);

            // Act
            var exists = await repository.ExistsAsync(999);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMatchingActiveFilms()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new FilmRepository(context, _mockLogger.Object);

            // Act - search by name
            var resultsByName = await repository.SearchAsync("Godfather");

            // Assert
            var filmsList = resultsByName.ToList();
            Assert.Single(filmsList);
            Assert.Equal("The Godfather", filmsList[0].Name);

            // Act - search by description
            var resultsByDescription = await repository.SearchAsync("imprisoned");

            // Assert
            filmsList = resultsByDescription.ToList();
            Assert.Single(filmsList);
            Assert.Equal("The Shawshank Redemption", filmsList[0].Name);

            // Act - search with no matches
            var noResults = await repository.SearchAsync("xyz123");

            // Assert
            Assert.Empty(noResults);
        }

        [Fact]
        public async Task SearchAsync_ShouldNotReturnInactiveFilms()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new FilmRepository(context, _mockLogger.Object);

            // Act - search by term that would match an inactive film
            var results = await repository.SearchAsync("Joker");

            // Assert
            Assert.Empty(results); // The Dark Knight contains "Joker" but is inactive
        }

        [Fact]
        public async Task Repository_WithCancellationToken_ShouldRespectCancellation()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new FilmRepository(context, _mockLogger.Object);

            // Create a cancellation token and cancel it
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert - should throw OperationCanceledException or TaskCanceledException
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
                repository.GetAllAsync(cts.Token));
        }
    }
}