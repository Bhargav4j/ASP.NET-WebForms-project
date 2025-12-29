using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Films.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Actor entity
/// </summary>
public class ActorRepository : IActorRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ActorRepository> _logger;

    public ActorRepository(ApplicationDbContext context, ILogger<ActorRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Actor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Actors
                .Include(a => a.Sex)
                .Where(a => a.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all actors");
            throw;
        }
    }

    public async Task<Actor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Actors
                .Include(a => a.Sex)
                .Include(a => a.RefAFs)
                .FirstOrDefaultAsync(a => a.Id == id && a.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving actor with ID: {ActorId}", id);
            throw;
        }
    }

    public async Task<Actor> AddAsync(Actor actor, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync(cancellationToken);
            return actor;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding actor");
            throw;
        }
    }

    public async Task UpdateAsync(Actor actor, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Actors.Update(actor);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating actor with ID: {ActorId}", actor.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var actor = await GetByIdAsync(id, cancellationToken);
            if (actor != null)
            {
                actor.IsActive = false;
                actor.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting actor with ID: {ActorId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Actors
                .AnyAsync(a => a.Id == id && a.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if actor exists with ID: {ActorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Actor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Actors
                .Include(a => a.Sex)
                .Where(a => a.IsActive &&
                    (a.Name.Contains(searchTerm) ||
                     (a.Description != null && a.Description.Contains(searchTerm))))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching actors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
