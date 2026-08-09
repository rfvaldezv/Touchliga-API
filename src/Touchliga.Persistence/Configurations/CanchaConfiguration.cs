using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class CanchaConfiguration : IEntityTypeConfiguration<Cancha>
{
    public void Configure(EntityTypeBuilder<Cancha> builder)
    {
        builder.ToTable("Cancha", "cat");

        builder.HasKey(x => x.Id);

    }
}
