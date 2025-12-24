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
    public class ActorRepositoryTests
    {
        private readonly DbContextOptions<FilmsDbContext> _options;
        private readonly Mock<ILogger<ActorRepository>> _mockLogger;

        public ActorRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<FilmsDbContext>()
                .UseInMemoryDatabase(databaseName: $"FilmsDbForActorTesting_{Guid.NewGuid()}")
                .Options;

            _mockLogger = new Mock<ILogger<ActorRepository>>();

            // Seed the database
            using (var context = new FilmsDbContext(_options))
            {
                context.Database.EnsureCreated();

                var sexes = new List<Sex>
                {
                    new Sex { Id = 1, Name = "Male", IsActive = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                    new Sex { Id = 2, Name = "Female", IsActive = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow }
                };

                var actors = new List<Actor>
                {
                    new Actor
                    {
                        Id = 1,
                        Name = "Brad",
                        Surname = "Pitt",
                        IdSex = 1,
                        CreatedDate = DateTime.UtcNow.AddDays(-10),
                        IsActive = true,
                        CreatedBy = "system"
                    },
                    new Actor
                    {
                        Id = 2,
                        Name = "Angelina",
                        Surname = "Jolie",
                        IdSex = 2,
                        CreatedDate = DateTime.UtcNow.AddDays(-9),
                        IsActive = true,
                        CreatedBy = "system"
                    },
                    new Actor
                    {
                        Id = 3,
                        Name = "Leonardo",
                        Surname = "DiCaprio",
                        IdSex = 1,
                        CreatedDate = DateTime.UtcNow.AddDays(-8),
                        IsActive = false,
                        CreatedBy = "system"
                    }
                };

                context.Sexes.AddRange(sexes);
                context.SaveChanges();

                context.Actors.AddRange(actors);
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOnlyActiveActors()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new ActorRepository(context, _mockLogger.Object);

            // Act
            var result = await repository.GetAllAsync();
            var actors = result.ToList();

            // Assert
            Assert.Equal(2, actors.Count);
            Assert.Contains(actors, a => a.Id == 1);
            Assert.Contains(actors, a => a.Id == 2);
            Assert.DoesNotContain(actors, a => a.Id == 3); // Inactive actor should not be returned
            Assert.All(actors, actor => Assert.True(actor.IsActive));
        }

        [Fact]
        public async Task GetAllAsync_ShouldIncludeRelatedSexData()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new ActorRepository(context, _mockLogger.Object);

            // Act
            var result = await repository.GetAllAsync();
            var actors = result.ToList();

            // Assert
            Assert.All(actors, actor => Assert.NotNull(actor.Sex));
            Assert.Equal("Male", actors.First(a => a.Id == 1).Sex.Name);
            Assert.Equal("Female", actors.First(a => a.Id == 2).Sex.Name);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidIdAndActiveActor_ShouldReturnActor()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new ActorRepository(context, _mockLogger.Object);

            // Act
            var actor = await repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(actor);
            Assert.Equal(1, actor.Id);
            Assert.Equal("Brad", actor.Name);
            Assert.Equal("Pitt", actor.Surname);
            Assert.True(actor.IsActive);
            Assert.NotNull(actor.Sex);
            Assert.Equal("Male", actor.Sex.Name);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidIdButInactiveActor_ShouldReturnNull()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new ActorRepository(context, _mockLogger.Object);

            // Act
            var actor = await repository.GetByIdAsync(3); // Id 3 exists but is inactive

            // Assert
            Assert.Null(actor);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new ActorRepository(context, _mockLogger.Object);

            // Act
            var actor = await repository.GetByIdAsync(999); // Non-existent id

            // Assert
            Assert.Null(actor);
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewActorAndReturnIt()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new ActorRepository(context, _mockLogger.Object);

            var newActor = new Actor
            {
                Name = "Tom",
                Surname = "Hanks",
                IdSex = 1,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "system"
            };

            // Act
            var result = await repository.AddAsync(newActor);

            // Assert
            Assert.NotEqual(0, result.Id); // Should have an ID assigned

            // Verify it was actually added to the database
            var actorInDb = await context.Actors.FindAsync(result.Id);
            Assert.NotNull(actorInDb);
            Assert.Equal("Tom", actorInDb.Name);
            Assert.Equal("Hanks", actorInDb.Surname);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingActor()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new ActorRepository(context, _mockLogger.Object);

            // First retrieve the actor
            var actor = await context.Actors.FindAsync(1);
            Assert.NotNull(actor);

            // Modify it
            actor.Name = "Updated Name";
            actor.Surname = "Updated Surname";
            actor.IdSex = 2;
            actor.ModifiedDate = DateTime.UtcNow;
            actor.ModifiedBy = "test_user";

            // Act
            await repository.UpdateAsync(actor);

            // Assert - Get a fresh instance from the context to verify the update
            context.Entry(actor).State = EntityState.Detached; // Detach the entity so we can get a fresh copy
            var updatedActor = await context.Actors.FindAsync(1);

            Assert.NotNull(updatedActor);
            Assert.Equal("Updated Name", updatedActor.Name);
            Assert.Equal("Updated Surname", updatedActor.Surname);
            Assert.Equal(2, updatedActor.IdSex);
            Assert.Equal("test_user", updatedActor.ModifiedBy);
            Assert.NotNull(updatedActor.ModifiedDate);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSetIsActiveToFalse()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new ActorRepository(context, _mockLogger.Object);

            // Act
            await repository.DeleteAsync(1);

            // Assert
            var actor = await context.Actors.FindAsync(1);
            Assert.NotNull(actor);
            Assert.False(actor.IsActive);
            Assert.NotNull(actor.ModifiedDate);
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistentId_ShouldNotThrowException()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new ActorRepository(context, _mockLogger.Object);

            // Act & Assert - should not throw
            await repository.DeleteAsync(999);
        }

        [Fact]
        public async Task ExistsAsync_WithExistingActiveActor_ShouldReturnTrue()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new ActorRepository(context, _mockLogger.Object);

            // Act
            var exists = await repository.ExistsAsync(1);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_WithExistingInactiveActor_ShouldReturnFalse()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new ActorRepository(context, _mockLogger.Object);

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
            var repository = new ActorRepository(context, _mockLogger.Object);

            // Act
            var exists = await repository.ExistsAsync(999);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMatchingActiveActors_ByName()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new ActorRepository(context, _mockLogger.Object);

            // Act - search by name
            var resultsByName = await repository.SearchAsync("Brad");

            // Assert
            var actorsList = resultsByName.ToList();
            Assert.Single(actorsList);
            Assert.Equal("Brad", actorsList[0].Name);
            Assert.Equal("Pitt", actorsList[0].Surname);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMatchingActiveActors_BySurname()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new ActorRepository(context, _mockLogger.Object);

            // Act - search by surname
            var resultsBySurname = await repository.SearchAsync("Jolie");

            // Assert
            var actorsList = resultsBySurname.ToList();
            Assert.Single(actorsList);
            Assert.Equal("Angelina", actorsList[0].Name);
            Assert.Equal("Jolie", actorsList[0].Surname);
        }

        [Fact]
        public async Task SearchAsync_ShouldNotReturnInactiveActors()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new ActorRepository(context, _mockLogger.Object);

            // Act - search by term that would match an inactive actor
            var results = await repository.SearchAsync("DiCaprio");

            // Assert
            Assert.Empty(results); // Leonardo DiCaprio is inactive
        }

        [Fact]
        public async Task SearchAsync_WithNoMatches_ShouldReturnEmptyCollection()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new ActorRepository(context, _mockLogger.Object);

            // Act - search with no matches
            var noResults = await repository.SearchAsync("xyz123");

            // Assert
            Assert.Empty(noResults);
        }

        [Fact]
        public async Task Repository_WithCancellationToken_ShouldRespectCancellation()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var repository = new ActorRepository(context, _mockLogger.Object);

            // Create a cancellation token and cancel it
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert - should throw OperationCanceledException or TaskCanceledException
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
                repository.GetAllAsync(cts.Token));
        }
    }
}