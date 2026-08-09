using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Generators.Layers;

/// <summary>
/// Genera la capa Domain.
/// </summary>
public sealed class DomainGenerator : ILayerGenerator
{
    private readonly EntityGenerator _entityGenerator = new();

    private readonly RepositoryInterfaceGenerator _repositoryGenerator = new();

    public string Name => "Domain";

    public async Task GenerateAsync(ModuleDefinition module)
    {
        await _entityGenerator.GenerateAsync(module);

        await _repositoryGenerator.GenerateAsync(module);
    }
}
