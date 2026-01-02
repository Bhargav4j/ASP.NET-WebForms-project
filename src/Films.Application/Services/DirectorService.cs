using AutoMapper;
using Films.Application.DTOs;
using Films.Application.Interfaces;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for Director business logic
/// </summary>
public class DirectorService : IDirectorService
{
    private readonly IDirectorRepository _directorRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DirectorService> _logger;

    public DirectorService(
        IDirectorRepository directorRepository,
        IMapper mapper,
        ILogger<DirectorService> logger)
    {
        _directorRepository = directorRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<DirectorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all directors");
            var directors = await _directorRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<DirectorDto>>(directors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all directors");
            throw;
        }
    }

    public async Task<DirectorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving director with ID: {DirectorId}", id);
            var director = await _directorRepository.GetByIdAsync(id, cancellationToken);
            return director != null ? _mapper.Map<DirectorDto>(director) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving director with ID: {DirectorId}", id);
            throw;
        }
    }

    public async Task<DirectorDto> CreateAsync(DirectorCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new director: {FirstName} {LastName}", createDto.FirstName, createDto.LastName);
            var director = _mapper.Map<Director>(createDto);
            var createdDirector = await _directorRepository.AddAsync(director, cancellationToken);
            return _mapper.Map<DirectorDto>(createdDirector);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating director: {FirstName} {LastName}", createDto.FirstName, createDto.LastName);
            throw;
        }
    }

    public async Task UpdateAsync(int id, DirectorUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating director with ID: {DirectorId}", id);

            var existingDirector = await _directorRepository.GetByIdAsync(id, cancellationToken);
            if (existingDirector == null)
            {
                throw new InvalidOperationException($"Director with ID {id} not found");
            }

            existingDirector.FirstName = updateDto.FirstName;
            existingDirector.LastName = updateDto.LastName;
            existingDirector.SexId = updateDto.SexId;
            existingDirector.IsActive = updateDto.IsActive;
            existingDirector.ModifiedDate = DateTime.UtcNow;
            existingDirector.ModifiedBy = "System";

            await _directorRepository.UpdateAsync(existingDirector, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating director with ID: {DirectorId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting director with ID: {DirectorId}", id);
            await _directorRepository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting director with ID: {DirectorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<DirectorDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching directors with term: {SearchTerm}", searchTerm);
            var directors = await _directorRepository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<DirectorDto>>(directors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching directors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
