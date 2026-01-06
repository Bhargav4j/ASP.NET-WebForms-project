using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for User operations
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository repository, ILogger<UserService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all users");
            return await _repository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all users");
            throw;
        }
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting user with ID {UserId}", id);
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user with ID {UserId}", id);
            throw;
        }
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _logger.LogInformation("Creating new user: {Username}", user.Username);
            user.CreatedDate = DateTime.UtcNow;
            user.IsActive = true;

            // Hash password before storing (simplified - in production use proper hashing)
            user.Password = HashPassword(user.Password);

            return await _repository.AddAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user: {Username}", user?.Username);
            throw;
        }
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _logger.LogInformation("Updating user with ID {UserId}", user.Id);
            user.ModifiedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID {UserId}", user?.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user with ID {UserId}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with ID {UserId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<User>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching users with term: {SearchTerm}", searchTerm);
            return await _repository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<User?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Authenticating user: {Username}", username);
            var hashedPassword = HashPassword(password);
            return await _repository.AuthenticateAsync(username, hashedPassword, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authenticating user: {Username}", username);
            throw;
        }
    }

    public async Task<User?> RecoverPasswordBySecretAsync(string username, string secretAnswer, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Recovering password by secret for user: {Username}", username);
            var user = await _repository.GetByUsernameAsync(username, cancellationToken);

            if (user != null && user.SecretAnswer?.Equals(secretAnswer, StringComparison.OrdinalIgnoreCase) == true)
            {
                return user;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recovering password by secret for user: {Username}", username);
            throw;
        }
    }

    public async Task<User?> RecoverPasswordByPhoneAsync(string username, string phoneNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Recovering password by phone for user: {Username}", username);
            var user = await _repository.GetByUsernameAsync(username, cancellationToken);

            if (user != null && user.PhoneNumber == phoneNumber)
            {
                return user;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recovering password by phone for user: {Username}", username);
            throw;
        }
    }

    private string HashPassword(string password)
    {
        // Simplified password hashing - in production use BCrypt, Argon2, or ASP.NET Core Identity
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}
