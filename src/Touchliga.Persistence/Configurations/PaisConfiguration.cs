using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class PaisConfiguration : IEntityTypeConfiguration<Pais>
{
    public void Configure(EntityTypeBuilder<Pais> builder)
    {
        builder.ToTable("Pais", "cat");

        builder.HasKey(x => x.Id);

    }
}
