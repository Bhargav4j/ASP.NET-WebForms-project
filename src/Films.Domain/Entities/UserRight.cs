namespace Films.Domain.Entities;

/// <summary>
/// Represents the relationship between users and rights/permissions
/// </summary>
public class UserRight
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RightId { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Right Right { get; set; } = null!;
}
