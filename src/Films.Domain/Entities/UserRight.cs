namespace Films.Domain.Entities;

/// <summary>
/// Reference table for User-Right relationship
/// </summary>
public class UserRight
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RightId { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Right Right { get; set; } = null!;
}
