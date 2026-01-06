namespace Films.Domain.Entities;

/// <summary>
/// Represents a user type entity in the system
/// </summary>
public class TypeUser
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; } = true;

    public string CreatedBy { get; set; } = "System";

    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
