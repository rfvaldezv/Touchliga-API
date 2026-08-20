using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;
using Touchliga.Domain.ValueObjects;

namespace Touchliga.Domain.Entities;

/// <summary>
/// Un segundo correo+contraseña que puede iniciar sesión COMO el
/// mismo participante (mismos pronósticos, mismos puntos, mismo Id
/// real) -- pensado para parejas/familiares que juegan juntos y
/// quieren su propio acceso, sin ser 2 cuentas separadas.
/// </summary>
public sealed class CredencialAlterna : AggregateRoot
{
    private CredencialAlterna()
    {
    }

    public long UsuarioId { get; private set; }

    public Email Correo { get; private set; } = null!;

    public string PasswordHash { get; private set; } = string.Empty;

    public static CredencialAlterna Crear(
        long usuarioId,
        Email correo,
        string passwordHash,
        long usuarioAlta)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("La contraseña es obligatoria.");

        return new CredencialAlterna
        {
            UsuarioId = usuarioId,
            Correo = correo,
            PasswordHash = passwordHash,
            UsuarioAltaId = usuarioAlta,
            FechaAlta = DateTime.UtcNow,
            Activo = true
        };
    }

    public void CambiarPassword(string nuevoHash, long usuarioId)
    {
        if (string.IsNullOrWhiteSpace(nuevoHash))
            throw new DomainException("Contraseña inválida.");

        PasswordHash = nuevoHash;
        MarcarModificado(usuarioId);
    }

    /// <summary>Actualiza correo y contraseña juntos -- usado cuando
    /// el admin reemplaza una credencial alterna ya existente.</summary>
    public void Actualizar(Email correo, string passwordHash, long usuarioId)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("La contraseña es obligatoria.");

        Correo = correo;
        PasswordHash = passwordHash;
        MarcarModificado(usuarioId);
    }
}
