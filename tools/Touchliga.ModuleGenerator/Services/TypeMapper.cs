using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Services;

public sealed class TypeMapper
{
    public string GetCSharpType(FieldType type)
    {
        return type switch
        {
            FieldType.String => "string",
            FieldType.Int => "int",
            FieldType.Long => "long",
            FieldType.Decimal => "decimal",
            FieldType.Double => "double",
            FieldType.Float => "float",
            FieldType.Bool => "bool",
            FieldType.Date => "DateOnly",
            FieldType.DateTime => "DateTime",
            FieldType.Time => "TimeOnly",
            FieldType.Guid => "Guid",
            FieldType.ByteArray => "byte[]",
            _ => "string"
        };
    }
}
