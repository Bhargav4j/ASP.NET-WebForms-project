using AutoMapper;
using Films.Application.DTOs;
using Films.Application.Interfaces;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for Film business logic
/// </summary>
public class FilmService : IFilmService
{
    private readonly IFilmRepository _filmRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<FilmService> _logger;

    public FilmService(
        IFilmRepository filmRepository,
        IMapper mapper,
        ILogger<FilmService> logger)
    {
        _filmRepository = filmRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<FilmDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all films");
            var films = await _filmRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all films");
            throw;
        }
    }

    public async Task<FilmDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving film with ID: {FilmId}", id);
            var film = await _filmRepository.GetByIdAsync(id, cancellationToken);
            return film != null ? _mapper.Map<FilmDto>(film) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving film with ID: {FilmId}", id);
            throw;
        }
    }

    public async Task<FilmDto> CreateAsync(FilmCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new film: {FilmName}", createDto.Name);
            var film = _mapper.Map<Film>(createDto);
            var createdFilm = await _filmRepository.AddAsync(film, cancellationToken);
            return _mapper.Map<FilmDto>(createdFilm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating film: {FilmName}", createDto.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, FilmUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating film with ID: {FilmId}", id);

            var existingFilm = await _filmRepository.GetByIdAsync(id, cancellationToken);
            if (existingFilm == null)
            {
                throw new InvalidOperationException($"Film with ID {id} not found");
            }

            existingFilm.Name = updateDto.Name;
            existingFilm.Description = updateDto.Description;
            existingFilm.IsActive = updateDto.IsActive;
            existingFilm.ModifiedDate = DateTime.UtcNow;
            existingFilm.ModifiedBy = "System";

            await _filmRepository.UpdateAsync(existingFilm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film with ID: {FilmId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting film with ID: {FilmId}", id);
            await _filmRepository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film with ID: {FilmId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<FilmDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching films with term: {SearchTerm}", searchTerm);
            var films = await _filmRepository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching films with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
