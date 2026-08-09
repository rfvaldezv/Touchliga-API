using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Roslyn;
using Touchliga.ModuleGenerator.Services;

namespace Touchliga.ModuleGenerator.Generators.Persistence;

/// <summary>
/// Genera la interfaz del repositorio del dominio.
/// </summary>
public sealed class RepositoryInterfaceGenerator : IGenerator
{
    private readonly FileService _fileService = new();
    private readonly SolutionLocator _locator = new();

    public string Name => "Repository Interface";

    public async Task GenerateAsync(ModuleDefinition module)
    {
        var builder = new RepositoryInterfaceSourceBuilder();

        var source = builder.Build(module);

        var destinationFile = Path.Combine(
            _locator.Domain,
            "Repositories",
            $"I{module.Entity}Repository.cs");

        _fileService.WriteFile(destinationFile, source);

        Console.WriteLine(
            $"✓ Repository Interface .... {destinationFile}");

        await Task.CompletedTask;
    }
}
