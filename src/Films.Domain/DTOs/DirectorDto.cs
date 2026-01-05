namespace Films.Domain.DTOs;

/// <summary>
/// Data transfer object for Director entity
/// </summary>
public class DirectorDto
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
/// DTO for creating a new director
/// </summary>
public class DirectorCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? SexId { get; set; }
    public DateTime? BirthDate { get; set; }
}

/// <summary>
/// DTO for updating an existing director
/// </summary>
public class DirectorUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? SexId { get; set; }
    public DateTime? BirthDate { get; set; }
}
