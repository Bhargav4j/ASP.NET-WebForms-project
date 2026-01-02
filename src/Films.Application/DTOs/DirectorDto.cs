namespace Films.Application.DTOs;

/// <summary>
/// Data transfer object for Director entity
/// </summary>
public class DirectorDto
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
/// DTO for creating a new Director
/// </summary>
public class DirectorCreateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? SexId { get; set; }
}

/// <summary>
/// DTO for updating an existing Director
/// </summary>
public class DirectorUpdateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? SexId { get; set; }
    public bool IsActive { get; set; }
}
