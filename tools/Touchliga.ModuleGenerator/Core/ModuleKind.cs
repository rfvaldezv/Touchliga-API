namespace Touchliga.ModuleGenerator.Core;

/// <summary>
/// Tipo de módulo que genera el FMG.
/// </summary>
public enum ModuleKind
{
    /// <summary>
    /// Catálogo.
    /// </summary>
    Catalog,

    /// <summary>
    /// Entidad de negocio.
    /// </summary>
    Entity,

    /// <summary>
    /// Agregado DDD.
    /// </summary>
    Aggregate,

    /// <summary>
    /// Vista de solo lectura.
    /// </summary>
    View,

    /// <summary>
    /// ReadModel CQRS.
    /// </summary>
    ReadModel
}
