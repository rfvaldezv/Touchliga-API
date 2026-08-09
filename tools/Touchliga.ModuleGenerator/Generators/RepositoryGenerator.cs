using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Roslyn;
using Touchliga.ModuleGenerator.Services;

namespace Touchliga.ModuleGenerator.Generators;

/// <summary>
/// Genera la implementación del repositorio.
/// </summary>
public sealed class RepositoryGenerator : IGenerator
{
    private readonly FileService _fileService = new();

    private readonly SolutionLocator _locator = new();

    public async Task GenerateAsync(ModuleDefinition module)
    {
        var builder = new RepositorySourceBuilder();

        var source = builder.Build(module);

        var destination = Path.Combine(
            _locator.Persistence,
            "Repositories",
            $"{module.Entity}Repository.cs");

        _fileService.WriteFile(
            destination,
            source);

        Console.WriteLine(
            $"✓ {module.Entity}Repository.cs");

        await Task.CompletedTask;
    }
}
