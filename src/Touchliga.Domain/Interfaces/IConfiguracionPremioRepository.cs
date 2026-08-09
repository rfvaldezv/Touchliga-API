using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

public interface IConfiguracionPremioRepository
{
    Task AgregarAsync(ConfiguracionPremio premio, CancellationToken cancellationToken = default);

    Task<ConfiguracionPremio?> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ConfiguracionPremio>> ObtenerPorTemporadaYAmbitoAsync(
        long temporadaId,
        string ambito,
        CancellationToken cancellationToken = default);

    void Actualizar(ConfiguracionPremio premio);

    void Eliminar(ConfiguracionPremio premio);
}
