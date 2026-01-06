using Films.Domain.Entities;

namespace Films.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Actor operations
/// </summary>
public interface IActorService
{
    Task<IEnumerable<Actor>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Actor?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Actor> CreateAsync(Actor actor, CancellationToken cancellationToken = default);

    Task UpdateAsync(Actor actor, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Actor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
