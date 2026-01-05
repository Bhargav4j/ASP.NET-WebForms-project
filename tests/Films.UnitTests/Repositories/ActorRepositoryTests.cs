using Films.Domain.Entities;
using Films.Infrastructure.Data;
using Films.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Films.UnitTests.Repositories;

public class ActorRepositoryTests
{
    private readonly Mock<ILogger<ActorRepository>> _mockLogger;
    private readonly DbContextOptions<FilmsDbContext> _options;

    public ActorRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<ActorRepository>>();
        _options = new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void ActorRepository_Constructor_ShouldThrowOnNullContext()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ActorRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void ActorRepository_Constructor_ShouldThrowOnNullLogger()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ActorRepository(context, null!));
    }

    [Fact]
    public async Task ActorRepository_GetAllAsync_ShouldReturnActiveActors()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new ActorRepository(context, _mockLogger.Object);

        context.Actors.AddRange(
            new Actor { Name = "Actor1", IsActive = true, CreatedBy = "test" },
            new Actor { Name = "Actor2", IsActive = true, CreatedBy = "test" },
            new Actor { Name = "Actor3", IsActive = false, CreatedBy = "test" }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ActorRepository_GetByIdAsync_ShouldReturnActor()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new ActorRepository(context, _mockLogger.Object);

        var actor = new Actor { Name = "Test Actor", IsActive = true, CreatedBy = "test" };
        context.Actors.Add(actor);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(actor.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Actor", result.Name);
    }

    [Fact]
    public async Task ActorRepository_GetByIdAsync_ShouldReturnNullForInactiveActor()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new ActorRepository(context, _mockLogger.Object);

        var actor = new Actor { Name = "Inactive Actor", IsActive = false, CreatedBy = "test" };
        context.Actors.Add(actor);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(actor.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ActorRepository_AddAsync_ShouldAddActor()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new ActorRepository(context, _mockLogger.Object);
        var actor = new Actor { Name = "New Actor", CreatedBy = "test" };

        // Act
        var result = await repository.AddAsync(actor);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.True(result.IsActive);
        Assert.NotEqual(default(DateTime), result.CreatedDate);
    }

    [Fact]
    public async Task ActorRepository_UpdateAsync_ShouldUpdateActor()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new ActorRepository(context, _mockLogger.Object);

        var actor = new Actor { Name = "Original Name", IsActive = true, CreatedBy = "test" };
        context.Actors.Add(actor);
        await context.SaveChangesAsync();

        // Act
        actor.Name = "Updated Name";
        await repository.UpdateAsync(actor);

        // Assert
        var updatedActor = await context.Actors.FindAsync(actor.Id);
        Assert.NotNull(updatedActor);
        Assert.Equal("Updated Name", updatedActor.Name);
        Assert.NotNull(updatedActor.ModifiedDate);
    }

    [Fact]
    public async Task ActorRepository_DeleteAsync_ShouldSoftDeleteActor()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new ActorRepository(context, _mockLogger.Object);

        var actor = new Actor { Name = "To Delete", IsActive = true, CreatedBy = "test" };
        context.Actors.Add(actor);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(actor.Id);

        // Assert
        var deletedActor = await context.Actors.FindAsync(actor.Id);
        Assert.NotNull(deletedActor);
        Assert.False(deletedActor.IsActive);
        Assert.NotNull(deletedActor.ModifiedDate);
    }

    [Fact]
    public async Task ActorRepository_ExistsAsync_ShouldReturnTrueForActiveActor()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new ActorRepository(context, _mockLogger.Object);

        var actor = new Actor { Name = "Existing Actor", IsActive = true, CreatedBy = "test" };
        context.Actors.Add(actor);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ExistsAsync(actor.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ActorRepository_ExistsAsync_ShouldReturnFalseForInactiveActor()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new ActorRepository(context, _mockLogger.Object);

        var actor = new Actor { Name = "Inactive Actor", IsActive = false, CreatedBy = "test" };
        context.Actors.Add(actor);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ExistsAsync(actor.Id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ActorRepository_SearchAsync_ShouldReturnMatchingActors()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new ActorRepository(context, _mockLogger.Object);

        context.Actors.AddRange(
            new Actor { Name = "Tom Hanks", IsActive = true, CreatedBy = "test" },
            new Actor { Name = "Tom Cruise", IsActive = true, CreatedBy = "test" },
            new Actor { Name = "Brad Pitt", IsActive = true, CreatedBy = "test" }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("Tom");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ActorRepository_SearchAsync_ShouldSearchInDescription()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new ActorRepository(context, _mockLogger.Object);

        context.Actors.AddRange(
            new Actor { Name = "Actor1", Description = "Oscar winner", IsActive = true, CreatedBy = "test" },
            new Actor { Name = "Actor2", Description = "Emmy winner", IsActive = true, CreatedBy = "test" }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("Oscar");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, a => a.Name == "Actor1");
    }

    [Fact]
    public async Task ActorRepository_GetAllAsync_ShouldIncludeSex()
    {
        // Arrange
        using var context = new FilmsDbContext(_options);
        var repository = new ActorRepository(context, _mockLogger.Object);

        var sex = new Sex { Name = "Male", IsActive = true, CreatedBy = "test" };
        context.Sexes.Add(sex);
        await context.SaveChangesAsync();

        var actor = new Actor { Name = "Actor with Sex", IsActive = true, CreatedBy = "test", SexId = sex.Id };
        context.Actors.Add(actor);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        var actorWithSex = result.FirstOrDefault(a => a.Name == "Actor with Sex");
        Assert.NotNull(actorWithSex);
        Assert.NotNull(actorWithSex.Sex);
        Assert.Equal("Male", actorWithSex.Sex.Name);
    }
}
