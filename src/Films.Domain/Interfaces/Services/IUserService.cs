using Films.Domain.DTOs;

namespace Films.Domain.Interfaces.Services;

/// <summary>
/// Service interface for User operations
/// </summary>
public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UserDto> CreateAsync(UserCreateDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, UserUpdateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
