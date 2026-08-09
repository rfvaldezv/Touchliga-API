namespace Touchliga.Domain.Common;

public abstract class BaseCatalogEntity : AggregateRoot
{
    public string Codigo { get; protected set; } = string.Empty;

    public string Nombre { get; protected set; } = string.Empty;

    public string? Descripcion { get; protected set; }

    protected void EstablecerCodigo(string codigo)
    {
        Codigo = codigo.Trim().ToUpperInvariant();
    }

    protected void EstablecerNombre(string nombre)
    {
        Nombre = nombre.Trim();
    }

    protected void EstablecerDescripcion(string? descripcion)
    {
        Descripcion = descripcion?.Trim();
    }

    protected void Actualizar(
        string nombre,
        string? descripcion,
        long usuarioId)
    {
        EstablecerNombre(nombre);
        EstablecerDescripcion(descripcion);
        MarcarModificado(usuarioId);
    }
}
