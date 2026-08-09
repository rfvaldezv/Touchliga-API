using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class JugadorConfiguration : IEntityTypeConfiguration<Jugador>
{
    public void Configure(EntityTypeBuilder<Jugador> builder)
    {
        builder.ToTable("Jugador", "cat");

        builder.HasKey(x => x.Id);

    }
}
