using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Engine;
using Touchliga.ModuleGenerator.Services;

namespace Touchliga.ModuleGenerator.Generators;

public sealed class InterfaceGenerator : IGenerator
{
    private readonly TemplateService _templateService = new();

    private readonly TemplateEngine _templateEngine = new();

    private readonly FileService _fileService = new();

    private readonly SolutionLocator _locator = new();

    public async Task GenerateAsync(ModuleDefinition module)
    {
        var template = _templateService.Load(
            Path.Combine(
                AppContext.BaseDirectory,
                "Templates",
                "Catalog",
                "Interface.tpl"));

        var source = _templateEngine.Render(
            template,
            new Dictionary<string, string>
            {
                ["Entity"] = module.Entity
            });

        var file = Path.Combine(
            _locator.Domain,
            "Interfaces",
            $"I{module.Entity}Repository.cs");

        _fileService.WriteFile(file, source);

        Console.WriteLine(
            $"✓ Interface .... I{module.Entity}Repository.cs");

        await Task.CompletedTask;
    }
}
