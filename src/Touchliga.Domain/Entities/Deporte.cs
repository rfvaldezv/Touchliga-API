using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities;

public sealed class Deporte : BaseCatalogEntity
{
    private Deporte()
    {
    }

    public static Deporte Crear(
        string codigo,
        string nombre,
        string? descripcion,
        long usuarioAlta)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            throw new DomainException("El código es obligatorio.");

        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("El nombre es obligatorio.");

        var deporte = new Deporte();

        deporte.EstablecerCodigo(codigo);
        deporte.EstablecerNombre(nombre);
        deporte.EstablecerDescripcion(descripcion);

        deporte.UsuarioAltaId = usuarioAlta;
        deporte.FechaAlta = DateTime.UtcNow;
        deporte.Activo = true;

        return deporte;
    }

    public void Editar(
        string codigo,
        string nombre,
        string? descripcion,
        long usuarioId)
    {
        EstablecerCodigo(codigo);
        EstablecerNombre(nombre);
        EstablecerDescripcion(descripcion);

        MarcarModificado(usuarioId);
    }
}
