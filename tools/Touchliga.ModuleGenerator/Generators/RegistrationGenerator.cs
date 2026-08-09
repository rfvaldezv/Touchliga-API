using Touchliga.ModuleGenerator.Core;
using Touchliga.ModuleGenerator.Services;

namespace Touchliga.ModuleGenerator.Generators;

public sealed class RegistrationGenerator : IGenerator
{
    private readonly ISolutionLocator _locator = new SolutionLocator();

    private readonly IFileService _fileService = new FileService();

    public async Task GenerateAsync(ModuleDefinition module)
    {
        var file = Path.Combine(
            _locator.Persistence,
            "DependencyInjection.cs");

        var source = await File.ReadAllTextAsync(file);

        var line =
            $"        services.AddScoped<I{module.Entity}Repository, {module.Entity}Repository>();";

        if (source.Contains(line))
        {
            Console.WriteLine($"✓ DI ............ {module.Entity} ya registrado.");
            return;
        }

        var marker =
            "        services.AddScoped<IUnitOfWork, UnitOfWork>();";

        if (!source.Contains(marker))
            throw new InvalidOperationException(
                "No fue posible localizar el punto de inserción en DependencyInjection.cs");

        source = source.Replace(
            marker,
            marker + Environment.NewLine + Environment.NewLine + line);

        await _fileService.WriteFileAsync(file, source);

        Console.WriteLine($"✓ DI ............ Registrado {module.Entity}");
    }
}
