using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Roslyn;

public sealed class RepositoryInterfaceSourceBuilder
{
    public string Build(ModuleDefinition module)
    {
        return
$@"namespace Touchliga.Domain.Interfaces;

/// <summary>
/// Repositorio de {module.Entity}.
/// </summary>
public interface I{module.Entity}Repository
{{
}}
";
    }
}
