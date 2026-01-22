using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Films.Domain.Entities;

namespace Films.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Actor entity
/// </summary>
public class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.ToTable("Actors");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Surname)
            .HasMaxLength(100);

        builder.Property(a => a.Country)
            .HasMaxLength(100);

        builder.Property(a => a.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.ModifiedBy)
            .HasMaxLength(100);

        builder.Property(a => a.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(a => a.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(a => a.Sex)
            .WithMany(s => s.Actors)
            .HasForeignKey(a => a.SexId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(a => a.RefAFs)
            .WithOne(r => r.Actor)
            .HasForeignKey(r => r.ActorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
