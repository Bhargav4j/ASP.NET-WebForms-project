namespace Films.Domain.Entities;

public class UserRight
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RightId { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    // Navigation properties
    public User User { get; set; } = null!;
    public Right Right { get; set; } = null!;
}
