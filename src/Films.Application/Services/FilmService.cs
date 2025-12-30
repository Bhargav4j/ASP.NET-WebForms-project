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
    private readonly IFilmRepository _filmRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<FilmService> _logger;

    public FilmService(
        IFilmRepository filmRepository,
        IMapper mapper,
        ILogger<FilmService> logger)
    {
        _filmRepository = filmRepository ?? throw new ArgumentNullException(nameof(filmRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

    public async Task<FilmDto> CreateAsync(FilmCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new film: {FilmTitle}", dto.Title);

            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Film title is required", nameof(dto.Title));

            var film = _mapper.Map<Film>(dto);
            var createdFilm = await _filmRepository.AddAsync(film, cancellationToken);

            _logger.LogInformation("Film created successfully with ID: {FilmId}", createdFilm.Id);
            return _mapper.Map<FilmDto>(createdFilm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating film: {FilmTitle}", dto.Title);
            throw;
        }
    }

    public async Task UpdateAsync(int id, FilmUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating film with ID: {FilmId}", id);

            var existingFilm = await _filmRepository.GetByIdAsync(id, cancellationToken);
            if (existingFilm == null)
                throw new KeyNotFoundException($"Film with ID {id} not found");

            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Film title is required", nameof(dto.Title));

            _mapper.Map(dto, existingFilm);
            existingFilm.ModifiedDate = DateTime.UtcNow;
            existingFilm.ModifiedBy = "System";

            await _filmRepository.UpdateAsync(existingFilm, cancellationToken);

            _logger.LogInformation("Film updated successfully: {FilmId}", id);
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

            var exists = await _filmRepository.ExistsAsync(id, cancellationToken);
            if (!exists)
                throw new KeyNotFoundException($"Film with ID {id} not found");

            await _filmRepository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Film deleted successfully: {FilmId}", id);
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
