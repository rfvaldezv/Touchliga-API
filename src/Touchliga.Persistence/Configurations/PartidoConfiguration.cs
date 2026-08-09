using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class PartidoConfiguration : IEntityTypeConfiguration<Partido>
{
    public void Configure(EntityTypeBuilder<Partido> builder)
    {
        builder.ToTable("Partido", "cat");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.JornadaId).IsRequired();
        builder.Property(x => x.EquipoLocalId).IsRequired();
        builder.Property(x => x.EquipoVisitanteId).IsRequired();
        builder.Property(x => x.FechaHora).IsRequired();
        builder.Property(x => x.CanchaId);
        builder.Property(x => x.GolesLocal);
        builder.Property(x => x.GolesVisitante);
        builder.Property(x => x.EsDesempate).IsRequired();

        builder.Ignore(x => x.TieneResultado);
        builder.Ignore(x => x.TotalPuntosReal);
        builder.Ignore(x => x.DiferenciaPuntosReal);

        builder.HasOne<Jornada>()
            .WithMany()
            .HasForeignKey(x => x.JornadaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Equipo>()
            .WithMany()
            .HasForeignKey(x => x.EquipoLocalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Equipo>()
            .WithMany()
            .HasForeignKey(x => x.EquipoVisitanteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Cancha>()
            .WithMany()
            .HasForeignKey(x => x.CanchaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
