using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Films.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for DirectedBy entity
/// </summary>
public class DirectedByRepository : IDirectedByRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DirectedByRepository> _logger;

    public DirectedByRepository(ApplicationDbContext context, ILogger<DirectedByRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<DirectedBy>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.DirectedBys
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

    public async Task<DirectedBy?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.DirectedBys
                .Include(d => d.RefDAFs)
                .FirstOrDefaultAsync(d => d.Id == id && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving director with ID: {DirectorId}", id);
            throw;
        }
    }

    public async Task<DirectedBy> AddAsync(DirectedBy directedBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.DirectedBys.Add(directedBy);
            await _context.SaveChangesAsync(cancellationToken);
            return directedBy;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding director");
            throw;
        }
    }

    public async Task UpdateAsync(DirectedBy directedBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.DirectedBys.Update(directedBy);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating director with ID: {DirectorId}", directedBy.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var directedBy = await GetByIdAsync(id, cancellationToken);
            if (directedBy != null)
            {
                directedBy.IsActive = false;
                directedBy.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting director with ID: {DirectorId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.DirectedBys
                .AnyAsync(d => d.Id == id && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if director exists with ID: {DirectorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<DirectedBy>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.DirectedBys
                .Where(d => d.IsActive &&
                    (d.Name.Contains(searchTerm) ||
                     (d.Description != null && d.Description.Contains(searchTerm))))
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
