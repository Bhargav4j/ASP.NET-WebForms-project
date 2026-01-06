namespace Films.Domain.Entities;

/// <summary>
/// Represents a director entity in the system
/// </summary>
public class DirectedBy
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Surname { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; } = true;

    public string CreatedBy { get; set; } = "System";

    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual ICollection<RefDAF> RefDAFs { get; set; } = new List<RefDAF>();
}
