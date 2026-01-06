using Films.Domain.Entities;

namespace Films.Domain.Interfaces.Services;

/// <summary>
/// Service interface for User operations
/// </summary>
public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);

    Task UpdateAsync(User user, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<IEnumerable<User>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

    Task<User?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default);

    Task<User?> RecoverPasswordBySecretAsync(string username, string secretAnswer, CancellationToken cancellationToken = default);

    Task<User?> RecoverPasswordByPhoneAsync(string username, string phoneNumber, CancellationToken cancellationToken = default);
}
