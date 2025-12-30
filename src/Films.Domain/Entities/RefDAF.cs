namespace Films.Domain.Entities;

/// <summary>
/// Represents the relationship between directors and films
/// </summary>
public class RefDAF
{
    public int Id { get; set; }
    public int DirectorId { get; set; }
    public int FilmId { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }

    public virtual Director Director { get; set; } = null!;
    public virtual Film Film { get; set; } = null!;
}
