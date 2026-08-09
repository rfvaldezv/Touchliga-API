using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Roslyn;
using Touchliga.ModuleGenerator.Services;

namespace Touchliga.ModuleGenerator.Generators;

public sealed class EntityGenerator : IModuleGenerator{

    public int Order => 100;
    
    private readonly FileService _fileService = new();

    private readonly ISolutionLocator _locator = new SolutionLocator();

    private readonly EntitySourceBuilder _builder = new();

    public async Task GenerateAsync(ModuleDefinition module)
    {
        ArgumentNullException.ThrowIfNull(module);

        var source = _builder.Build(module);

        var destinationFile = Path.Combine(
            _locator.Domain,
            "Entities",
            $"{module.Entity}.cs");

        await _fileService.WriteFileAsync(
            destinationFile,
            source);

        Console.WriteLine(
            $"✓ Entity ........ {destinationFile}");
    }
}
