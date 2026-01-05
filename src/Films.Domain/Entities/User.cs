namespace Films.Domain.Entities;

/// <summary>
/// Represents a user entity
/// </summary>
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? SecretQuestion { get; set; }
    public string? SecretAnswer { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? SexId { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? PhoneNumber { get; set; }
    public int? TypeUserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Sex? Sex { get; set; }
    public virtual TypeUser? TypeUser { get; set; }
    public virtual ICollection<UserRight> UserRights { get; set; } = new List<UserRight>();
}
