namespace Films.Application.DTOs;

/// <summary>
/// Data transfer object for User entity
/// </summary>
public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? SexId { get; set; }
    public string? SexName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? PhoneNumber { get; set; }
    public int? TypeUserId { get; set; }
    public string? TypeUserName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for creating a new user (registration)
/// </summary>
public class UserCreateDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? SecretQuestion { get; set; }
    public string? SecretAnswer { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? SexId { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? PhoneNumber { get; set; }
}

/// <summary>
/// DTO for updating an existing user
/// </summary>
public class UserUpdateDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? SexId { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? PhoneNumber { get; set; }
}
