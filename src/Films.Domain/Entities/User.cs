namespace Films.Domain.Entities;

/// <summary>
/// Represents a user entity
/// </summary>
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? TypeUserId { get; set; }
    public int? SexId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public virtual TypeUser? TypeUser { get; set; }
    public virtual Sex? Sex { get; set; }
    public virtual ICollection<UserRight> UserRights { get; set; } = new List<UserRight>();
}
