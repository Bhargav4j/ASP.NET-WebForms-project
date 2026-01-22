namespace Films.Domain.Entities;

/// <summary>
/// Represents a director entity in the system
/// </summary>
public class Director
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Surname { get; set; }
    public int? SexId { get; set; }
    public string? Country { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public virtual Sex? Sex { get; set; }
    public virtual ICollection<RefDAF> RefDAFs { get; set; } = new List<RefDAF>();
}
