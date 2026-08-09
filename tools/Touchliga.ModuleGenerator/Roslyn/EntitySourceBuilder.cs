using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Roslyn;

public sealed class EntitySourceBuilder
{
    public string Build(ModuleDefinition module)
    {
        var inheritedFields = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase)
        {
            "Codigo",
            "Nombre",
            "Descripcion",
            "Activo"
        };

        var entityFields = module.Fields
            .Where(f => !inheritedFields.Contains(f.Name));

        var syntax =
            new EntityBuilder(
                module.EntityNamespace,
                module.Entity)            .Inherits(module.BaseClass)
            .WithPrivateConstructor()
            .AddProperties(entityFields)
            .AddRelations(module.Relations)
            .Build();

        return syntax.ToFullString();
    }
}
