using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class TemporadaConfiguration : IEntityTypeConfiguration<Temporada>
{
    public void Configure(EntityTypeBuilder<Temporada> builder)
    {
        builder.ToTable("Temporada", "cat");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Codigo).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Nombre).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Descripcion).HasMaxLength(500);

        builder.Property(x => x.LigaId).IsRequired();
        builder.Property(x => x.FechaInicio).IsRequired();
        builder.Property(x => x.FechaFin).IsRequired();
        builder.Property(x => x.Cuota).HasPrecision(10, 2);

        builder.HasOne<Liga>()
            .WithMany()
            .HasForeignKey(x => x.LigaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
