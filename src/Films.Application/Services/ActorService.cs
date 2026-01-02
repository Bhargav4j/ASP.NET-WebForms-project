using AutoMapper;
using Films.Application.DTOs;
using Films.Application.Interfaces;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for Actor business logic
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
        _actorRepository = actorRepository;
        _mapper = mapper;
        _logger = logger;
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

    public async Task<ActorDto> CreateAsync(ActorCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new actor: {FirstName} {LastName}", createDto.FirstName, createDto.LastName);
            var actor = _mapper.Map<Actor>(createDto);
            var createdActor = await _actorRepository.AddAsync(actor, cancellationToken);
            return _mapper.Map<ActorDto>(createdActor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating actor: {FirstName} {LastName}", createDto.FirstName, createDto.LastName);
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
                throw new InvalidOperationException($"Actor with ID {id} not found");
            }

            existingActor.FirstName = updateDto.FirstName;
            existingActor.LastName = updateDto.LastName;
            existingActor.SexId = updateDto.SexId;
            existingActor.IsActive = updateDto.IsActive;
            existingActor.ModifiedDate = DateTime.UtcNow;
            existingActor.ModifiedBy = "System";

            await _actorRepository.UpdateAsync(existingActor, cancellationToken);
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
            await _actorRepository.DeleteAsync(id, cancellationToken);
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
