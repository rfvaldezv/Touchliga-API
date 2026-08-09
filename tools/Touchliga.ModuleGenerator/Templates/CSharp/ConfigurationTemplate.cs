public static class ConfigurationTemplate
{
    public const string Source =
"""
using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class {{Entity}}Configuration
    : IEntityTypeConfiguration<{{Entity}}>
{
    public void Configure(EntityTypeBuilder<{{Entity}}> builder)
    {
        builder.ToTable("{{Plural}}","{{Schema}}");

{{Properties}}

{{Relations}}
    }
}
""";
}
