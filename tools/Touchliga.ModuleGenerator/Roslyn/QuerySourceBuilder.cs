using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Roslyn;

public sealed class QuerySourceBuilder
{
    public string BuildSingle(ModuleDefinition module)
    {
        return
$@"using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.{module.Entity}.Get;

/// <summary>
/// Obtiene un {module.Entity} por Id.
/// </summary>
public sealed record Get{module.Entity}Query(
    long Id)
    : IRequest<{module.Entity}Dto>;
";
    }

    public string BuildCollection(ModuleDefinition module)
    {
        return
$@"using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.{module.Entity}.GetAll;

/// <summary>
/// Obtiene la colección de {module.EntityPlural}.
/// </summary>
public sealed record Get{module.EntityPlural}Query()
    : IRequest<IReadOnlyList<{module.Entity}Dto>>;
";
    }
}
