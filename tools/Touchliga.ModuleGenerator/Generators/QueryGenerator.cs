using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Roslyn;
using Touchliga.ModuleGenerator.Services;

namespace Touchliga.ModuleGenerator.Generators;

public sealed class QueryGenerator : IGenerator
{
    private readonly FileService _fileService = new();

    private readonly SolutionLocator _locator = new();

    public async Task GenerateAsync(ModuleDefinition module)
    {
        var builder = new QuerySourceBuilder();

        Write(
            module,
            builder.BuildSingle(module),
            "Get",
            $"Get{module.Entity}Query.cs");

        Write(
            module,
            builder.BuildCollection(module),
            "GetAll",
            $"Get{module.EntityPlural}Query.cs");

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
            "Queries",
            module.Entity,
            folder,
            fileName);

        _fileService.WriteFile(destination, source);

        Console.WriteLine($"✓ {fileName}");
    }
}
