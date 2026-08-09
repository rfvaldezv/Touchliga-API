namespace Touchliga.ModuleGenerator.Core;

public sealed class FieldDefinition
{
    public string Name { get; set; } = string.Empty;

    public FieldType Type { get; set; }

    public bool Nullable { get; set; }

    public int? Length { get; set; }

    public bool Required { get; set; }

    public bool IsKey { get; set; }

    public bool IsUnique { get; set; }

    public string? DisplayName { get; set; }

    public object? DefaultValue { get; set; }

    public bool Searchable { get; set; }

    public bool Sortable { get; set; }

    public bool VisibleInGrid { get; set; } = true;

    public bool VisibleInForm { get; set; } = true;

    public bool Audit { get; set; }

    public bool SoftDelete { get; set; }
}
