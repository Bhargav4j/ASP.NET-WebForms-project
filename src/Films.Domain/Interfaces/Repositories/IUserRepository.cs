using Films.Domain.Entities;

namespace Films.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for User entity operations
/// </summary>
public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

    Task<User> AddAsync(User entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(User entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    Task<IEnumerable<User>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

    Task<User?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default);
}
