using Films.Domain.Entities;

namespace Films.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Film operations
/// </summary>
public interface IFilmService
{
    Task<IEnumerable<Film>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Film?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Film> CreateAsync(Film film, CancellationToken cancellationToken = default);

    Task UpdateAsync(Film film, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Film>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
