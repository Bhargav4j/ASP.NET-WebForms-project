using Films.Domain.Entities;

namespace Films.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for TypeUser entity operations
/// </summary>
public interface ITypeUserRepository
{
    Task<IEnumerable<TypeUser>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TypeUser?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<TypeUser> AddAsync(TypeUser entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(TypeUser entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
