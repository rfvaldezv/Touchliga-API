using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

public interface IPushTokenRepository
{
    Task<PushToken?> ObtenerPorTokenAsync(string token, CancellationToken cancellationToken = default);

    Task AgregarAsync(PushToken pushToken, CancellationToken cancellationToken = default);

    void Eliminar(PushToken pushToken);

    /// <summary>Todos los tokens activos de todos los usuarios — para "enviar a todos".</summary>
    Task<IReadOnlyList<PushToken>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PushToken>> ObtenerPorUsuarioAsync(long usuarioId, CancellationToken cancellationToken = default);
}
