using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class UsuarioRolConfiguration : IEntityTypeConfiguration<UsuarioRol>
{
    public void Configure(EntityTypeBuilder<UsuarioRol> builder)
    {
        builder.ToTable("UsuarioRol", "seg");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("UsuarioRolId");

        builder.HasOne(x => x.Usuario)
            .WithMany(x => x.Roles)
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Rol)
            .WithMany()
            .HasForeignKey(x => x.RolId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.UsuarioId,
            x.RolId
        }).IsUnique();

        builder.Property(x => x.Activo);

        builder.Property(x => x.FechaAlta);

        builder.Property(x => x.UsuarioAltaId);

        builder.Property(x => x.FechaModificacion);

        builder.Property(x => x.UsuarioModificacionId);

        builder.Property(x => x.RowVersion)
            .IsRowVersion();
    }
}

