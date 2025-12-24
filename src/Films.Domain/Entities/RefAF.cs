namespace Films.Domain.Entities;

/// <summary>
/// Represents the relationship between Actors and Films
/// </summary>
public class RefAF
{
    public int Id { get; set; }
    public int ActorId { get; set; }
    public int FilmId { get; set; }
    public string? Role { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Actor Actor { get; set; } = null!;
    public virtual Film Film { get; set; } = null!;
}
