using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Generators;

public interface IGenerationTask
{
    string Name { get; }

    Task ExecuteAsync(ModuleDefinition module);
}
