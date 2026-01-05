using AutoMapper;
using Films.Domain.DTOs;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for Film business operations
/// </summary>
public class FilmService : IFilmService
{
    private readonly IFilmRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<FilmService> _logger;

    public FilmService(IFilmRepository repository, IMapper mapper, ILogger<FilmService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<FilmDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all films");
            var films = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in FilmService.GetAllAsync");
            throw;
        }
    }

    public async Task<FilmDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting film by id: {Id}", id);
            var film = await _repository.GetByIdAsync(id, cancellationToken);
            return film == null ? null : _mapper.Map<FilmDto>(film);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in FilmService.GetByIdAsync for id: {Id}", id);
            throw;
        }
    }

    public async Task<FilmDto> CreateAsync(FilmCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new film: {Name}", createDto.Name);
            var film = _mapper.Map<Film>(createDto);
            film.CreatedBy = "System";
            film.CreatedDate = DateTime.UtcNow;
            film.IsActive = true;

            var createdFilm = await _repository.AddAsync(film, cancellationToken);
            return _mapper.Map<FilmDto>(createdFilm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in FilmService.CreateAsync");
            throw;
        }
    }

    public async Task UpdateAsync(int id, FilmUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating film: {Id}", id);
            var film = await _repository.GetByIdAsync(id, cancellationToken);
            if (film == null)
            {
                throw new InvalidOperationException($"Film with id {id} not found");
            }

            _mapper.Map(updateDto, film);
            film.ModifiedBy = "System";
            film.ModifiedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(film, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in FilmService.UpdateAsync for id: {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting film: {Id}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in FilmService.DeleteAsync for id: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<FilmDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching films with term: {SearchTerm}", searchTerm);
            var films = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in FilmService.SearchAsync");
            throw;
        }
    }
}
