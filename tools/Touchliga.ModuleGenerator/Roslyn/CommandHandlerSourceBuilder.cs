using System.Text;
using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Roslyn;

/// <summary>
/// Genera los Command Handlers.
/// </summary>
public sealed class CommandHandlerSourceBuilder
{
    public string BuildCreate(ModuleDefinition module)
        => Build(module, "Create", "long");

    public string BuildUpdate(ModuleDefinition module)
        => Build(module, "Update", "long");

    public string BuildDelete(ModuleDefinition module)
        => Build(module, "Delete", "Unit");

    private string Build(
        ModuleDefinition module,
        string action,
        string responseType)
    {
        var sb = new StringBuilder();

        sb.AppendLine("using MediatR;");
        sb.AppendLine("using Touchliga.Domain.Interfaces;");
        sb.AppendLine($"using Touchliga.Application.Commands.{module.Entity}.{action};");
        sb.AppendLine();

        sb.AppendLine($"namespace Touchliga.Application.Handlers.{module.Entity}.{action};");
        sb.AppendLine();

        sb.AppendLine(
            $"public sealed class {action}{module.Entity}CommandHandler : IRequestHandler<{action}{module.Entity}Command, {responseType}>");
        sb.AppendLine("{");

        sb.AppendLine(
            $"    private readonly I{module.Entity}Repository _repository;");
        sb.AppendLine();

        sb.AppendLine(
            $"    public {action}{module.Entity}CommandHandler(");
        sb.AppendLine(
            $"        I{module.Entity}Repository repository)");
        sb.AppendLine("    {");
        sb.AppendLine("        _repository = repository;");
        sb.AppendLine("    }");
        sb.AppendLine();

        sb.AppendLine(
            $"    public async Task<{responseType}> Handle(");
        sb.AppendLine(
            $"        {action}{module.Entity}Command request,");
        sb.AppendLine(
            "        CancellationToken cancellationToken)");
        sb.AppendLine("    {");

        if (responseType == "Unit")
        {
            sb.AppendLine("        throw new NotImplementedException();");
        }
        else
        {
            sb.AppendLine("        throw new NotImplementedException();");
        }

        sb.AppendLine("    }");

        sb.AppendLine("}");

        return sb.ToString();
    }
}
