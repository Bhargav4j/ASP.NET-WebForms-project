namespace Films.Domain.Entities;

/// <summary>
/// Represents an actor entity
/// </summary>
public class Actor
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
    public virtual ICollection<RefAF> RefAFs { get; set; } = new List<RefAF>();
}
