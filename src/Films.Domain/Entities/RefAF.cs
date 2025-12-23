namespace Films.Domain.Entities;

/// <summary>
/// Represents the relationship between actors and films
/// </summary>
public class RefAF
{
    public int Id { get; set; }
    public int IdActor { get; set; }
    public int IdFilm { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Actor Actor { get; set; } = null!;
    public virtual Film Film { get; set; } = null!;
}
