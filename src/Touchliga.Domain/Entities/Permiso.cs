using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities;

public sealed class Permiso : AggregateRoot
{
    private Permiso()
    {
    }

    public string Codigo { get; private set; } = string.Empty;

    public string Nombre { get; private set; } = string.Empty;

    public string? Descripcion { get; private set; }

    public static Permiso Crear(
        string codigo,
        string nombre,
        string? descripcion,
        long usuarioAlta)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            throw new DomainException("Código inválido.");

        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("Nombre inválido.");

        return new Permiso
        {
            Codigo = codigo.Trim().ToUpper(),
            Nombre = nombre.Trim(),
            Descripcion = descripcion,
            UsuarioAltaId = usuarioAlta,
            FechaAlta = DateTime.UtcNow
        };
    }
}
