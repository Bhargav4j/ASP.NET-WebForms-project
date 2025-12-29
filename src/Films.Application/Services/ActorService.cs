using AutoMapper;
using Films.Domain.DTOs;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for Actor operations
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
            _logger.LogInformation("Getting all actors");
            var actors = await _actorRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ActorDto>>(actors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all actors");
            throw;
        }
    }

    public async Task<ActorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting actor with ID: {ActorId}", id);
            var actor = await _actorRepository.GetByIdAsync(id, cancellationToken);
            return actor != null ? _mapper.Map<ActorDto>(actor) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting actor with ID: {ActorId}", id);
            throw;
        }
    }

    public async Task<ActorDto> CreateAsync(ActorCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new actor: {ActorName}", createDto.Name);
            var actor = _mapper.Map<Actor>(createDto);
            var createdActor = await _actorRepository.AddAsync(actor, cancellationToken);
            _logger.LogInformation("Actor created successfully with ID: {ActorId}", createdActor.Id);
            return _mapper.Map<ActorDto>(createdActor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating actor: {ActorName}", createDto.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, ActorUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating actor with ID: {ActorId}", id);
            var existingActor = await _actorRepository.GetByIdAsync(id, cancellationToken);
            if (existingActor == null)
            {
                throw new KeyNotFoundException($"Actor with ID {id} not found");
            }

            _mapper.Map(updateDto, existingActor);
            await _actorRepository.UpdateAsync(existingActor, cancellationToken);
            _logger.LogInformation("Actor updated successfully with ID: {ActorId}", id);
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
            {
                throw new KeyNotFoundException($"Actor with ID {id} not found");
            }

            await _actorRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Actor deleted successfully with ID: {ActorId}", id);
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
