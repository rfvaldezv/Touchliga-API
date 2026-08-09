using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Roslyn;
using Touchliga.ModuleGenerator.Services;

namespace Touchliga.ModuleGenerator.Generators.Persistence;

public sealed class ConfigurationGenerator : IGenerator
{
    private readonly FileService _files = new();
    private readonly SolutionLocator _locator = new();

    public string Name => "Configuration";

    public async Task GenerateAsync(ModuleDefinition module)
    {
        var builder = new ConfigurationSourceBuilder();

        var source = builder.Build(module);

        var destination = Path.Combine(
            _locator.Persistence,
            "Configurations",
            $"{module.Entity}Configuration.cs");

        _files.WriteFile(destination, source);

        Console.WriteLine(
            $"✓ Configuration .... {destination}");

        await Task.CompletedTask;
    }
}
