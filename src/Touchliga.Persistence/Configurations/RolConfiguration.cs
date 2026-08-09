using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.ToTable("Rol", "seg");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("RolId");

        builder.Property(x => x.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Descripcion)
            .HasMaxLength(250);

        builder.Property(x => x.RowVersion)
            .IsRowVersion();

        builder.Ignore(x => x.Permisos);

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
