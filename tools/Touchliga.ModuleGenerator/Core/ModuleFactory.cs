namespace Touchliga.ModuleGenerator.Core;

/// <summary>
/// Fabrica de definiciones de módulos.
/// </summary>
public static class ModuleFactory
{
    public static ModuleDefinition CreateCatalog(string entity)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(entity);

        entity = entity.Trim();

        return entity switch
        {
            "Liga" => CreateLiga(),

            _ => CreateDefault(entity)
        };
    }

    private static ModuleDefinition CreateDefault(string entity)
    {
        return new ModuleDefinition
        {
            Entity = entity,
            DisplayName = entity,
            Table = entity,
            Schema = "cat",
            GenerateCrud = true,
            GenerateApi = true,
            GenerateFlutter = true
        };
    }

    private static ModuleDefinition CreateLiga()
    {
        var module = CreateDefault("Liga");

        module.Table = "Ligas";

        module.Fields.Add(new FieldDefinition
        {
            Name = "Codigo",
            Type = FieldType.String,
            Length = 20,
            Required = true,
            Searchable = true
        });

        module.Fields.Add(new FieldDefinition
        {
            Name = "Nombre",
            Type = FieldType.String,
            Length = 150,
            Required = true,
            Searchable = true
        });

        module.Fields.Add(new FieldDefinition
        {
            Name = "Descripcion",
            Type = FieldType.String,
            Length = 500
        });

        module.Fields.Add(new FieldDefinition
        {
            Name = "Activo",
            Type = FieldType.Bool
        });

        return module;
    }
}
