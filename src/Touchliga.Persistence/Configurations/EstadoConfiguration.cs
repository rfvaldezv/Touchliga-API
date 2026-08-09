using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class EstadoConfiguration : IEntityTypeConfiguration<Estado>
{
    public void Configure(EntityTypeBuilder<Estado> builder)
    {
        builder.ToTable("Estado", "cat");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PaisId).IsRequired();

        builder.HasOne<Pais>()
            .WithMany()
            .HasForeignKey(x => x.PaisId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
