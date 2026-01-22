using Microsoft.Extensions.Logging;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Domain.Interfaces.Services;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for Director entity operations
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
            _logger.LogInformation("Getting all directors");
            return await _repository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all directors");
            throw;
        }
    }

    public async Task<Director?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting director by id: {DirectorId}", id);
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting director by id: {DirectorId}", id);
            throw;
        }
    }

    public async Task<Director> CreateAsync(Director director, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating director: {DirectorName}", director.Name);

            if (string.IsNullOrWhiteSpace(director.Name))
            {
                throw new ArgumentException("Director name is required", nameof(director));
            }

            return await _repository.AddAsync(director, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating director: {DirectorName}", director.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, Director director, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating director: {DirectorId}", id);

            var existingDirector = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingDirector == null)
            {
                throw new InvalidOperationException($"Director with id {id} not found");
            }

            director.Id = id;
            await _repository.UpdateAsync(director, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating director: {DirectorId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting director: {DirectorId}", id);

            var existingDirector = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingDirector == null)
            {
                throw new InvalidOperationException($"Director with id {id} not found");
            }

            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting director: {DirectorId}", id);
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
