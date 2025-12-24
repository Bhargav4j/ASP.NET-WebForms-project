namespace Films.Domain.Entities;

/// <summary>
/// Represents a permission/right entity in the system
/// </summary>
public class Right
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<UserRight> UserRights { get; set; } = new List<UserRight>();
}
