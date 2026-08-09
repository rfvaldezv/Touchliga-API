using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Roslyn;
using Touchliga.ModuleGenerator.Services;

namespace Touchliga.ModuleGenerator.Generators;

/// <summary>
/// Genera los Query Handlers.
/// </summary>
public sealed class QueryHandlerGenerator : IGenerator
{
    private readonly FileService _fileService = new();

    private readonly SolutionLocator _locator = new();

    public async Task GenerateAsync(ModuleDefinition module)
    {
        var builder = new QueryHandlerSourceBuilder();

        Write(
            module,
            builder.BuildSingle(module),
            "Get",
            $"Get{module.Entity}QueryHandler.cs");

        Write(
            module,
            builder.BuildCollection(module),
            "GetAll",
            $"Get{module.EntityPlural}QueryHandler.cs");

        await Task.CompletedTask;
    }

    private void Write(
        ModuleDefinition module,
        string source,
        string folder,
        string fileName)
    {
        var destination = Path.Combine(
            _locator.Application,
            "Handlers",
            module.Entity,
            folder,
            fileName);

        _fileService.WriteFile(destination, source);

        Console.WriteLine($"✓ {fileName}");
    }
}
