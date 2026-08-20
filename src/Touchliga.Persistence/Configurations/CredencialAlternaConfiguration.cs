using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class CredencialAlternaConfiguration : IEntityTypeConfiguration<CredencialAlterna>
{
    public void Configure(EntityTypeBuilder<CredencialAlterna> builder)
    {
        builder.ToTable("CredencialAlterna", "seg");

        builder.HasKey(x => x.Id);

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

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

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
