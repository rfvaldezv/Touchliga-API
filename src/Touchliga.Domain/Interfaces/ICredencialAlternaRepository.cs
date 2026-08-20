using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

public interface ICredencialAlternaRepository
{
    Task<CredencialAlterna?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default);

    Task<CredencialAlterna?> ObtenerPorUsuarioIdAsync(long usuarioId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CredencialAlterna>> ObtenerTodasAsync(CancellationToken cancellationToken = default);

    /// <summary>El nombre de pila de quien está vinculado a este
    /// participante (ej. "Ximena"), o null si no tiene a nadie
    /// vinculado -- para mostrar "Pedro y Ximena" en vez de solo
    /// "Pedro" en pantallas donde se ve el nombre.</summary>
    Task<string?> ObtenerNombreVinculadoAsync(long usuarioId, CancellationToken cancellationToken = default);

    Task AgregarAsync(CredencialAlterna entidad, CancellationToken cancellationToken = default);

    void Eliminar(CredencialAlterna entidad);
}
