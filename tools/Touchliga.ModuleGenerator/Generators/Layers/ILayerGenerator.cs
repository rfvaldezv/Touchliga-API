using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Generators.Layers;

public interface ILayerGenerator
{
    string Name { get; }

    Task GenerateAsync(ModuleDefinition module);
}
