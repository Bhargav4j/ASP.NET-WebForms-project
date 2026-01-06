using Films.Domain.Entities;

namespace Films.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Sex entity operations
/// </summary>
public interface ISexRepository
{
    Task<IEnumerable<Sex>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Sex?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Sex> AddAsync(Sex entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(Sex entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
