using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Touchliga.Persistence.Configurations;

public sealed class ConfiguracionSmtpConfiguration : IEntityTypeConfiguration<ConfiguracionSmtp>
{
    public void Configure(EntityTypeBuilder<ConfiguracionSmtp> builder)
    {
        builder.ToTable("ConfiguracionSmtp", "com");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Host).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Username).HasMaxLength(200);
        builder.Property(x => x.Password).HasMaxLength(200);
        builder.Property(x => x.FromEmail).HasMaxLength(200).IsRequired();
        builder.Property(x => x.FromName).HasMaxLength(100).IsRequired();
    }
}
