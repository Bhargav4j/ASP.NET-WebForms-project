namespace Films.Application.DTOs;

public class ActorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Surname { get; set; }
    public int? IdSex { get; set; }
    public string? SexName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
}

public class ActorCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Surname { get; set; }
    public int? IdSex { get; set; }
}

public class ActorUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Surname { get; set; }
    public int? IdSex { get; set; }
    public bool IsActive { get; set; }
}
