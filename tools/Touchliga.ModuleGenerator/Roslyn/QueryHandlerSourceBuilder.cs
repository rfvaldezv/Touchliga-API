using System.Text;
using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Roslyn;

/// <summary>
/// Genera los Query Handlers.
/// </summary>
public sealed class QueryHandlerSourceBuilder
{
    public string BuildSingle(ModuleDefinition module)
    {
        return Build(
            module,
            "Get",
            $"Get{module.Entity}Query",
            $"{module.Entity}Dto");
    }

    public string BuildCollection(ModuleDefinition module)
    {
        return Build(
            module,
            "GetAll",
            $"Get{module.EntityPlural}Query",
            $"IReadOnlyList<{module.Entity}Dto>");
    }

    private string Build(
        ModuleDefinition module,
        string folder,
        string query,
        string response)
    {
        var sb = new StringBuilder();

        sb.AppendLine("using MediatR;");
        sb.AppendLine("using Touchliga.Domain.Interfaces;");
        sb.AppendLine("using Touchliga.Application.DTOs;");
        sb.AppendLine($"using Touchliga.Application.Queries.{module.Entity}.{folder};");
        sb.AppendLine();

        sb.AppendLine(
            $"namespace Touchliga.Application.Handlers.{module.Entity}.{folder};");

        sb.AppendLine();

        sb.AppendLine(
            $"public sealed class {query}Handler : IRequestHandler<{query}, {response}>");

        sb.AppendLine("{");

        sb.AppendLine(
            $"    private readonly I{module.Entity}Repository _repository;");

        sb.AppendLine();

        sb.AppendLine(
            $"    public {query}Handler(");

        sb.AppendLine(
            $"        I{module.Entity}Repository repository)");

        sb.AppendLine("    {");

        sb.AppendLine("        _repository = repository;");

        sb.AppendLine("    }");

        sb.AppendLine();

        sb.AppendLine(
            $"    public async Task<{response}> Handle(");

        sb.AppendLine(
            $"        {query} request,");

        sb.AppendLine(
            "        CancellationToken cancellationToken)");

        sb.AppendLine("    {");

        sb.AppendLine("        throw new NotImplementedException();");

        sb.AppendLine("    }");

        sb.AppendLine("}");

        return sb.ToString();
    }
}
