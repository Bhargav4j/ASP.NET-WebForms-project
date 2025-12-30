using AutoMapper;
using Films.Domain.DTOs;
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
    private readonly IActorRepository _actorRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ActorService> _logger;

    public ActorService(
        IActorRepository actorRepository,
        IMapper mapper,
        ILogger<ActorService> logger)
    {
        _actorRepository = actorRepository ?? throw new ArgumentNullException(nameof(actorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<ActorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all actors");
            var actors = await _actorRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ActorDto>>(actors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all actors");
            throw;
        }
    }

    public async Task<ActorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving actor with ID: {ActorId}", id);
            var actor = await _actorRepository.GetByIdAsync(id, cancellationToken);
            return actor != null ? _mapper.Map<ActorDto>(actor) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving actor with ID: {ActorId}", id);
            throw;
        }
    }

    public async Task<ActorDto> CreateAsync(ActorCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new actor: {ActorName}", dto.Name);

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Actor name is required", nameof(dto.Name));

            var actor = _mapper.Map<Actor>(dto);
            var createdActor = await _actorRepository.AddAsync(actor, cancellationToken);

            _logger.LogInformation("Actor created successfully with ID: {ActorId}", createdActor.Id);
            return _mapper.Map<ActorDto>(createdActor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating actor: {ActorName}", dto.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, ActorUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating actor with ID: {ActorId}", id);

            var existingActor = await _actorRepository.GetByIdAsync(id, cancellationToken);
            if (existingActor == null)
                throw new KeyNotFoundException($"Actor with ID {id} not found");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Actor name is required", nameof(dto.Name));

            _mapper.Map(dto, existingActor);
            existingActor.ModifiedDate = DateTime.UtcNow;
            existingActor.ModifiedBy = "System";

            await _actorRepository.UpdateAsync(existingActor, cancellationToken);

            _logger.LogInformation("Actor updated successfully: {ActorId}", id);
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

            var exists = await _actorRepository.ExistsAsync(id, cancellationToken);
            if (!exists)
                throw new KeyNotFoundException($"Actor with ID {id} not found");

            await _actorRepository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Actor deleted successfully: {ActorId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting actor with ID: {ActorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<ActorDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching actors with term: {SearchTerm}", searchTerm);
            var actors = await _actorRepository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<ActorDto>>(actors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching actors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
