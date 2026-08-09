namespace Touchliga.ModuleGenerator.Core;

/// <summary>
/// Representa una etapa del pipeline de generación.
/// </summary>
public interface IGenerationStage
{
    Task ExecuteAsync(
        GenerationContext context,
        GenerationResult result);
}
