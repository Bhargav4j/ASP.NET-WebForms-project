namespace Films.Domain.Entities;

/// <summary>
/// Represents a director in the system
/// </summary>
public class DirectedBy
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual ICollection<RefDAF> RefDAFs { get; set; } = new List<RefDAF>();
}
