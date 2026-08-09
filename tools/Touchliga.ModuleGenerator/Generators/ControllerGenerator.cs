using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Roslyn;
using Touchliga.ModuleGenerator.Services;

namespace Touchliga.ModuleGenerator.Generators;

public sealed class ControllerGenerator : IGenerator
{
    private readonly FileService _fileService = new();

    private readonly SolutionLocator _locator = new();

    public async Task GenerateAsync(ModuleDefinition module)
    {
        var source = ControllerSourceBuilder.Build(module);

        var destination = Path.Combine(
            _locator.Api,
            "Controllers",
            $"{module.EntityPlural}Controller.cs");

        _fileService.WriteFile(
            destination,
            source);

        Console.WriteLine(
            $"✓ {module.EntityPlural}Controller.cs");

        await Task.CompletedTask;
    }
}
