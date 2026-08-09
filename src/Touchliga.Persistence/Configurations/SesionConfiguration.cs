using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class SesionConfiguration : IEntityTypeConfiguration<Sesion>
{
    public void Configure(EntityTypeBuilder<Sesion> builder)
    {
        builder.ToTable("Sesion", "seg");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("SesionId");

        builder.Property(x => x.DireccionIp)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Dispositivo)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.SistemaOperativo)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Navegador)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);            

        builder.Property(x => x.RowVersion)
            .IsRowVersion();

        builder.Property(x => x.Activo)
            .IsRequired();

        builder.Property(x => x.FechaAlta)
            .IsRequired();

        builder.Property(x => x.UsuarioAltaId)
            .IsRequired();

        builder.Property(x => x.FechaModificacion);

        builder.Property(x => x.UsuarioModificacionId);
    }
}
