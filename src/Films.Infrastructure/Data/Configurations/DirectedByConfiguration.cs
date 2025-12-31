using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for DirectedBy
/// </summary>
public class DirectedByConfiguration : IEntityTypeConfiguration<DirectedBy>
{
    public void Configure(EntityTypeBuilder<DirectedBy> builder)
    {
        builder.ToTable("DirectedBy");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Surname)
            .HasMaxLength(100);

        builder.Property(e => e.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100);

        builder.HasMany(e => e.RefDAFs)
            .WithOne(e => e.DirectedBy)
            .HasForeignKey(e => e.DirectedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
