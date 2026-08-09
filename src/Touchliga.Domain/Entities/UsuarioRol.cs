using Touchliga.Domain.Common;

namespace Touchliga.Domain.Entities;

public sealed class UsuarioRol : BaseEntity
{
    private UsuarioRol()
    {
    }

    public long UsuarioId { get; private set; }

    public long RolId { get; private set; }

    public Usuario Usuario { get; private set; } = null!;

    public Rol Rol { get; private set; } = null!;

    public static UsuarioRol Crear(
        long usuarioId,
        long rolId,
        long usuarioAlta)
    {
        return new UsuarioRol
        {
            UsuarioId = usuarioId,
            RolId = rolId,
            UsuarioAltaId = usuarioAlta,
            FechaAlta = DateTime.UtcNow,
            Activo = true
        };
    }
}
