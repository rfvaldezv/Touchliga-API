using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshToken", "seg");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("RefreshTokenId");

        builder.Property(x => x.Token)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.RowVersion)
            .IsRowVersion();

        builder.HasIndex(x => x.Token)
            .IsUnique();

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.Expira)
            .IsRequired();

        builder.Property(x => x.Revocado)
            .IsRequired();

        builder.Property(x => x.FechaRevocacion);

        builder.Property(x => x.MotivoRevocacion)
            .HasMaxLength(250);

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
