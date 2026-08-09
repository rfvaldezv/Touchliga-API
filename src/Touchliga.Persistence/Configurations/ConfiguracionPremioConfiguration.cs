using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class ConfiguracionPremioConfiguration : IEntityTypeConfiguration<ConfiguracionPremio>
{
    public void Configure(EntityTypeBuilder<ConfiguracionPremio> builder)
    {
        builder.ToTable("ConfiguracionPremio", "com");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Ambito).HasMaxLength(20).IsRequired();
        builder.Property(x => x.TipoPremio).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Monto).HasPrecision(10, 2);
        builder.Property(x => x.Descripcion).HasMaxLength(250);

        // Una sola configuración por posición dentro de un mismo
        // ámbito y temporada (no puede haber dos "1er lugar" en
        // "Jornada" de la misma temporada).
        builder.HasIndex(x => new { x.TemporadaId, x.Ambito, x.Posicion }).IsUnique();

        builder.HasOne<Temporada>()
            .WithMany()
            .HasForeignKey(x => x.TemporadaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
