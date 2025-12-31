namespace Films.Domain.Entities;

/// <summary>
/// Represents a film entity in the system
/// </summary>
public class Film
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? Year { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual ICollection<RefAF> RefAFs { get; set; } = new List<RefAF>();
    public virtual ICollection<RefDAF> RefDAFs { get; set; } = new List<RefDAF>();
}
