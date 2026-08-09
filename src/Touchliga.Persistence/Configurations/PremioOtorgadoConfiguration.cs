using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class PremioOtorgadoConfiguration : IEntityTypeConfiguration<PremioOtorgado>
{
    public void Configure(EntityTypeBuilder<PremioOtorgado> builder)
    {
        builder.ToTable("PremioOtorgado", "com");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Ambito).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Estado).HasMaxLength(20).IsRequired();
        builder.Property(x => x.MontoAjustado).HasPrecision(10, 2);
        builder.Property(x => x.Motivo).HasMaxLength(250);

        // Una sola decisión vigente por persona, por jornada/final.
        builder.HasIndex(x => new { x.Ambito, x.ReferenciaId, x.UsuarioId }).IsUnique();

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
