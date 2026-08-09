using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class ReaccionAnuncioConfiguration : IEntityTypeConfiguration<ReaccionAnuncio>
{
    public void Configure(EntityTypeBuilder<ReaccionAnuncio> builder)
    {
        builder.ToTable("ReaccionAnuncio", "com");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Emoji).HasMaxLength(10).IsRequired();

        // Una sola reacción por persona por anuncio.
        builder.HasIndex(x => new { x.AnuncioId, x.UsuarioId }).IsUnique();

        builder.HasOne<Anuncio>()
            .WithMany()
            .HasForeignKey(x => x.AnuncioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
