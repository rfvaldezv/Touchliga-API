using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class CiudadConfiguration : IEntityTypeConfiguration<Ciudad>
{
    public void Configure(EntityTypeBuilder<Ciudad> builder)
    {
        builder.ToTable("Ciudad", "cat");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PaisId).IsRequired();
        builder.Property(x => x.EstadoId).IsRequired();

        builder.HasOne<Pais>()
            .WithMany()
            .HasForeignKey(x => x.PaisId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Estado>()
            .WithMany()
            .HasForeignKey(x => x.EstadoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
