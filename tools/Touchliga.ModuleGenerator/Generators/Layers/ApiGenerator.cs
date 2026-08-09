using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Generators.Layers;

/// <summary>
/// Genera la capa API.
/// </summary>
public sealed class ApiGenerator : ILayerGenerator
{
    private readonly ControllerGenerator _controllerGenerator = new();

    public string Name => "API";

    public async Task GenerateAsync(ModuleDefinition module)
    {
        await _controllerGenerator.GenerateAsync(module);
    }
}
