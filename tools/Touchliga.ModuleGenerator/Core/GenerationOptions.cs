namespace Touchliga.ModuleGenerator.Core;

/// <summary>
/// Opciones para controlar qué artefactos generar.
/// </summary>
public sealed class GenerationOptions
{
    public IList<ModuleArtifact> Artifacts { get; }
        = new List<ModuleArtifact>();
}
