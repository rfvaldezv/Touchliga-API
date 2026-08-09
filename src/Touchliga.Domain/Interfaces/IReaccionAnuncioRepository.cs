using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

public interface IReaccionAnuncioRepository
{
    Task AgregarAsync(ReaccionAnuncio reaccion, CancellationToken cancellationToken = default);

    Task<ReaccionAnuncio?> ObtenerAsync(
        long anuncioId, long usuarioId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReaccionAnuncio>> ObtenerPorAnunciosAsync(
        IReadOnlyList<long> anuncioIds, CancellationToken cancellationToken = default);

    void Actualizar(ReaccionAnuncio reaccion);

    void Eliminar(ReaccionAnuncio reaccion);
}
