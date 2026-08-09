using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class JornadaConfiguration : IEntityTypeConfiguration<Jornada>
{
    public void Configure(EntityTypeBuilder<Jornada> builder)
    {
        builder.ToTable("Jornada", "cat");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Codigo).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Nombre).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Descripcion).HasMaxLength(500);

        builder.Property(x => x.TemporadaId).IsRequired();
        builder.Property(x => x.Numero).IsRequired();
        builder.Property(x => x.FechaCierre).IsRequired();
        builder.Property(x => x.Cerrada).IsRequired();

        builder.HasOne<Temporada>()
            .WithMany()
            .HasForeignKey(x => x.TemporadaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
