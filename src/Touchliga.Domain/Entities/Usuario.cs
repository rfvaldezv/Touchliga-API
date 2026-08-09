using Touchliga.Domain.Common;
using Touchliga.Domain.Enums;
using Touchliga.Domain.Exceptions;
using Touchliga.Domain.ValueObjects;

namespace Touchliga.Domain.Entities;

public sealed class Usuario : AggregateRoot
{
    private readonly List<UsuarioRol> _roles = new();

    private Usuario()
    {
    }

    public string Nombre { get; private set; } = string.Empty;

    public string Apellidos { get; private set; } = string.Empty;

    public string Telefono { get; private set; } = string.Empty;

    public Email Correo { get; private set; } = null!;

    public string PasswordHash { get; private set; } = string.Empty;

    public bool EmailConfirmado { get; private set; }

    public long? InvitadoPorId { get; private set; }

    public long? CiudadId { get; private set; }

    public long? PaisId { get; private set; }

    public long? EstadoId { get; private set; }

    public string Sexo { get; private set; } = string.Empty;

    public EstatusParticipante Estatus { get; private set; } = EstatusParticipante.Activo;

    /// <summary>Captura posterior, vía Perfil — opcionales al dar de alta.</summary>
    public DateTime? FechaNacimiento { get; private set; }

    public long? EquipoFavoritoId { get; private set; }

    public string? Nickname { get; private set; }

    public string? FotoUrl { get; private set; }

    public IReadOnlyCollection<UsuarioRol> Roles => _roles.AsReadOnly();

    public static Usuario Crear(
        string nombre,
        Email correo,
        string passwordHash,
        long usuarioAlta)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("La contraseña es obligatoria.");

        return new Usuario
        {
            Nombre = nombre.Trim(),
            Correo = correo,
            PasswordHash = passwordHash,
            UsuarioAltaId = usuarioAlta,
            FechaAlta = DateTime.UtcNow,
            Activo = true,
            EmailConfirmado = false
        };
    }

    /// <summary>
    /// Alta de un participante nuevo, capturada por un administrador.
    /// A diferencia de Crear (usada solo para el usuario admin inicial
    /// del sistema), aquí todos los datos de contacto son obligatorios.
    /// </summary>
    public static Usuario CrearParticipante(
        string nombre,
        string apellidos,
        string telefono,
        Email correo,
        string passwordHash,
        string sexo,
        long invitadoPorId,
        long ciudadId,
        long paisId,
        long estadoId,
        long usuarioAlta)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(apellidos))
            throw new DomainException("Los apellidos son obligatorios.");

        if (string.IsNullOrWhiteSpace(telefono))
            throw new DomainException("El teléfono es obligatorio.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("La contraseña es obligatoria.");

        if (sexo != "M" && sexo != "F" && sexo != "O")
            throw new DomainException("Sexo inválido (use M, F u O).");

        return new Usuario
        {
            Nombre = nombre.Trim(),
            Apellidos = apellidos.Trim(),
            Telefono = telefono.Trim(),
            Correo = correo,
            PasswordHash = passwordHash,
            Sexo = sexo,
            InvitadoPorId = invitadoPorId,
            CiudadId = ciudadId,
            PaisId = paisId,
            EstadoId = estadoId,
            UsuarioAltaId = usuarioAlta,
            FechaAlta = DateTime.UtcNow,
            Activo = true,
            EmailConfirmado = false
        };
    }

    /// <summary>
    /// El propio usuario completa estos datos después de su primer
    /// ingreso (no son obligatorios al darlo de alta). Sirven para
    /// estadísticas: edad, equipo favorito, apodo.
    /// </summary>
    public void ActualizarPerfilExtendido(
        DateTime? fechaNacimiento,
        long? equipoFavoritoId,
        string? nickname,
        string? fotoUrl,
        long usuarioId)
    {
        FechaNacimiento = fechaNacimiento;
        EquipoFavoritoId = equipoFavoritoId;
        Nickname = string.IsNullOrWhiteSpace(nickname) ? null : nickname.Trim();
        FotoUrl = string.IsNullOrWhiteSpace(fotoUrl) ? FotoUrl : fotoUrl.Trim();

        MarcarModificado(usuarioId);
    }

    public void ConfirmarCorreo()
    {
        EmailConfirmado = true;
    }

    /// <summary>Edición desde Administración: nombre, apellidos, teléfono,
    /// correo, ciudad/país/estado — soporte a participantes que se
    /// equivocaron de correo o lo olvidan.</summary>
    public void ActualizarInfoContacto(
        string nombre,
        string apellidos,
        string telefono,
        string correo,
        long? ciudadId,
        long? paisId,
        long? estadoId,
        long usuarioId)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(apellidos))
            throw new DomainException("Los apellidos son obligatorios.");

        Nombre = nombre.Trim();
        Apellidos = apellidos.Trim();
        Telefono = telefono?.Trim() ?? string.Empty;
        Correo = Email.Create(correo);
        CiudadId = ciudadId;
        PaisId = paisId;
        EstadoId = estadoId;

        MarcarModificado(usuarioId);
    }

    /// <summary>Soporte: el admin genera una contraseña temporal nueva
    /// para un participante que la olvidó. El hash ya viene calculado
    /// (BCrypt) desde la capa de aplicación.</summary>
    public void RestablecerPassword(string nuevoHash, long usuarioId)
    {
        if (string.IsNullOrWhiteSpace(nuevoHash))
            throw new DomainException("El hash de la contraseña es obligatorio.");

        PasswordHash = nuevoHash;
        MarcarModificado(usuarioId);
    }

    /// <summary>Igual que el A/X/Z del sistema anterior — Activo (bool)
    /// queda sincronizado automáticamente según el estatus elegido.</summary>
    public void CambiarEstatus(EstatusParticipante nuevoEstatus, long usuarioId)
    {
        Estatus = nuevoEstatus;

        if (nuevoEstatus == EstatusParticipante.Activo)
        {
            if (!Activo) Activar(usuarioId);
        }
        else
        {
            if (Activo) Desactivar(usuarioId);
        }

        MarcarModificado(usuarioId);
    }

    public void CambiarNombre(
        string nombre,
        long usuarioId)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("Nombre inválido.");

        Nombre = nombre.Trim();

        MarcarModificado(usuarioId);
    }

    public void CambiarPassword(
        string nuevoHash,
        long usuarioId)
    {
        if (string.IsNullOrWhiteSpace(nuevoHash))
            throw new DomainException("Contraseña inválida.");

        PasswordHash = nuevoHash;

        MarcarModificado(usuarioId);
    }

    public void AsignarRol(
        Rol rol,
        long usuarioAlta)
    {
        if (_roles.Any(x => x.RolId == rol.Id))
            return;

        _roles.Add(
            UsuarioRol.Crear(
                Id,
                rol.Id,
                usuarioAlta));
    }

    public void QuitarRol(long rolId)
    {
        var rol = _roles.FirstOrDefault(x => x.RolId == rolId);

        if (rol != null)
            _roles.Remove(rol);
    }

    public bool TieneRol(string nombreRol)
    {
        return _roles.Any(x =>
            x.Rol.Nombre.Equals(
                nombreRol,
                StringComparison.OrdinalIgnoreCase));
    }
}
