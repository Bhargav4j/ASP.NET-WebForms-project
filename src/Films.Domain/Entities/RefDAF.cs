namespace Films.Domain.Entities;

/// <summary>
/// Represents the relationship between directors, actors, and films
/// </summary>
public class RefDAF
{
    public int Id { get; set; }

    public int DirectedById { get; set; }

    public int FilmId { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; } = true;

    public string CreatedBy { get; set; } = "System";

    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual DirectedBy DirectedBy { get; set; } = null!;

    public virtual Film Film { get; set; } = null!;
}
