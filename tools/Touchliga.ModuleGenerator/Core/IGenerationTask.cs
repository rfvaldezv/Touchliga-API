namespace Touchliga.ModuleGenerator.Core;

/// <summary>
/// Representa una tarea dentro de una etapa del pipeline.
/// </summary>
public interface IGenerationTask
{
    Task ExecuteAsync(
        GenerationContext context,
        GenerationResult result);
}
