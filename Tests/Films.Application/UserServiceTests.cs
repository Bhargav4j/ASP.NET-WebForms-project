using Xunit;
using Moq;
using Films.Application.Services;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Films.Application.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger<UserService>>();
        _service = new UserService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithNullRepository_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new UserService(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new UserService(_mockRepository.Object, null!));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Username = "user1" },
            new User { Id = 2, Username = "user2" }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsUser()
    {
        // Arrange
        var user = new User { Id = 1, Username = "testuser" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("testuser", result.Username);
        _mockRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidUser_CreatesUser()
    {
        // Arrange
        var user = new User { Username = "newuser", Password = "password123" };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User u, CancellationToken ct) => { u.Id = 1; return u; });

        // Act
        var result = await _service.CreateAsync(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.True(result.IsActive);
        Assert.NotEqual(default(DateTime), result.CreatedDate);
        Assert.NotEqual("password123", result.Password); // Password should be hashed
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithNullUser_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _service.CreateAsync(null!));
    }

    [Fact]
    public async Task UpdateAsync_WithValidUser_UpdatesUser()
    {
        // Arrange
        var user = new User { Id = 1, Username = "updateduser" };
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(user);

        // Assert
        Assert.NotNull(user.ModifiedDate);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithNullUser_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _service.UpdateAsync(null!));
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_DeletesUser()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_WithValidSearchTerm_ReturnsMatchingUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Username = "admin" },
            new User { Id = 2, Username = "administrator" }
        };
        _mockRepository.Setup(r => r.SearchAsync("admin", It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await _service.SearchAsync("admin");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.SearchAsync("admin", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AuthenticateAsync_WithValidCredentials_ReturnsUser()
    {
        // Arrange
        var user = new User { Id = 1, Username = "testuser", Password = "hashedpassword" };
        _mockRepository.Setup(r => r.AuthenticateAsync("testuser", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _service.AuthenticateAsync("testuser", "password");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("testuser", result.Username);
        _mockRepository.Verify(r => r.AuthenticateAsync("testuser", It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AuthenticateAsync_WithInvalidCredentials_ReturnsNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.AuthenticateAsync("testuser", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _service.AuthenticateAsync("testuser", "wrongpassword");

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.AuthenticateAsync("testuser", It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RecoverPasswordBySecretAsync_WithCorrectSecret_ReturnsUser()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            SecretQuestion = "Pet name?",
            SecretAnswer = "Fluffy"
        };
        _mockRepository.Setup(r => r.GetByUsernameAsync("testuser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _service.RecoverPasswordBySecretAsync("testuser", "Fluffy");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("testuser", result.Username);
        _mockRepository.Verify(r => r.GetByUsernameAsync("testuser", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RecoverPasswordBySecretAsync_WithIncorrectSecret_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            SecretQuestion = "Pet name?",
            SecretAnswer = "Fluffy"
        };
        _mockRepository.Setup(r => r.GetByUsernameAsync("testuser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _service.RecoverPasswordBySecretAsync("testuser", "WrongAnswer");

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.GetByUsernameAsync("testuser", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RecoverPasswordByPhoneAsync_WithCorrectPhone_ReturnsUser()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            PhoneNumber = "1234567890"
        };
        _mockRepository.Setup(r => r.GetByUsernameAsync("testuser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _service.RecoverPasswordByPhoneAsync("testuser", "1234567890");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("testuser", result.Username);
        _mockRepository.Verify(r => r.GetByUsernameAsync("testuser", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RecoverPasswordByPhoneAsync_WithIncorrectPhone_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            PhoneNumber = "1234567890"
        };
        _mockRepository.Setup(r => r.GetByUsernameAsync("testuser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _service.RecoverPasswordByPhoneAsync("testuser", "0000000000");

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.GetByUsernameAsync("testuser", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_HashesPassword()
    {
        // Arrange
        var user = new User { Username = "testuser", Password = "plainpassword" };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User u, CancellationToken ct) => u);

        // Act
        var result = await _service.CreateAsync(user);

        // Assert
        Assert.NotEqual("plainpassword", result.Password);
        Assert.NotEmpty(result.Password);
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryThrowsException_RethrowsException()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.GetAllAsync());
    }
}
