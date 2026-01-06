using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Films.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for TypeUser entity
/// </summary>
public class TypeUserRepository : ITypeUserRepository
{
    private readonly FilmsDbContext _context;
    private readonly ILogger<TypeUserRepository> _logger;

    public TypeUserRepository(FilmsDbContext context, ILogger<TypeUserRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TypeUser>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TypeUsers
                .AsNoTracking()
                .Where(t => t.IsActive)
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all type users from database");
            throw;
        }
    }

    public async Task<TypeUser?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TypeUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting type user with ID {TypeUserId} from database", id);
            throw;
        }
    }

    public async Task<TypeUser> AddAsync(TypeUser entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.TypeUsers.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding type user to database");
            throw;
        }
    }

    public async Task UpdateAsync(TypeUser entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.TypeUsers.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating type user with ID {TypeUserId} in database", entity.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _context.TypeUsers.FindAsync(new object[] { id }, cancellationToken);
            if (entity != null)
            {
                entity.IsActive = false;
                entity.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting type user with ID {TypeUserId} from database", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TypeUsers.AnyAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if type user with ID {TypeUserId} exists in database", id);
            throw;
        }
    }
}
