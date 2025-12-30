using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Films.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Film entity
/// </summary>
public class FilmRepository : IFilmRepository
{
    private readonly FilmsDbContext _context;
    private readonly ILogger<FilmRepository> _logger;

    public FilmRepository(FilmsDbContext context, ILogger<FilmRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Film>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all active films");
            return await _context.Films
                .AsNoTracking()
                .Where(f => f.IsActive)
                .Include(f => f.ActorFilms)
                .Include(f => f.DirectorFilms)
                .ToListAsync(cancellationToken);
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
            return await _context.Films
                .AsNoTracking()
                .Include(f => f.ActorFilms)
                .Include(f => f.DirectorFilms)
                .FirstOrDefaultAsync(f => f.Id == id && f.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving film with ID: {FilmId}", id);
            throw;
        }
    }

    public async Task<Film> AddAsync(Film film, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new film: {FilmTitle}", film.Title);
            film.CreatedDate = DateTime.UtcNow;
            film.IsActive = true;

            await _context.Films.AddAsync(film, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Film added successfully with ID: {FilmId}", film.Id);
            return film;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding film: {FilmTitle}", film.Title);
            throw;
        }
    }

    public async Task UpdateAsync(Film film, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating film with ID: {FilmId}", film.Id);
            film.ModifiedDate = DateTime.UtcNow;

            _context.Films.Update(film);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Film updated successfully: {FilmId}", film.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film with ID: {FilmId}", film.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting film with ID: {FilmId}", id);
            var film = await _context.Films.FindAsync(new object[] { id }, cancellationToken);

            if (film != null)
            {
                film.IsActive = false;
                film.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Film soft-deleted successfully: {FilmId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film with ID: {FilmId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Films.AnyAsync(f => f.Id == id && f.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if film exists with ID: {FilmId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Film>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching films with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync(cancellationToken);

            return await _context.Films
                .AsNoTracking()
                .Where(f => f.IsActive &&
                    (f.Title.Contains(searchTerm) ||
                     (f.Description != null && f.Description.Contains(searchTerm)) ||
                     (f.Genre != null && f.Genre.Contains(searchTerm))))
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching films with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
