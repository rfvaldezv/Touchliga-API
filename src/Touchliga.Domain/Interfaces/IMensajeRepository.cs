using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

public interface IMensajeRepository
{
    Task AgregarAsync(Mensaje mensaje, CancellationToken cancellationToken = default);

    Task<Mensaje?> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default);

    void Actualizar(Mensaje mensaje);

    void Eliminar(Mensaje mensaje);

    Task<IReadOnlyList<Mensaje>> ObtenerConversacionAsync(
        long usuarioId1,
        long usuarioId2,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Últimos mensajes con cada contacto distinto que le ha
    /// escrito o al que le ha escrito el usuario — para armar la
    /// bandeja de entrada.
    /// </summary>
    Task<IReadOnlyList<Mensaje>> ObtenerUltimosPorContactoAsync(
        long usuarioId,
        CancellationToken cancellationToken = default);

    Task<int> ContarNoLeidosAsync(long usuarioId, CancellationToken cancellationToken = default);

    Task MarcarConversacionLeidaAsync(
        long usuarioId,
        long otroUsuarioId,
        CancellationToken cancellationToken = default);
}
