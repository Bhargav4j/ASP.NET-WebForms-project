namespace Films.Domain.Entities;

/// <summary>
/// Represents a user type/role lookup entity
/// </summary>
public class TypeUser
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<UserRight> UserRights { get; set; } = new List<UserRight>();
}
