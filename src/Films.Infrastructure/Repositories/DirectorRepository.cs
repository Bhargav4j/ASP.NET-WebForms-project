using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;

namespace Films.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Director entity
/// </summary>
public class DirectorRepository : IDirectorRepository
{
    private readonly FilmsDbContext _context;
    private readonly ILogger<DirectorRepository> _logger;

    public DirectorRepository(FilmsDbContext context, ILogger<DirectorRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Director>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all directors");
            return await _context.Directors
                .Include(d => d.Sex)
                .Where(d => d.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
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
            _logger.LogInformation("Retrieving director with id {DirectorId}", id);
            return await _context.Directors
                .Include(d => d.Sex)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving director with id {DirectorId}", id);
            throw;
        }
    }

    public async Task<Director> AddAsync(Director director, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new director: {DirectorName}", director.Name);
            director.CreatedDate = DateTime.UtcNow;
            director.IsActive = true;

            await _context.Directors.AddAsync(director, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Director added successfully with id {DirectorId}", director.Id);
            return director;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding director: {DirectorName}", director.Name);
            throw;
        }
    }

    public async Task UpdateAsync(Director director, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating director with id {DirectorId}", director.Id);
            director.ModifiedDate = DateTime.UtcNow;

            _context.Directors.Update(director);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Director updated successfully with id {DirectorId}", director.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating director with id {DirectorId}", director.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting director with id {DirectorId}", id);
            var director = await _context.Directors.FindAsync(new object[] { id }, cancellationToken);

            if (director != null)
            {
                director.IsActive = false;
                director.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Director soft-deleted successfully with id {DirectorId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting director with id {DirectorId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Directors.AnyAsync(d => d.Id == id && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if director exists with id {DirectorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Director>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching directors with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync(cancellationToken);
            }

            return await _context.Directors
                .Include(d => d.Sex)
                .Where(d => d.IsActive &&
                    (d.Name.Contains(searchTerm) ||
                     (d.Surname != null && d.Surname.Contains(searchTerm)) ||
                     (d.Country != null && d.Country.Contains(searchTerm))))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching directors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
