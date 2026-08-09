using System.Text;
using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Roslyn;

public sealed class CommandSourceBuilder
{
    public string BuildCreate(ModuleDefinition module)
    {
        return Build(module, "Create");
    }

    public string BuildUpdate(ModuleDefinition module)
    {
        return Build(module, "Update");
    }

    public string BuildDelete(ModuleDefinition module)
    {
        return
$@"using MediatR;

namespace Touchliga.Application.Commands.{module.Entity}.Delete;

/// <summary>
/// Elimina un {module.Entity}.
/// </summary>
public sealed record Delete{module.Entity}Command(
    long Id)
    : IRequest<Unit>;
";
    }

    private string Build(
        ModuleDefinition module,
        string action)
    {
        var sb = new StringBuilder();

        sb.AppendLine("using MediatR;");
        sb.AppendLine();

        sb.AppendLine(
            $"namespace Touchliga.Application.Commands.{module.Entity}.{action};");

        sb.AppendLine();

        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// {action} {module.Entity}.");
        sb.AppendLine("/// </summary>");

        sb.AppendLine(
            $"public sealed record {action}{module.Entity}Command(");

        var parameters = new List<string>();

        if (action == "Update")
        {
            parameters.Add("long Id");
        }

        parameters.AddRange(
            module.Fields
                .Where(f => !f.IsKey)
                .Select(f => $"{Map(f.Type)} {f.Name}")
        );

        for (int i = 0; i < parameters.Count; i++)
        {
            var comma = i < parameters.Count - 1 ? "," : "";

            sb.AppendLine(
                $"    {parameters[i]}{comma}");
        }

        sb.AppendLine(")");

        sb.AppendLine("    : IRequest<long>;");

        return sb.ToString();
    }

    private static string Map(FieldType type)
    {
        return type switch
        {
            FieldType.String => "string",
            FieldType.Bool => "bool",
            FieldType.Int => "int",
            FieldType.Long => "long",
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
