using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

public interface IPartidoRepository : IGenericRepository<Partido>
{
    Task<IReadOnlyList<Partido>> ObtenerPorJornadaAsync(
        long jornadaId,
        CancellationToken cancellationToken = default);
}
