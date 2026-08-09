using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class EquipoConfiguration : IEntityTypeConfiguration<Equipo>
{
    public void Configure(EntityTypeBuilder<Equipo> builder)
    {
        builder.ToTable("Equipo", "cat");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EscudoUrl).HasMaxLength(500);

        builder.Property(x => x.Apodo).HasMaxLength(100);

    }
}
