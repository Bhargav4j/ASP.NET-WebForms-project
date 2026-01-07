namespace Films.Application.DTOs;

public class DirectorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public int? SexId { get; set; }
    public string? SexName { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}

public class DirectorCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public int? SexId { get; set; }
}

public class DirectorUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public int? SexId { get; set; }
    public bool IsActive { get; set; }
}
