namespace Films.Domain.DTOs;

/// <summary>
/// Data transfer object for Actor entity
/// </summary>
public class ActorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? SexId { get; set; }
    public string? SexName { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for creating a new actor
/// </summary>
public class ActorCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? SexId { get; set; }
    public DateTime? BirthDate { get; set; }
}

/// <summary>
/// DTO for updating an existing actor
/// </summary>
public class ActorUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? SexId { get; set; }
    public DateTime? BirthDate { get; set; }
}
