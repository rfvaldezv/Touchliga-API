using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class ArchivoConfiguration : IEntityTypeConfiguration<Archivo>
{
    public void Configure(EntityTypeBuilder<Archivo> builder)
    {
        builder.ToTable("Archivo", "com");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.NombreArchivo).HasMaxLength(260).IsRequired();
        builder.Property(x => x.ContentType).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Datos).HasColumnType("varbinary(max)").IsRequired();
    }
}
