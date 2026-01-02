using Films.Application.DTOs;

namespace Films.Application.Interfaces;

/// <summary>
/// Service interface for Film business logic operations
/// </summary>
public interface IFilmService
{
    Task<IEnumerable<FilmDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<FilmDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<FilmDto> CreateAsync(FilmCreateDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, FilmUpdateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FilmDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
