namespace Touchliga.ModuleGenerator.Core;

/// <summary>
/// Orquesta la ejecución de todas las etapas de generación.
/// </summary>
public sealed class GenerationPipeline
{
    private readonly List<IGenerationStage> _stages = [];

    public GenerationPipeline Add(IGenerationStage stage)
    {
        _stages.Add(stage);
        return this;
    }

    public async Task<GenerationResult> ExecuteAsync(
        GenerationContext context)
    {
        var result = new GenerationResult();

        foreach (var stage in _stages)
        {
            await stage.ExecuteAsync(
                context,
                result);
        }

        return result;
    }
}
