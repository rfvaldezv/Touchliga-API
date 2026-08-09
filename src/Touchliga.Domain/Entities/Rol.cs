using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities;

public sealed class Rol : AggregateRoot
{
    private readonly List<Permiso> _permisos = new();

    private Rol()
    {
    }

    public string Nombre { get; private set; } = string.Empty;

    public string? Descripcion { get; private set; }

    public IReadOnlyCollection<Permiso> Permisos => _permisos.AsReadOnly();

    public static Rol Crear(
        string nombre,
        string? descripcion,
        long usuarioAlta)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("El nombre del rol es obligatorio.");

        return new Rol
        {
            Nombre = nombre.Trim(),
            Descripcion = descripcion,
            UsuarioAltaId = usuarioAlta,
            FechaAlta = DateTime.UtcNow
        };
    }

    public void AgregarPermiso(Permiso permiso)
    {
        if (_permisos.Any(x => x.Id == permiso.Id))
            return;

        _permisos.Add(permiso);
    }

    public void RemoverPermiso(long permisoId)
    {
        var permiso = _permisos.FirstOrDefault(x => x.Id == permisoId);

        if (permiso != null)
            _permisos.Remove(permiso);
    }
}
