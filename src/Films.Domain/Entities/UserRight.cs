namespace Films.Domain.Entities;

/// <summary>
/// Represents a many-to-many relationship between users and rights
/// </summary>
public class UserRight
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RightId { get; set; }
    public int TypeUserId { get; set; }
    public DateTime GrantedDate { get; set; } = DateTime.UtcNow;
    public string GrantedBy { get; set; } = "System";
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Right Right { get; set; } = null!;
    public virtual TypeUser TypeUser { get; set; } = null!;
}
