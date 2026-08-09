using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Roslyn;

/// <summary>
/// Contrato para todos los generadores de código fuente.
/// </summary>
public interface ISourceBuilder
{
    string Build(ModuleDefinition module);
}
