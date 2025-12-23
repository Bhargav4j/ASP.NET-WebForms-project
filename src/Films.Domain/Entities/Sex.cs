namespace Films.Domain.Entities;

/// <summary>
/// Represents a sex/gender entity
/// </summary>
public class Sex
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual ICollection<Actor> Actors { get; set; } = new List<Actor>();
    public virtual ICollection<DirectedBy> DirectedBys { get; set; } = new List<DirectedBy>();
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
