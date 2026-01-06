namespace Films.Domain.Entities;

/// <summary>
/// Represents a right/permission entity in the system
/// </summary>
public class Right
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; } = true;

    public string CreatedBy { get; set; } = "System";

    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual ICollection<UserRight> UserRights { get; set; } = new List<UserRight>();
}
