using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            return await _context.Directors
                .AsNoTracking()
                .Include(d => d.Sex)
                .Where(d => d.IsActive)
                .OrderBy(d => d.LastName)
                .ThenBy(d => d.FirstName)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all directors from database");
            throw;
        }
    }

    public async Task<Director?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Directors
                .AsNoTracking()
                .Include(d => d.Sex)
                .FirstOrDefaultAsync(d => d.Id == id && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving director with ID {DirectorId} from database", id);
            throw;
        }
    }

    public async Task<Director> AddAsync(Director director, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Directors.AddAsync(director, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return director;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding director to database");
            throw;
        }
    }

    public async Task UpdateAsync(Director director, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Directors.Update(director);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating director with ID {DirectorId} in database", director.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var director = await _context.Directors.FindAsync(new object[] { id }, cancellationToken);
            if (director != null)
            {
                director.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting director with ID {DirectorId} from database", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Directors
                .AsNoTracking()
                .AnyAsync(d => d.Id == id && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of director with ID {DirectorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Director>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Directors
                .AsNoTracking()
                .Include(d => d.Sex)
                .Where(d => d.IsActive &&
                    (d.FirstName.Contains(searchTerm) || d.LastName.Contains(searchTerm)))
                .OrderBy(d => d.LastName)
                .ThenBy(d => d.FirstName)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching directors with term {SearchTerm}", searchTerm);
            throw;
        }
    }
}
