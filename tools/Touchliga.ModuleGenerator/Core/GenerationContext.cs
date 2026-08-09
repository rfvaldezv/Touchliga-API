using Touchliga.ModuleGenerator.Services;

namespace Touchliga.ModuleGenerator.Core;

/// <summary>
/// Contexto completo de una generación.
/// Todos los generadores reciben este objeto.
/// </summary>
public sealed class GenerationContext
{
    /// <summary>
    /// Módulo que será generado.
    /// </summary>
    public required ModuleDefinition Module { get; init; }

    /// <summary>
    /// Localizador de la solución.
    /// </summary>
    public required SolutionLocator Solution { get; init; }

    /// <summary>
    /// Fecha de generación.
    /// </summary>
    public DateTime GeneratedAt { get; init; }
        = DateTime.UtcNow;

    /// <summary>
    /// Versión del FMG.
    /// </summary>
    public string Version { get; init; }
        = "1.0.0";

    /// <summary>
    /// Sobrescribir archivos existentes.
    /// </summary>
    public bool OverwriteFiles { get; init; }

    /// <summary>
    /// Solo simular la generación.
    /// </summary>
    public bool DryRun { get; init; }
}
