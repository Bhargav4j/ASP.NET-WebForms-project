using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for Actor business operations
/// </summary>
public class ActorService : IActorService
{
    private readonly IActorRepository _repository;
    private readonly ILogger<ActorService> _logger;

    public ActorService(IActorRepository repository, ILogger<ActorService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Actor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all actors");
            return await _repository.GetAllAsync(cancellationToken);
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
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving actor with ID: {ActorId}", id);
            throw;
        }
    }

    public async Task<Actor> CreateAsync(Actor actor, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new actor: {ActorName}", $"{actor.FirstName} {actor.LastName}");
            actor.CreatedDate = DateTime.UtcNow;
            actor.IsActive = true;
            return await _repository.AddAsync(actor, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating actor: {ActorName}", $"{actor.FirstName} {actor.LastName}");
            throw;
        }
    }

    public async Task UpdateAsync(int id, Actor actor, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating actor with ID: {ActorId}", id);
            var existingActor = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingActor == null)
            {
                throw new InvalidOperationException($"Actor with ID {id} not found");
            }

            actor.Id = id;
            actor.ModifiedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(actor, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating actor with ID: {ActorId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting actor with ID: {ActorId}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting actor with ID: {ActorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Actor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching actors with term: {SearchTerm}", searchTerm);
            return await _repository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching actors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
