using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

public interface IPermisoRepository
{
    Task<Permiso?> ObtenerPorCodigoAsync(string codigo);

    Task<IReadOnlyList<Permiso>> ObtenerTodosAsync();
}
