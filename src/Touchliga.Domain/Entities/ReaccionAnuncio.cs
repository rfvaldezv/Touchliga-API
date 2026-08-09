using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities;

/// <summary>
/// Reacción rápida de un usuario a un anuncio (uno por persona por
/// anuncio, se puede cambiar de emoji o quitar).
/// </summary>
public sealed class ReaccionAnuncio : AggregateRoot
{
    private ReaccionAnuncio()
    {
    }

    public static readonly string[] EmojisPermitidos = ["👍", "🔥", "😂", "❤️", "😮"];

    public long AnuncioId { get; private set; }

    public long UsuarioId { get; private set; }

    public string Emoji { get; private set; } = string.Empty;

    public DateTime FechaReaccion { get; private set; }

    public static ReaccionAnuncio Crear(long anuncioId, long usuarioId, string emoji)
    {
        Validar(emoji);

        return new ReaccionAnuncio
        {
            AnuncioId = anuncioId,
            UsuarioId = usuarioId,
            Emoji = emoji,
            FechaReaccion = DateTime.UtcNow,
            UsuarioAltaId = usuarioId,
            FechaAlta = DateTime.UtcNow,
            Activo = true
        };
    }

    public void Cambiar(string emoji)
    {
        Validar(emoji);
        Emoji = emoji;
        FechaReaccion = DateTime.UtcNow;
    }

    private static void Validar(string emoji)
    {
        if (!EmojisPermitidos.Contains(emoji))
            throw new DomainException("Ese emoji no está permitido para reaccionar.");
    }
}
