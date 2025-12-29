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
    private readonly ApplicationDbContext _context;
    private readonly ILogger<FilmRepository> _logger;

    public FilmRepository(ApplicationDbContext context, ILogger<FilmRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Film>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Films
                .Where(f => f.IsActive)
                .AsNoTracking()
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
            return await _context.Films
                .Include(f => f.RefAFs)
                .Include(f => f.RefDAFs)
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
            _context.Films.Add(film);
            await _context.SaveChangesAsync(cancellationToken);
            return film;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding film");
            throw;
        }
    }

    public async Task UpdateAsync(Film film, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Films.Update(film);
            await _context.SaveChangesAsync(cancellationToken);
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
            var film = await GetByIdAsync(id, cancellationToken);
            if (film != null)
            {
                film.IsActive = false;
                film.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
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
            return await _context.Films
                .AnyAsync(f => f.Id == id && f.IsActive, cancellationToken);
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
            return await _context.Films
                .Where(f => f.IsActive &&
                    (f.Name.Contains(searchTerm) ||
                     (f.Description != null && f.Description.Contains(searchTerm)) ||
                     (f.Genre != null && f.Genre.Contains(searchTerm))))
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
