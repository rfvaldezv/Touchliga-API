namespace Touchliga.ModuleGenerator.Core;

/// <summary>
/// Contrato para los generadores que producen artefactos.
/// </summary>
public interface IArtifactGenerator
{
    string Name { get; }

    Task GenerateAsync(
        ModuleDefinition module,
        GenerationResult result);
}
