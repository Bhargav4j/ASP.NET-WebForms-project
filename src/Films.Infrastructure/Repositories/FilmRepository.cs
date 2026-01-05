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
            _logger.LogInformation("Getting all films");
            return await _context.Films
                .Where(f => f.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
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
            _logger.LogInformation("Getting film by id: {Id}", id);
            return await _context.Films
                .Where(f => f.Id == id && f.IsActive)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting film by id: {Id}", id);
            throw;
        }
    }

    public async Task<Film> AddAsync(Film film, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new film: {Name}", film.Name);
            film.CreatedDate = DateTime.UtcNow;
            film.IsActive = true;

            await _context.Films.AddAsync(film, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return film;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding film: {Name}", film.Name);
            throw;
        }
    }

    public async Task UpdateAsync(Film film, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating film: {Id}", film.Id);
            film.ModifiedDate = DateTime.UtcNow;

            _context.Films.Update(film);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film: {Id}", film.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting film: {Id}", id);
            var film = await _context.Films.FindAsync(new object[] { id }, cancellationToken);
            if (film != null)
            {
                film.IsActive = false;
                film.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film: {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Films
                .AnyAsync(f => f.Id == id && f.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking film existence: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Film>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching films with term: {SearchTerm}", searchTerm);
            return await _context.Films
                .Where(f => f.IsActive &&
                    (f.Name.Contains(searchTerm) ||
                     (f.Description != null && f.Description.Contains(searchTerm))))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching films with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
