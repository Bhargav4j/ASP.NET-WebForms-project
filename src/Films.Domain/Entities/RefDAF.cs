namespace Films.Domain.Entities;

/// <summary>
/// Represents the many-to-many relationship between directors and films
/// </summary>
public class RefDAF
{
    public int Id { get; set; }
    public int DirectedById { get; set; }
    public int FilmId { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    // Navigation properties
    public virtual DirectedBy DirectedBy { get; set; } = null!;
    public virtual Film Film { get; set; } = null!;
}
