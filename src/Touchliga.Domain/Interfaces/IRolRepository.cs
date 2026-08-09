using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

public interface IRolRepository
{
    Task<Rol?> ObtenerPorIdAsync(long id);

    Task<Rol?> ObtenerPorNombreAsync(string nombre);

    Task<IReadOnlyList<Rol>> ObtenerTodosAsync();

    Task AgregarAsync(Rol rol);
}
