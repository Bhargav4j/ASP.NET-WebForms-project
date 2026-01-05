using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for Actor entity
/// </summary>
public class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.ToTable("Actors");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Description)
            .HasMaxLength(1000);

        builder.Property(a => a.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(a => a.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(a => a.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.ModifiedBy)
            .HasMaxLength(100);

        builder.HasOne(a => a.Sex)
            .WithMany(s => s.Actors)
            .HasForeignKey(a => a.SexId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.RefAFs)
            .WithOne(r => r.Actor)
            .HasForeignKey(r => r.ActorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
