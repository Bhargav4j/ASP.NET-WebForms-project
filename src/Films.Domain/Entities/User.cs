namespace Films.Domain.Entities;

/// <summary>
/// Represents a user entity in the system
/// </summary>
public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int? TypeUserId { get; set; }

    public string? PhoneNumber { get; set; }

    public string? SecretQuestion { get; set; }

    public string? SecretAnswer { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; } = true;

    public string CreatedBy { get; set; } = "System";

    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual TypeUser? TypeUser { get; set; }

    public virtual ICollection<UserRight> UserRights { get; set; } = new List<UserRight>();
}
