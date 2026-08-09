using System.Text;
using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Roslyn;

/// <summary>
/// Genera la configuración EF Core.
/// </summary>
public sealed class ConfigurationSourceBuilder
{
    public string Build(ModuleDefinition module)
    {
        var sb = new StringBuilder();

        BuildHeader(sb, module);
        BuildTable(sb, module);
        BuildKey(sb);
        BuildProperties(sb, module);
        BuildIndexes(sb, module);
        BuildFooter(sb);

        return sb.ToString();
    }

    private static void BuildHeader(
        StringBuilder sb,
        ModuleDefinition module)
    {
        sb.AppendLine("using Touchliga.Domain.Entities;");
        sb.AppendLine("using Microsoft.EntityFrameworkCore;");
        sb.AppendLine("using Microsoft.EntityFrameworkCore.Metadata.Builders;");
        sb.AppendLine();

        sb.AppendLine("namespace Touchliga.Persistence.Configurations;");
        sb.AppendLine();

        sb.AppendLine(
            $"public sealed class {module.Entity}Configuration : IEntityTypeConfiguration<{module.Entity}>");
        sb.AppendLine("{");

        sb.AppendLine(
            $"    public void Configure(EntityTypeBuilder<{module.Entity}> builder)");
        sb.AppendLine("    {");
    }

    private static void BuildTable(
        StringBuilder sb,
        ModuleDefinition module)
    {
        sb.AppendLine(
            $"        builder.ToTable(\"{module.Table}\", \"{module.Schema}\");");
        sb.AppendLine();
    }

    private static void BuildKey(StringBuilder sb)
    {
        sb.AppendLine("        builder.HasKey(x => x.Id);");
        sb.AppendLine();
    }

    private static void BuildProperties(
        StringBuilder sb,
        ModuleDefinition module)
    {
        foreach (var field in module.Fields)
        {
            if (field.IsKey)
                continue;

            sb.Append($"        builder.Property(x => x.{field.Name})");

            if (field.Length.HasValue)
            {
                sb.AppendLine();
                sb.Append(
                    $"            .HasMaxLength({field.Length.Value})");
            }

            if (field.Required)
            {
                sb.AppendLine();
                sb.Append("            .IsRequired()");
            }

            sb.AppendLine(";");
            sb.AppendLine();
        }
    }

    private static void BuildIndexes(
        StringBuilder sb,
        ModuleDefinition module)
    {
        foreach (var field in module.Fields.Where(f => f.IsUnique))
        {
            sb.AppendLine(
                $"        builder.HasIndex(x => x.{field.Name})");

            sb.AppendLine(
                "            .IsUnique();");

            sb.AppendLine();
        }
    }

    private static void BuildFooter(
        StringBuilder sb)
    {
        sb.AppendLine("    }");
        sb.AppendLine("}");
    }
}
