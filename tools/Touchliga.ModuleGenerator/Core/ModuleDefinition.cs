namespace Touchliga.ModuleGenerator.Core;

/// <summary>
/// Define un módulo que será generado por el FMG.
/// </summary>
public sealed class ModuleDefinition
{
    /// <summary>
    /// Nombre de la entidad.
    /// </summary>
    public string Entity { get; set; } = string.Empty;

    /// <summary>
    /// Nombre plural de la entidad.
    /// </summary>
    public string EntityPlural
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Entity))
                return Entity;

            // Excepciones
            return Entity switch
            {
                "Pais" => "Paises",
                _ => GetRegularPlural(Entity)
            };
        }
    }

    private static string GetRegularPlural(string entity)
    {
        if (entity.EndsWith("z", StringComparison.OrdinalIgnoreCase))
            return entity[..^1] + "ces";

        if (entity.EndsWith("s", StringComparison.OrdinalIgnoreCase))
            return entity;

        return entity + "s";
    }
   
    /// <summary>
    /// Esquema de la base de datos.
    /// </summary>
    public string Schema { get; set; } = "dbo";

    /// <summary>
    /// Nombre de la tabla.
    /// </summary>
    public string? Table { get; set; }

    /// <summary>
    /// Nombre para mostrar.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Clase base que heredará la entidad.
    /// </summary>
    public string BaseClass { get; set; } = "BaseCatalogEntity";

    /// <summary>
    /// Tipo de módulo.
    /// </summary>
    public ModuleKind Kind { get; set; } = ModuleKind.Catalog;

    /// <summary>
    /// Nombre del controlador.
    /// </summary>
    public string ControllerName =>
        $"{EntityPlural}Controller";

    /// <summary>
    /// Ruta base de la API.
    /// </summary>
    public string Route =>
        $"api/{EntityPlural.ToLowerInvariant()}";

    /// <summary>
    /// Namespace de Features.
    /// </summary>
    public string FeatureNamespace =>
        $"Touchliga.Application.Features.{EntityPlural}";

    /// <summary>
    /// Genera CRUD.
    /// </summary>
    public bool GenerateCrud { get; set; } = true;

    /// <summary>
    /// Genera API.
    /// </summary>
    public bool GenerateApi { get; set; } = true;

    /// <summary>
    /// Genera Flutter.
    /// </summary>
    public bool GenerateFlutter { get; set; } = true;

    /// <summary>
    /// Campos del módulo.
    /// </summary>
    public IList<FieldDefinition> Fields { get; } = new List<FieldDefinition>();

    /// <summary>
    /// Relaciones del módulo.
    /// </summary>
    public IList<RelationDefinition> Relations { get; } = new List<RelationDefinition>();

    public string EntityNamespace =>
        Kind switch
        {
            ModuleKind.Aggregate => "Touchliga.Domain.Aggregates",
            ModuleKind.View => "Touchliga.Domain.Views",
            ModuleKind.ReadModel => "Touchliga.Domain.ReadModels",
            _ => "Touchliga.Domain.Entities"
        };
}
