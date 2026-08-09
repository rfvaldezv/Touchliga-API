using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Generators.Layers;

/// <summary>
/// Genera la capa Persistence.
/// </summary>
public sealed class PersistenceGenerator : ILayerGenerator
{
    private readonly IReadOnlyList<IGenerator> _tasks =
    [
        new RepositoryGenerator(),
        new ConfigurationGenerator(),
        new DependencyInjectionGenerator(),
        new DbContextGenerator()
    ];

    public string Name => "Persistence";

    public async Task GenerateAsync(ModuleDefinition module)
    {
        foreach (var task in _tasks)
        {
            await task.GenerateAsync(module);
        }
    }
}
