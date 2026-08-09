namespace Touchliga.ModuleGenerator.Core;

/// <summary>
/// Define un generador de módulos.
/// </summary>
public interface IModuleGenerator
{
    /// <summary>
    /// Orden de ejecución.
    /// </summary>
    int Order { get; }

    /// <summary>
    /// Genera los artefactos del módulo.
    /// </summary>
    Task GenerateAsync(ModuleDefinition module);
}
