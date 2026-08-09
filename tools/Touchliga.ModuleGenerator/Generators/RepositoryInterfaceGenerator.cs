using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Roslyn;
using Touchliga.ModuleGenerator.Services;

namespace Touchliga.ModuleGenerator.Generators;

public sealed class RepositoryInterfaceGenerator : IGenerator
{
    private readonly FileService _fileService = new();
    private readonly SolutionLocator _locator = new();

    public string Name => "Repository Interface";

    public async Task GenerateAsync(ModuleDefinition module)
    {
        var builder = new RepositoryInterfaceSourceBuilder();

        var source = builder.Build(module);

        var destination = Path.Combine(
            _locator.Domain,
            "Interfaces",
            $"I{module.Entity}Repository.cs");

        _fileService.WriteFile(destination, source);

        Console.WriteLine(
            $"✓ Repository Interface ..... {destination}");

        await Task.CompletedTask;
    }
}
