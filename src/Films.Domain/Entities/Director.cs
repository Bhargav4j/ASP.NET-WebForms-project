namespace Films.Domain.Entities;

public class Director
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public int? SexId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public Sex? Sex { get; set; }
    public ICollection<RefDAF> RefDAFs { get; set; } = new List<RefDAF>();
}
