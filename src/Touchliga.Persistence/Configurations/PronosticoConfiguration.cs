using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class PronosticoConfiguration : IEntityTypeConfiguration<Pronostico>
{
    public void Configure(EntityTypeBuilder<Pronostico> builder)
    {
        builder.ToTable("Pronostico", "cat");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PartidoId).IsRequired();
        builder.Property(x => x.UsuarioId).IsRequired();
        builder.Property(x => x.EquipoGanadorId).IsRequired();
        builder.Property(x => x.Puntos);
        builder.Property(x => x.PuntosTotalesPredichos);
        builder.Property(x => x.DiferenciaPuntosPredicha);
        builder.Property(x => x.PuntosBono).IsRequired();

        // Un usuario solo puede tener un pronóstico por partido.
        builder.HasIndex(x => new { x.PartidoId, x.UsuarioId }).IsUnique();

        builder.HasOne<Partido>()
            .WithMany()
            .HasForeignKey(x => x.PartidoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Equipo>()
            .WithMany()
            .HasForeignKey(x => x.EquipoGanadorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
