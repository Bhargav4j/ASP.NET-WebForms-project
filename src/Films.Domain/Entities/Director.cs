namespace Films.Domain.Entities;

/// <summary>
/// Represents a director entity
/// </summary>
public class Director
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? SexId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public virtual Sex? Sex { get; set; }
    public virtual ICollection<RefDAF> RefDAFs { get; set; } = new List<RefDAF>();
}
