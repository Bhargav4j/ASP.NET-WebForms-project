using AutoMapper;
using Films.Application.DTOs;
using Films.Application.Interfaces;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

public class FilmService : IFilmService
{
    private readonly IFilmRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<FilmService> _logger;

    public FilmService(IFilmRepository repository, IMapper mapper, ILogger<FilmService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<FilmDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var films = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<FilmDto>>(films);
    }

    public async Task<FilmDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var film = await _repository.GetByIdAsync(id, cancellationToken);
        return film == null ? null : _mapper.Map<FilmDto>(film);
    }

    public async Task<FilmDto> CreateAsync(FilmCreateDto dto, CancellationToken cancellationToken = default)
    {
        var film = _mapper.Map<Film>(dto);
        film.CreatedDate = DateTime.UtcNow;
        film.IsActive = true;
        film.CreatedBy = "system";
        var created = await _repository.AddAsync(film, cancellationToken);
        return _mapper.Map<FilmDto>(created);
    }

    public async Task UpdateAsync(int id, FilmUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var film = await _repository.GetByIdAsync(id, cancellationToken);
        if (film == null) throw new KeyNotFoundException($"Film with ID {id} not found");
        film.Name = dto.Name;
        film.Description = dto.Description;
        film.IsActive = dto.IsActive;
        film.ModifiedDate = DateTime.UtcNow;
        film.ModifiedBy = "system";
        await _repository.UpdateAsync(film, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<FilmDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var films = await _repository.SearchAsync(searchTerm, cancellationToken);
        return _mapper.Map<IEnumerable<FilmDto>>(films);
    }
}
