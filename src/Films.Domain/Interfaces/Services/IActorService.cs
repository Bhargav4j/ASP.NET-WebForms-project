using Films.Domain.DTOs;

namespace Films.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Actor operations
/// </summary>
public interface IActorService
{
    Task<IEnumerable<ActorDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ActorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ActorDto> CreateAsync(ActorCreateDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, ActorUpdateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ActorDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
