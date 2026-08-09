namespace Touchliga.ModuleGenerator.Core;

/// <summary>
/// Representa un archivo generado por el FMG.
/// </summary>
public sealed class GeneratedFile
{
    /// <summary>
    /// Ruta absoluta del archivo.
    /// </summary>
    public string FileName { get; init; } = string.Empty;

    /// <summary>
    /// Contenido del archivo.
    /// </summary>
    public string Content { get; init; } = string.Empty;

    /// <summary>
    /// Indica si el archivo ya existía.
    /// </summary>
    public bool Exists { get; set; }

    /// <summary>
    /// Indica si fue sobrescrito.
    /// </summary>
    public bool Overwritten { get; set; }
}
