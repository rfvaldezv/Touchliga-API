using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Roslyn;
using Touchliga.ModuleGenerator.Services;

namespace Touchliga.ModuleGenerator.Generators;

/// <summary>
/// Genera los DTOs de Application.
/// </summary>
public sealed class DtoGenerator : IGenerator
{
    private readonly FileService _fileService = new();

    private readonly SolutionLocator _locator = new();

    public async Task GenerateAsync(ModuleDefinition module)
    {
        var builder = new DtoSourceBuilder();

        var source = builder.Build(module);

        var destination = Path.Combine(
            _locator.Application,
            "DTOs",
            $"{module.Entity}Dto.cs");

        _fileService.WriteFile(destination, source);

        Console.WriteLine(
            $"✓ {module.Entity}Dto.cs");

        await Task.CompletedTask;
    }
}
