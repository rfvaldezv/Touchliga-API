using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities;

public sealed class RefreshToken : AggregateRoot
{
    private RefreshToken()
    {
    }

    public long UsuarioId { get; private set; }

    public string Token { get; private set; } = string.Empty;

    public DateTime Expira { get; private set; }

    public bool Revocado { get; private set; }

    public DateTime? FechaRevocacion { get; private set; }

    public string? MotivoRevocacion { get; private set; }

    public static RefreshToken Crear(
        long usuarioId,
        string token,
        DateTime expira,
        long usuarioAlta)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new DomainException("Refresh Token inválido.");

        return new RefreshToken
        {
            UsuarioId = usuarioId,
            Token = token,
            Expira = expira,
            UsuarioAltaId = usuarioAlta,
            FechaAlta = DateTime.UtcNow
        };
    }

    public bool EstaVigente()
    {
        return !Revocado && Expira > DateTime.UtcNow;
    }

    public void Revocar(
        string motivo,
        long usuarioId)
    {
        Revocado = true;
        FechaRevocacion = DateTime.UtcNow;
        MotivoRevocacion = motivo;

        MarcarModificado(usuarioId);
    }
}
