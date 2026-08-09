namespace Touchliga.ModuleGenerator.Core;

/// <summary>
/// Describe una relación entre entidades.
/// </summary>
public sealed class RelationDefinition
{
    /// <summary>
    /// Entidad relacionada.
    /// </summary>
    public string Entity { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de relación.
    /// </summary>
    public RelationType RelationType { get; set; }

    /// <summary>
    /// Campo FK.
    /// </summary>
    public string ForeignKey { get; set; } = string.Empty;
}
