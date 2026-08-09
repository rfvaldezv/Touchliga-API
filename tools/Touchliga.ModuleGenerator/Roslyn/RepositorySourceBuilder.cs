using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Roslyn;

public sealed class RepositorySourceBuilder
{
    public string Build(ModuleDefinition module)
    {
        return
$@"using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;

namespace Touchliga.Persistence.Repositories;

/// <summary>
/// Implementación del repositorio de {module.Entity}.
/// </summary>
public sealed class {module.Entity}Repository
    : I{module.Entity}Repository
{{
}}
";
    }
}
