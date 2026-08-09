using System.Text;
using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Roslyn;

/// <summary>
/// Genera los DTOs de Application.
/// </summary>
public sealed class DtoSourceBuilder
{
    public string Build(ModuleDefinition module)
    {
        var sb = new StringBuilder();

        sb.AppendLine("namespace Touchliga.Application.DTOs;");
        sb.AppendLine();

        sb.AppendLine($"public sealed class {module.Entity}Dto");
        sb.AppendLine("{");

        sb.AppendLine("    public long Id { get; set; }");
        sb.AppendLine();

        foreach (var field in module.Fields)
        {
            if (field.IsKey)
                continue;

            sb.AppendLine(
                $"    public {Map(field.Type)} {field.Name} {{ get; set; }}");
            sb.AppendLine();
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string Map(FieldType type)
    {
        return type switch
        {
            FieldType.String => "string",
            FieldType.Int => "int",
            FieldType.Long => "long",
            FieldType.Bool => "bool",
            FieldType.Decimal => "decimal",
            FieldType.Double => "double",
            FieldType.Float => "float",
            FieldType.Date => "DateOnly",
            FieldType.DateTime => "DateTime",
            FieldType.Guid => "Guid",
            _ => "string"
        };
    }
}
