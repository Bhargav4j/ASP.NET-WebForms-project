using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for Director business operations
/// </summary>
public class DirectorService : IDirectorService
{
    private readonly IDirectorRepository _repository;
    private readonly ILogger<DirectorService> _logger;

    public DirectorService(IDirectorRepository repository, ILogger<DirectorService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Director>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all directors");
            return await _repository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all directors");
            throw;
        }
    }

    public async Task<Director?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving director with ID: {DirectorId}", id);
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving director with ID: {DirectorId}", id);
            throw;
        }
    }

    public async Task<Director> CreateAsync(Director director, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new director: {DirectorName}", $"{director.FirstName} {director.LastName}");
            director.CreatedDate = DateTime.UtcNow;
            director.IsActive = true;
            return await _repository.AddAsync(director, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating director: {DirectorName}", $"{director.FirstName} {director.LastName}");
            throw;
        }
    }

    public async Task UpdateAsync(int id, Director director, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating director with ID: {DirectorId}", id);
            var existingDirector = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingDirector == null)
            {
                throw new InvalidOperationException($"Director with ID {id} not found");
            }

            director.Id = id;
            director.ModifiedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(director, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating director with ID: {DirectorId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting director with ID: {DirectorId}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting director with ID: {DirectorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Director>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching directors with term: {SearchTerm}", searchTerm);
            return await _repository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching directors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
