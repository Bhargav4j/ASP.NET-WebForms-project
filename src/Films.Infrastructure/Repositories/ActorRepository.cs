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
            _logger.LogInformation("Retrieving all active actors");
            return await _context.Actors
                .AsNoTracking()
                .Where(a => a.IsActive)
                .Include(a => a.Sex)
                .Include(a => a.ActorFilms)
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
            _logger.LogInformation("Retrieving actor with ID: {ActorId}", id);
            return await _context.Actors
                .AsNoTracking()
                .Include(a => a.Sex)
                .Include(a => a.ActorFilms)
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
            _logger.LogInformation("Adding new actor: {ActorName}", actor.Name);
            actor.CreatedDate = DateTime.UtcNow;
            actor.IsActive = true;

            await _context.Actors.AddAsync(actor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Actor added successfully with ID: {ActorId}", actor.Id);
            return actor;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding actor: {ActorName}", actor.Name);
            throw;
        }
    }

    public async Task UpdateAsync(Actor actor, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating actor with ID: {ActorId}", actor.Id);
            actor.ModifiedDate = DateTime.UtcNow;

            _context.Actors.Update(actor);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Actor updated successfully: {ActorId}", actor.Id);
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
            _logger.LogInformation("Deleting actor with ID: {ActorId}", id);
            var actor = await _context.Actors.FindAsync(new object[] { id }, cancellationToken);

            if (actor != null)
            {
                actor.IsActive = false;
                actor.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Actor soft-deleted successfully: {ActorId}", id);
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
            return await _context.Actors.AnyAsync(a => a.Id == id && a.IsActive, cancellationToken);
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
            _logger.LogInformation("Searching actors with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync(cancellationToken);

            return await _context.Actors
                .AsNoTracking()
                .Where(a => a.IsActive &&
                    (a.Name.Contains(searchTerm) ||
                     (a.Description != null && a.Description.Contains(searchTerm))))
                .Include(a => a.Sex)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching actors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
