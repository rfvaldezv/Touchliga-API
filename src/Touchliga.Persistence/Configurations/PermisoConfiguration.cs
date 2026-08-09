using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class PermisoConfiguration : IEntityTypeConfiguration<Permiso>
{
    public void Configure(EntityTypeBuilder<Permiso> builder)
    {
        builder.ToTable("Permiso", "seg");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("PermisoId");

        builder.Property(x => x.Codigo)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(x => x.Codigo)
            .IsUnique();

        builder.Property(x => x.Nombre)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Descripcion)
            .HasMaxLength(300);

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
