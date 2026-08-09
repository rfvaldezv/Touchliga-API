using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

public interface IPremioOtorgadoRepository
{
    Task AgregarAsync(PremioOtorgado premio, CancellationToken cancellationToken = default);

    Task<PremioOtorgado?> ObtenerAsync(
        string ambito,
        long referenciaId,
        long usuarioId,
        CancellationToken cancellationToken = default);

    /// <summary>Todas las decisiones ya tomadas para un ámbito+referencia
    /// (una jornada específica, o el cierre final de una temporada) — para
    /// mezclarlas con las sugerencias calculadas.</summary>
    Task<IReadOnlyList<PremioOtorgado>> ObtenerPorReferenciaAsync(
        string ambito,
        long referenciaId,
        CancellationToken cancellationToken = default);

    void Actualizar(PremioOtorgado premio);
}
