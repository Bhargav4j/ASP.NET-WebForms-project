namespace Films.Domain.Entities;

/// <summary>
/// Represents an actor entity in the system
/// </summary>
public class Actor
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Surname { get; set; }

    public int? SexId { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; } = true;

    public string CreatedBy { get; set; } = "System";

    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Sex? Sex { get; set; }

    public virtual ICollection<RefAF> RefAFs { get; set; } = new List<RefAF>();
}
