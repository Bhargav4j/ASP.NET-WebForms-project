namespace Films.Domain.Entities;

/// <summary>
/// Represents a user entity in the system
/// </summary>
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? FullName => $"{FirstName} {LastName}";
    public int? SexId { get; set; }
    public int TypeUserId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Sex? Sex { get; set; }
    public virtual TypeUser TypeUser { get; set; } = null!;
    public virtual ICollection<UserRight> UserRights { get; set; } = new List<UserRight>();
}
