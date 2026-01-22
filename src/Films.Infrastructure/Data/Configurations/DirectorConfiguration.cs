using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Films.Domain.Entities;

namespace Films.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Director entity
/// </summary>
public class DirectorConfiguration : IEntityTypeConfiguration<Director>
{
    public void Configure(EntityTypeBuilder<Director> builder)
    {
        builder.ToTable("Directors");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Surname)
            .HasMaxLength(100);

        builder.Property(d => d.Country)
            .HasMaxLength(100);

        builder.Property(d => d.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.ModifiedBy)
            .HasMaxLength(100);

        builder.Property(d => d.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(d => d.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(d => d.Sex)
            .WithMany(s => s.Directors)
            .HasForeignKey(d => d.SexId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(d => d.RefDAFs)
            .WithOne(r => r.Director)
            .HasForeignKey(r => r.DirectorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
