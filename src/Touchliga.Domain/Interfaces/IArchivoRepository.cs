using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

public interface IArchivoRepository
{
    Task AgregarAsync(Archivo archivo, CancellationToken cancellationToken = default);

    Task<Archivo?> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default);
}
