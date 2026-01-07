namespace Films.Application.DTOs;

public class FilmDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
}

public class FilmCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class FilmUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
