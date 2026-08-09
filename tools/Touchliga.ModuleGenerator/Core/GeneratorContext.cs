using Touchliga.ModuleGenerator.Services;

namespace Touchliga.ModuleGenerator.Core;

/// <summary>
/// Contexto compartido durante la generación.
/// </summary>
public sealed class GeneratorContext
{
    public required ModuleDefinition Module { get; init; }

    public required ISolutionLocator Locator { get; init; }

    public GenerationResult Result { get; } = new();
}
