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
    private readonly FilmsDbContext _context;
    private readonly ILogger<ActorRepository> _logger;

    public ActorRepository(FilmsDbContext context, ILogger<ActorRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Actor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all actors");
            return await _context.Actors
                .Include(a => a.Sex)
                .Where(a => a.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all actors");
            throw;
        }
    }

    public async Task<Actor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting actor by id: {Id}", id);
            return await _context.Actors
                .Include(a => a.Sex)
                .Where(a => a.Id == id && a.IsActive)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting actor by id: {Id}", id);
            throw;
        }
    }

    public async Task<Actor> AddAsync(Actor actor, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new actor: {Name}", actor.Name);
            actor.CreatedDate = DateTime.UtcNow;
            actor.IsActive = true;

            await _context.Actors.AddAsync(actor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return actor;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding actor: {Name}", actor.Name);
            throw;
        }
    }

    public async Task UpdateAsync(Actor actor, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating actor: {Id}", actor.Id);
            actor.ModifiedDate = DateTime.UtcNow;

            _context.Actors.Update(actor);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating actor: {Id}", actor.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting actor: {Id}", id);
            var actor = await _context.Actors.FindAsync(new object[] { id }, cancellationToken);
            if (actor != null)
            {
                actor.IsActive = false;
                actor.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting actor: {Id}", id);
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
            _logger.LogError(ex, "Error checking actor existence: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Actor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching actors with term: {SearchTerm}", searchTerm);
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
