using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class PatrocinadorConfiguration : IEntityTypeConfiguration<Patrocinador>
{
    public void Configure(EntityTypeBuilder<Patrocinador> builder)
    {
        builder.ToTable("Patrocinador", "cat");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Codigo).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Nombre).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Descripcion).HasMaxLength(500);
        builder.Property(x => x.ImagenUrl).HasMaxLength(500).IsRequired();
        builder.Property(x => x.EnlaceUrl).HasMaxLength(500);
        builder.Property(x => x.Orden).IsRequired();
    }
}
