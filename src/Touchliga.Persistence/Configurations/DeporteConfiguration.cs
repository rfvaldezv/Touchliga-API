using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class DeporteConfiguration : IEntityTypeConfiguration<Deporte>
{
    public void Configure(EntityTypeBuilder<Deporte> builder)
    {
        builder.ToTable("Deporte", "cat");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("DeporteId");

        builder.Property(x => x.Codigo)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Descripcion)
            .HasMaxLength(250);

        builder.HasIndex(x => x.Codigo)
            .IsUnique();
    }
}
