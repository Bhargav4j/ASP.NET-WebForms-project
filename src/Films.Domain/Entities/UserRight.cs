namespace Films.Domain.Entities;

/// <summary>
/// Represents the relationship between users and rights
/// </summary>
public class UserRight
{
    public int Id { get; set; }
    public int IdUser { get; set; }
    public int IdRight { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Right Right { get; set; } = null!;
}
