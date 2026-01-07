using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

public class DirectorConfiguration : IEntityTypeConfiguration<Director>
{
    public void Configure(EntityTypeBuilder<Director> builder)
    {
        builder.ToTable("DirectedBy");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Surname)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(d => d.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(d => d.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.ModifiedBy)
            .HasMaxLength(100);

        builder.HasOne(d => d.Sex)
            .WithMany(s => s.Directors)
            .HasForeignKey(d => d.SexId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
