namespace Films.Domain.Entities;

/// <summary>
/// Represents a sex/gender entity in the system
/// </summary>
public class Sex
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; } = true;

    public string CreatedBy { get; set; } = "System";

    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual ICollection<Actor> Actors { get; set; } = new List<Actor>();
}
