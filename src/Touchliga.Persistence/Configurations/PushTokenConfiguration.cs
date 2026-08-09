using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class PushTokenConfiguration : IEntityTypeConfiguration<PushToken>
{
    public void Configure(EntityTypeBuilder<PushToken> builder)
    {
        builder.ToTable("PushToken", "seg");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Token).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Plataforma).HasMaxLength(20).IsRequired();

        // Un mismo token de dispositivo no se duplica — si ya existe,
        // se actualiza a qué usuario pertenece (por si cierra sesión
        // y entra otro usuario en el mismo celular).
        builder.HasIndex(x => x.Token).IsUnique();

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
