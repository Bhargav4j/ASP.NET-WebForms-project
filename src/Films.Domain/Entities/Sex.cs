namespace Films.Domain.Entities;

/// <summary>
/// Represents a sex/gender lookup entity
/// </summary>
public class Sex
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Actor> Actors { get; set; } = new List<Actor>();
    public virtual ICollection<Director> Directors { get; set; } = new List<Director>();
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
