namespace Films.Domain.Entities;

/// <summary>
/// Represents the relationship between Directors and Films
/// </summary>
public class RefDAF
{
    public int Id { get; set; }
    public int DirectorId { get; set; }
    public int FilmId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Director Director { get; set; } = null!;
    public virtual Film Film { get; set; } = null!;
}
