using AutoMapper;
using Films.Domain.DTOs;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for User operations
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all users");
            var users = await _userRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all users");
            throw;
        }
    }

    public async Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting user with ID: {UserId}", id);
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user with ID: {UserId}", id);
            throw;
        }
    }

    public async Task<UserDto> CreateAsync(UserCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new user: {Username}", createDto.Username);
            var user = _mapper.Map<User>(createDto);

            // Hash password (simple implementation, should use proper password hashing)
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createDto.Password);

            var createdUser = await _userRepository.AddAsync(user, cancellationToken);
            _logger.LogInformation("User created successfully with ID: {UserId}", createdUser.Id);
            return _mapper.Map<UserDto>(createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user: {Username}", createDto.Username);
            throw;
        }
    }

    public async Task UpdateAsync(int id, UserUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user with ID: {UserId}", id);
            var existingUser = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found");
            }

            _mapper.Map(updateDto, existingUser);
            await _userRepository.UpdateAsync(existingUser, cancellationToken);
            _logger.LogInformation("User updated successfully with ID: {UserId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID: {UserId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user with ID: {UserId}", id);
            var exists = await _userRepository.ExistsAsync(id, cancellationToken);
            if (!exists)
            {
                throw new KeyNotFoundException($"User with ID {id} not found");
            }

            await _userRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("User deleted successfully with ID: {UserId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with ID: {UserId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<UserDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching users with term: {SearchTerm}", searchTerm);
            var users = await _userRepository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
