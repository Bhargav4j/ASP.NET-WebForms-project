using Films.Domain.Entities;

namespace Films.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Director business operations
/// </summary>
public interface IDirectorService
{
    Task<IEnumerable<Director>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Director?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Director> CreateAsync(Director director, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, Director director, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Director>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
