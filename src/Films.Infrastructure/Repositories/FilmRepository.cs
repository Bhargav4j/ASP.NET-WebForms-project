using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Films.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Film entity using EF Core
/// </summary>
public class FilmRepository : IFilmRepository
{
    private readonly FilmsDbContext _context;
    private readonly ILogger<FilmRepository> _logger;

    public FilmRepository(FilmsDbContext context, ILogger<FilmRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Film>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Films
                .AsNoTracking()
                .Where(f => f.IsActive)
                .OrderBy(f => f.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all films from database");
            throw;
        }
    }

    public async Task<Film?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Films
                .AsNoTracking()
                .Include(f => f.RefAFs)
                    .ThenInclude(r => r.Actor)
                .Include(f => f.RefDAFs)
                    .ThenInclude(r => r.Director)
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
            _logger.LogError(ex, "Error adding film to database");
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
            var film = await _context.Films.FindAsync(new object[] { id }, cancellationToken);
            if (film != null)
            {
                film.IsActive = false;
                film.ModifiedDate = DateTime.UtcNow;
                film.ModifiedBy = "System";
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
            return await _context.Films.AnyAsync(f => f.Id == id && f.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of film with ID: {FilmId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Film>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Films
                .AsNoTracking()
                .Where(f => f.IsActive &&
                    (f.Name.Contains(searchTerm) ||
                     (f.Description != null && f.Description.Contains(searchTerm))))
                .OrderBy(f => f.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching films with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
