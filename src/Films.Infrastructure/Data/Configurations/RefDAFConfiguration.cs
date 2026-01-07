using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

public class RefDAFConfiguration : IEntityTypeConfiguration<RefDAF>
{
    public void Configure(EntityTypeBuilder<RefDAF> builder)
    {
        builder.ToTable("RefDAF");

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

        builder.HasOne(r => r.Director)
            .WithMany(d => d.RefDAFs)
            .HasForeignKey(r => r.DirectorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Film)
            .WithMany(f => f.RefDAFs)
            .HasForeignKey(r => r.FilmId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
