using Xunit;
using Films.Infrastructure.Repositories;
using Films.Infrastructure.Data;
using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Films.Infrastructure.Tests;

public class UserRepositoryTests
{
    private readonly FilmsDbContext _context;
    private readonly Mock<ILogger<UserRepository>> _mockLogger;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new FilmsDbContext(options);
        _mockLogger = new Mock<ILogger<UserRepository>>();
        _repository = new UserRepository(_context, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithNullContext_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new UserRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new UserRepository(_context, null!));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllActiveUsers()
    {
        // Arrange
        _context.Users.AddRange(
            new User { Id = 1, Username = "user1", Password = "pass1", Email = "user1@test.com", IsActive = true },
            new User { Id = 2, Username = "user2", Password = "pass2", Email = "user2@test.com", IsActive = true },
            new User { Id = 3, Username = "user3", Password = "pass3", Email = "user3@test.com", IsActive = false }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsUser()
    {
        // Arrange
        var user = new User { Id = 1, Username = "testuser", Password = "pass", Email = "test@test.com", IsActive = true };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testuser", result.Username);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange & Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUsernameAsync_WithValidUsername_ReturnsUser()
    {
        // Arrange
        var user = new User { Username = "testuser", Password = "pass", Email = "test@test.com", IsActive = true };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByUsernameAsync("testuser");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testuser", result.Username);
    }

    [Fact]
    public async Task GetByUsernameAsync_WithInvalidUsername_ReturnsNull()
    {
        // Arrange & Act
        var result = await _repository.GetByUsernameAsync("nonexistent");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsUserToDatabase()
    {
        // Arrange
        var user = new User { Username = "newuser", Password = "pass", Email = "new@test.com", IsActive = true };

        // Act
        var result = await _repository.AddAsync(user);

        // Assert
        Assert.NotEqual(0, result.Id);
        Assert.Equal(1, _context.Users.Count());
    }

    [Fact]
    public async Task UpdateAsync_UpdatesExistingUser()
    {
        // Arrange
        var user = new User { Username = "original", Password = "pass", Email = "orig@test.com", IsActive = true };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        _context.Entry(user).State = EntityState.Detached;

        user.Username = "updated";

        // Act
        await _repository.UpdateAsync(user);

        // Assert
        var updated = await _context.Users.FindAsync(user.Id);
        Assert.Equal("updated", updated?.Username);
    }

    [Fact]
    public async Task DeleteAsync_SetsIsActiveToFalse()
    {
        // Arrange
        var user = new User { Username = "todelete", Password = "pass", Email = "del@test.com", IsActive = true };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(user.Id);

        // Assert
        var deleted = await _context.Users.FindAsync(user.Id);
        Assert.False(deleted?.IsActive);
        Assert.NotNull(deleted?.ModifiedDate);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_DoesNotThrow()
    {
        // Arrange & Act
        await _repository.DeleteAsync(999);

        // Assert - no exception thrown
        Assert.True(true);
    }

    [Fact]
    public async Task ExistsAsync_WithExistingId_ReturnsTrue()
    {
        // Arrange
        var user = new User { Username = "testuser", Password = "pass", Email = "test@test.com", IsActive = true };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync(user.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_WithNonExistingId_ReturnsFalse()
    {
        // Arrange & Act
        var result = await _repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_WithInactiveUser_ReturnsFalse()
    {
        // Arrange
        var user = new User { Username = "inactive", Password = "pass", Email = "inactive@test.com", IsActive = false };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync(user.Id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_WithMatchingUsername_ReturnsUsers()
    {
        // Arrange
        _context.Users.AddRange(
            new User { Username = "admin", Password = "pass1", Email = "admin@test.com", IsActive = true },
            new User { Username = "administrator", Password = "pass2", Email = "admin2@test.com", IsActive = true },
            new User { Username = "user", Password = "pass3", Email = "user@test.com", IsActive = true }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("admin");

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task SearchAsync_WithMatchingEmail_ReturnsUsers()
    {
        // Arrange
        _context.Users.AddRange(
            new User { Username = "user1", Password = "pass1", Email = "test@example.com", IsActive = true },
            new User { Username = "user2", Password = "pass2", Email = "test@sample.com", IsActive = true },
            new User { Username = "user3", Password = "pass3", Email = "other@example.com", IsActive = true }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("test@");

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task SearchAsync_WithEmptySearchTerm_ReturnsAllUsers()
    {
        // Arrange
        _context.Users.AddRange(
            new User { Username = "user1", Password = "pass1", Email = "user1@test.com", IsActive = true },
            new User { Username = "user2", Password = "pass2", Email = "user2@test.com", IsActive = true }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("");

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task AuthenticateAsync_WithValidCredentials_ReturnsUser()
    {
        // Arrange
        var user = new User
        {
            Username = "testuser",
            Password = "hashedpass",
            Email = "test@test.com",
            IsActive = true
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.AuthenticateAsync("testuser", "hashedpass");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testuser", result.Username);
    }

    [Fact]
    public async Task AuthenticateAsync_WithInvalidPassword_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Username = "testuser",
            Password = "hashedpass",
            Email = "test@test.com",
            IsActive = true
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.AuthenticateAsync("testuser", "wrongpass");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AuthenticateAsync_WithInactiveUser_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Username = "testuser",
            Password = "hashedpass",
            Email = "test@test.com",
            IsActive = false
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.AuthenticateAsync("testuser", "hashedpass");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsUsersOrderedByUsername()
    {
        // Arrange
        _context.Users.AddRange(
            new User { Username = "zebra", Password = "pass1", Email = "z@test.com", IsActive = true },
            new User { Username = "alpha", Password = "pass2", Email = "a@test.com", IsActive = true },
            new User { Username = "beta", Password = "pass3", Email = "b@test.com", IsActive = true }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetAllAsync()).ToList();

        // Assert
        Assert.Equal("alpha", result[0].Username);
        Assert.Equal("beta", result[1].Username);
        Assert.Equal("zebra", result[2].Username);
    }

    [Fact]
    public async Task GetAllAsync_IncludesTypeUserNavigationProperty()
    {
        // Arrange
        var typeUser = new TypeUser { Id = 1, Name = "Admin", IsActive = true };
        _context.TypeUsers.Add(typeUser);
        await _context.SaveChangesAsync();

        var user = new User
        {
            Username = "admin",
            Password = "pass",
            Email = "admin@test.com",
            TypeUserId = 1,
            IsActive = true
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetAllAsync()).First();

        // Assert
        Assert.NotNull(result.TypeUser);
        Assert.Equal("Admin", result.TypeUser.Name);
    }
}
