using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class PagoConfiguration : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> builder)
    {
        builder.ToTable("Pago", "com");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Monto).HasPrecision(10, 2);
        builder.Property(x => x.MetodoPago).HasMaxLength(50).IsRequired();
        builder.Property(x => x.FechaPago).IsRequired();
        builder.Property(x => x.Referencia).HasMaxLength(150);

        // Un usuario puede tener varios pagos por temporada (pago
        // completo, o parcial + el resto después) — por eso este
        // índice ya NO es único, solo ayuda a que sea rápido buscar
        // "los pagos de este usuario en esta temporada".
        builder.HasIndex(x => new { x.UsuarioId, x.TemporadaId });

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Temporada>()
            .WithMany()
            .HasForeignKey(x => x.TemporadaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
