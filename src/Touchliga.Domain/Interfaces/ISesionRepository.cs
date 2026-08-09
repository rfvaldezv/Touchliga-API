using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

public interface ISesionRepository
{
    Task AgregarAsync(Sesion sesion);

    Task<Sesion?> ObtenerPorIdAsync(long id);

    Task<Sesion?> ObtenerActivaPorUsuarioAsync(long usuarioId);

    Task ActualizarAsync(Sesion sesion);
}
