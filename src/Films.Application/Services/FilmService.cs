using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for Film business operations
/// </summary>
public class FilmService : IFilmService
{
    private readonly IFilmRepository _repository;
    private readonly ILogger<FilmService> _logger;

    public FilmService(IFilmRepository repository, ILogger<FilmService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Film>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all films");
            return await _repository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all films");
            throw;
        }
    }

    public async Task<Film?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving film with ID: {FilmId}", id);
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving film with ID: {FilmId}", id);
            throw;
        }
    }

    public async Task<Film> CreateAsync(Film film, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new film: {FilmName}", film.Name);
            film.CreatedDate = DateTime.UtcNow;
            film.IsActive = true;
            return await _repository.AddAsync(film, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating film: {FilmName}", film.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, Film film, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating film with ID: {FilmId}", id);
            var existingFilm = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingFilm == null)
            {
                throw new InvalidOperationException($"Film with ID {id} not found");
            }

            film.Id = id;
            film.ModifiedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(film, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film with ID: {FilmId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting film with ID: {FilmId}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film with ID: {FilmId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Film>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching films with term: {SearchTerm}", searchTerm);
            return await _repository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching films with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
