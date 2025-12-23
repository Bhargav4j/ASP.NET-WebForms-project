namespace Films.Domain.Entities;

/// <summary>
/// Represents a director entity
/// </summary>
public class DirectedBy
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Surname { get; set; }
    public int? IdSex { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Sex? Sex { get; set; }
    public virtual ICollection<RefDAF> RefDAFs { get; set; } = new List<RefDAF>();
}
