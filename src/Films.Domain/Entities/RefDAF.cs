namespace Films.Domain.Entities;

/// <summary>
/// Represents the relationship between Directors, Actors, and Films
/// </summary>
public class RefDAF
{
    public int Id { get; set; }
    public int DirectorId { get; set; }
    public int FilmId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Director Director { get; set; } = null!;
    public virtual Film Film { get; set; } = null!;
}
