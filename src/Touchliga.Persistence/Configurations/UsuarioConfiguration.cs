using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuario", "seg");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("UsuarioId");

        builder.Property(x => x.Nombre)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Apellidos)
            .HasMaxLength(150);

        builder.Property(x => x.Telefono)
            .HasMaxLength(20);

        builder.OwnsOne(x => x.Correo, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Correo")
                .HasMaxLength(150)
                .IsRequired();

            email.HasIndex(e => e.Value)
                .IsUnique();
        });

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.EmailConfirmado)
            .IsRequired();

        builder.Property(x => x.InvitadoPorId);

        builder.Property(x => x.CiudadId);

        builder.Property(x => x.PaisId);

        builder.Property(x => x.EstadoId);

        builder.Property(x => x.Estatus)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Sexo)
            .HasMaxLength(1);

        builder.Property(x => x.FechaNacimiento);

        builder.Property(x => x.EquipoFavoritoId);

        builder.Property(x => x.Nickname)
            .HasMaxLength(50);

        builder.Property(x => x.FotoUrl)
            .HasMaxLength(500);

        builder.Property(x => x.ParejaId);

        builder.Property(x => x.NombreEquipo)
            .HasMaxLength(100);

        builder.Property(x => x.EsCuentaVinculada)
            .HasDefaultValue(false);

        builder.HasOne<Equipo>()
            .WithMany()
            .HasForeignKey(x => x.EquipoFavoritoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Auto-referencia: quién invitó a este usuario.
        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.InvitadoPorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Auto-referencia: con quién juega en pareja/equipo (opcional,
        // solo visual -- ver comentario en la entidad).
        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.ParejaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Ciudad>()
            .WithMany()
            .HasForeignKey(x => x.CiudadId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Pais>()
            .WithMany()
            .HasForeignKey(x => x.PaisId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Estado>()
            .WithMany()
            .HasForeignKey(x => x.EstadoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.Activo)
            .IsRequired();

        builder.Property(x => x.FechaAlta)
            .IsRequired();

        builder.Property(x => x.UsuarioAltaId)
            .IsRequired();

        builder.Property(x => x.FechaModificacion);

        builder.Property(x => x.UsuarioModificacionId);

        builder.Property(x => x.RowVersion)
            .IsRowVersion();
    }
}
