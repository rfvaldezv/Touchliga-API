using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class AnuncioConfiguration : IEntityTypeConfiguration<Anuncio>
{
    public void Configure(EntityTypeBuilder<Anuncio> builder)
    {
        builder.ToTable("Anuncio", "com");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Titulo).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Contenido).HasMaxLength(2000).IsRequired();
        builder.Property(x => x.ImagenUrl).HasMaxLength(500);
        builder.Property(x => x.UsuarioAutorId).IsRequired();
        builder.Property(x => x.FechaPublicacion).IsRequired();

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.UsuarioAutorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
