using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Roslyn;
using Touchliga.ModuleGenerator.Services;

namespace Touchliga.ModuleGenerator.Generators;

/// <summary>
/// Genera los handlers de Commands.
/// </summary>
public sealed class CommandHandlerGenerator : IGenerator
{
    private readonly FileService _fileService = new();

    private readonly SolutionLocator _locator = new();

    public async Task GenerateAsync(ModuleDefinition module)
    {
        var builder = new CommandHandlerSourceBuilder();

        Write(
            module,
            builder.BuildCreate(module),
            "Create",
            $"Create{module.Entity}CommandHandler.cs");

        Write(
            module,
            builder.BuildUpdate(module),
            "Update",
            $"Update{module.Entity}CommandHandler.cs");

        Write(
            module,
            builder.BuildDelete(module),
            "Delete",
            $"Delete{module.Entity}CommandHandler.cs");

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
