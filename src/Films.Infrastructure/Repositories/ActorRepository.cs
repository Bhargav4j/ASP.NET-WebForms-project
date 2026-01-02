using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Films.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Actor entity using EF Core
/// </summary>
public class ActorRepository : IActorRepository
{
    private readonly FilmsDbContext _context;
    private readonly ILogger<ActorRepository> _logger;

    public ActorRepository(FilmsDbContext context, ILogger<ActorRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Actor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Actors
                .AsNoTracking()
                .Include(a => a.Sex)
                .Where(a => a.IsActive)
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all actors from database");
            throw;
        }
    }

    public async Task<Actor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Actors
                .AsNoTracking()
                .Include(a => a.Sex)
                .Include(a => a.RefAFs)
                    .ThenInclude(r => r.Film)
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
            _logger.LogError(ex, "Error adding actor to database");
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
            var actor = await _context.Actors.FindAsync(new object[] { id }, cancellationToken);
            if (actor != null)
            {
                actor.IsActive = false;
                actor.ModifiedDate = DateTime.UtcNow;
                actor.ModifiedBy = "System";
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
            return await _context.Actors.AnyAsync(a => a.Id == id && a.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of actor with ID: {ActorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Actor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Actors
                .AsNoTracking()
                .Include(a => a.Sex)
                .Where(a => a.IsActive &&
                    (a.FirstName.Contains(searchTerm) || a.LastName.Contains(searchTerm)))
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching actors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
