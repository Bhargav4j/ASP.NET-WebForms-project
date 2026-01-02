namespace Films.Application.DTOs;

/// <summary>
/// Data transfer object for Actor entity
/// </summary>
public class ActorDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? SexId { get; set; }
    public string? SexName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for creating a new Actor
/// </summary>
public class ActorCreateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? SexId { get; set; }
}

/// <summary>
/// DTO for updating an existing Actor
/// </summary>
public class ActorUpdateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? SexId { get; set; }
    public bool IsActive { get; set; }
}
