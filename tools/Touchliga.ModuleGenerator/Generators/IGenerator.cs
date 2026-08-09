using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Generators;

public interface IGenerator
{
    Task GenerateAsync(ModuleDefinition module);
}
