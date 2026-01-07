using Films.Domain.Entities;

namespace Films.Domain.Interfaces.Repositories;

public interface IActorRepository
{
    Task<IEnumerable<Actor>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Actor?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Actor> AddAsync(Actor actor, CancellationToken cancellationToken = default);
    Task UpdateAsync(Actor actor, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Actor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
