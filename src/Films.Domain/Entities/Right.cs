namespace Films.Domain.Entities;

/// <summary>
/// Represents a right/permission entity in the system
/// </summary>
public class Right
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public virtual ICollection<UserRight> UserRights { get; set; } = new List<UserRight>();
}
