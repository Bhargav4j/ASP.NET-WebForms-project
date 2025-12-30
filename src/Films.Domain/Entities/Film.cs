namespace Films.Domain.Entities;

/// <summary>
/// Represents a film in the system
/// </summary>
public class Film
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? Year { get; set; }
    public string? Genre { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public virtual ICollection<RefAF> ActorFilms { get; set; } = new List<RefAF>();
    public virtual ICollection<RefDAF> DirectorFilms { get; set; } = new List<RefDAF>();
}
