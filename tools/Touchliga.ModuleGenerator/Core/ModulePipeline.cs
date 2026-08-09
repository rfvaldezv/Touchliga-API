namespace Touchliga.ModuleGenerator.Core;

public sealed class ModulePipeline
{
    private readonly IEnumerable<IModuleGenerator> _generators;

    public ModulePipeline(IEnumerable<IModuleGenerator> generators)
    {
        _generators = generators
            .OrderBy(g => g.Order);
    }

    public async Task ExecuteAsync(ModuleDefinition module)
    {
        foreach (var generator in _generators)
        {
            await generator.GenerateAsync(module);
        }
    }
}
