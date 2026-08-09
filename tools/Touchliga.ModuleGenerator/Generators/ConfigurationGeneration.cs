using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Roslyn;
using Touchliga.ModuleGenerator.Services;

namespace Touchliga.ModuleGenerator.Generators;

/// <summary>
/// Genera la configuración de EF Core.
/// </summary>
public sealed class ConfigurationGenerator : IGenerator
{
    private readonly FileService _fileService = new();

    private readonly SolutionLocator _locator = new();

    public async Task GenerateAsync(ModuleDefinition module)
    {
        var builder = new ConfigurationSourceBuilder();

        var source = builder.Build(module);

        var destination = Path.Combine(
            _locator.Persistence,
            "Configurations",
            $"{module.Entity}Configuration.cs");

        _fileService.WriteFile(destination, source);

        Console.WriteLine(
            $"✓ {module.Entity}Configuration.cs");

        await Task.CompletedTask;
    }
}
