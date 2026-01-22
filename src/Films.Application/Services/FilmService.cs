using Microsoft.Extensions.Logging;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Domain.Interfaces.Services;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for Film entity operations
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
            _logger.LogInformation("Getting all films");
            return await _repository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all films");
            throw;
        }
    }

    public async Task<Film?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting film by id: {FilmId}", id);
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting film by id: {FilmId}", id);
            throw;
        }
    }

    public async Task<Film> CreateAsync(Film film, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating film: {FilmName}", film.Name);

            if (string.IsNullOrWhiteSpace(film.Name))
            {
                throw new ArgumentException("Film name is required", nameof(film));
            }

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
            _logger.LogInformation("Updating film: {FilmId}", id);

            var existingFilm = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingFilm == null)
            {
                throw new InvalidOperationException($"Film with id {id} not found");
            }

            film.Id = id;
            await _repository.UpdateAsync(film, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film: {FilmId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting film: {FilmId}", id);

            var existingFilm = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingFilm == null)
            {
                throw new InvalidOperationException($"Film with id {id} not found");
            }

            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film: {FilmId}", id);
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
