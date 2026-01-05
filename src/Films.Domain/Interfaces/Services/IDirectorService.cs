using Films.Domain.DTOs;

namespace Films.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Director business operations
/// </summary>
public interface IDirectorService
{
    Task<IEnumerable<DirectorDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DirectorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<DirectorDto> CreateAsync(DirectorCreateDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, DirectorUpdateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DirectorDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
