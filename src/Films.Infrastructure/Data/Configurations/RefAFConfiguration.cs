using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

public class RefAFConfiguration : IEntityTypeConfiguration<RefAF>
{
    public void Configure(EntityTypeBuilder<RefAF> builder)
    {
        builder.ToTable("RefAF");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(r => r.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(r => r.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(r => r.Actor)
            .WithMany(a => a.RefAFs)
            .HasForeignKey(r => r.ActorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Film)
            .WithMany(f => f.RefAFs)
            .HasForeignKey(r => r.FilmId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
