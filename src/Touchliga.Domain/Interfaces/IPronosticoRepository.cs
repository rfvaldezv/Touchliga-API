using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

public interface IPronosticoRepository : IGenericRepository<Pronostico>
{
    Task<Pronostico?> ObtenerPorPartidoYUsuarioAsync(
        long partidoId,
        long usuarioId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Pronostico>> ObtenerPorPartidoAsync(
        long partidoId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Pronostico>> ObtenerPorPartidoIdsAsync(
        IReadOnlyCollection<long> partidoIds,
        CancellationToken cancellationToken = default);

    Task<int> ContarPorPartidoIdsYUsuarioAsync(
        IReadOnlyCollection<long> partidoIds,
        long usuarioId,
        CancellationToken cancellationToken = default);
}
