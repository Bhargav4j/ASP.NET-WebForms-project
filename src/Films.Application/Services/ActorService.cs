using AutoMapper;
using Films.Application.DTOs;
using Films.Application.Interfaces;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

public class ActorService : IActorService
{
    private readonly IActorRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ActorService> _logger;

    public ActorService(IActorRepository repository, IMapper mapper, ILogger<ActorService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ActorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var actors = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ActorDto>>(actors);
    }

    public async Task<ActorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var actor = await _repository.GetByIdAsync(id, cancellationToken);
        return actor == null ? null : _mapper.Map<ActorDto>(actor);
    }

    public async Task<ActorDto> CreateAsync(ActorCreateDto dto, CancellationToken cancellationToken = default)
    {
        var actor = _mapper.Map<Actor>(dto);
        actor.CreatedDate = DateTime.UtcNow;
        actor.IsActive = true;
        actor.CreatedBy = "system";
        var created = await _repository.AddAsync(actor, cancellationToken);
        return _mapper.Map<ActorDto>(created);
    }

    public async Task UpdateAsync(int id, ActorUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var actor = await _repository.GetByIdAsync(id, cancellationToken);
        if (actor == null) throw new KeyNotFoundException($"Actor with ID {id} not found");
        actor.Name = dto.Name;
        actor.Surname = dto.Surname;
        actor.IdSex = dto.IdSex;
        actor.IsActive = dto.IsActive;
        actor.ModifiedDate = DateTime.UtcNow;
        actor.ModifiedBy = "system";
        await _repository.UpdateAsync(actor, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<ActorDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var actors = await _repository.SearchAsync(searchTerm, cancellationToken);
        return _mapper.Map<IEnumerable<ActorDto>>(actors);
    }
}
