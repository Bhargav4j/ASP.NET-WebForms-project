using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for Sex entity
/// </summary>
public class SexConfiguration : IEntityTypeConfiguration<Sex>
{
    public void Configure(EntityTypeBuilder<Sex> builder)
    {
        builder.ToTable("Sex");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Description)
            .HasMaxLength(500);

        builder.Property(s => s.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(s => s.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(s => s.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.ModifiedBy)
            .HasMaxLength(100);
    }
}
