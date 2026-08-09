using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Roslyn;
using Touchliga.ModuleGenerator.Services;

namespace Touchliga.ModuleGenerator.Generators;

/// <summary>
/// Genera los Commands de la capa Application.
/// </summary>
public sealed class CommandGenerator : IGenerator
{
    private readonly FileService _fileService = new();

    private readonly SolutionLocator _locator = new();

    public async Task GenerateAsync(ModuleDefinition module)
    {
        var builder = new CommandSourceBuilder();

        Write(
            module,
            builder.BuildCreate(module),
            "Create",
            $"Create{module.Entity}Command.cs");

        Write(
            module,
            builder.BuildUpdate(module),
            "Update",
            $"Update{module.Entity}Command.cs");

        Write(
            module,
            builder.BuildDelete(module),
            "Delete",
            $"Delete{module.Entity}Command.cs");

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
            "Commands",
            module.Entity,
            folder,
            fileName);

        _fileService.WriteFile(
            destination,
            source);

        Console.WriteLine(
            $"✓ {fileName}");
    }
}
