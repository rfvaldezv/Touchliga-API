using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Generators.Layers;

namespace Touchliga.ModuleGenerator.Generators;

public sealed class CatalogGenerator
{
    private readonly IReadOnlyList<ILayerGenerator> _layers =
    [
        new DomainGenerator(),
        new PersistenceGenerator(),
        new ApplicationGenerator(),
        new ApiGenerator()
    ];

    public async Task GenerateAsync(ModuleDefinition module)
    {
        Console.WriteLine();
        Console.WriteLine($"Generando módulo {module.Entity}");
        Console.WriteLine();

        foreach (var layer in _layers)
        {
            Console.WriteLine(
                $" -> {layer.Name}");

            await layer.GenerateAsync(module);
        }

        Console.WriteLine();
        Console.WriteLine("✔ Módulo generado correctamente.");
    }
}
