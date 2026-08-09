using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities;

/// <summary>
/// Token de un dispositivo (celular) para poder mandarle
/// notificaciones push vía Firebase Cloud Messaging. Un usuario
/// puede tener varios (si usa la app en más de un celular).
/// </summary>
public sealed class PushToken : AggregateRoot
{
    private PushToken()
    {
    }

    public long UsuarioId { get; private set; }

    public string Token { get; private set; } = string.Empty;

    public string Plataforma { get; private set; } = string.Empty;

    public static PushToken Registrar(long usuarioId, string token, string plataforma)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new DomainException("El token es obligatorio.");

        return new PushToken
        {
            UsuarioId = usuarioId,
            Token = token,
            Plataforma = string.IsNullOrWhiteSpace(plataforma) ? "android" : plataforma,
            UsuarioAltaId = usuarioId,
            FechaAlta = DateTime.UtcNow,
            Activo = true
        };
    }
}
