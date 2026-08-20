using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

/// <summary>Solo existe (o debe existir) una fila -- se trata como
/// un ajuste único, no un catálogo de varias filas.</summary>
public interface IConfiguracionSmtpRepository
{
    Task<ConfiguracionSmtp?> ObtenerAsync(CancellationToken cancellationToken = default);

    Task AgregarAsync(ConfiguracionSmtp entidad, CancellationToken cancellationToken = default);

    void Actualizar(ConfiguracionSmtp entidad);
}
