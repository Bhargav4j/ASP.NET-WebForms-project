using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for Film entity
/// </summary>
public class FilmConfiguration : IEntityTypeConfiguration<Film>
{
    public void Configure(EntityTypeBuilder<Film> builder)
    {
        builder.ToTable("Films");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(f => f.Description)
            .HasMaxLength(1000);

        builder.Property(f => f.Genre)
            .HasMaxLength(100);

        builder.Property(f => f.Year)
            .IsRequired();

        builder.Property(f => f.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(f => f.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(f => f.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.ModifiedBy)
            .HasMaxLength(100);

        builder.HasMany(f => f.RefAFs)
            .WithOne(r => r.Film)
            .HasForeignKey(r => r.FilmId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(f => f.RefDAFs)
            .WithOne(r => r.Film)
            .HasForeignKey(r => r.FilmId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
