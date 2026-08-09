using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Generators.Layers;

/// <summary>
/// Genera la capa Application.
/// </summary>
public sealed class ApplicationGenerator : ILayerGenerator
{
    private readonly CommandGenerator _commandGenerator = new();

    private readonly QueryGenerator _queryGenerator = new();

    private readonly DtoGenerator _dtoGenerator = new();

    private readonly CommandHandlerGenerator _commandHandlerGenerator = new();

    private readonly QueryHandlerGenerator _queryHandlerGenerator = new();

    public string Name => "Application";

    public async Task GenerateAsync(ModuleDefinition module)
    {
        await _commandGenerator.GenerateAsync(module);

        await _queryGenerator.GenerateAsync(module);

        await _dtoGenerator.GenerateAsync(module);

        await _commandHandlerGenerator.GenerateAsync(module);

        await _queryHandlerGenerator.GenerateAsync(module);
    }
}
