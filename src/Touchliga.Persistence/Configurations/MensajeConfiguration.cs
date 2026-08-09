using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class MensajeConfiguration : IEntityTypeConfiguration<Mensaje>
{
    public void Configure(EntityTypeBuilder<Mensaje> builder)
    {
        builder.ToTable("Mensaje", "com");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Contenido).HasMaxLength(2000).IsRequired();
        builder.Property(x => x.ImagenUrl).HasMaxLength(500);
        builder.Property(x => x.FechaEnvio).IsRequired();
        builder.Property(x => x.Leido).IsRequired();

        builder.HasIndex(x => new { x.RemitenteId, x.DestinatarioId });

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.RemitenteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.DestinatarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
