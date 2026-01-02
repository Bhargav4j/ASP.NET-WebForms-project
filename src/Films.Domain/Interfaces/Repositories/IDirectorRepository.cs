using Films.Domain.Entities;

namespace Films.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Director entity operations
/// </summary>
public interface IDirectorRepository
{
    Task<IEnumerable<Director>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Director?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Director> AddAsync(Director director, CancellationToken cancellationToken = default);
    Task UpdateAsync(Director director, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Director>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
