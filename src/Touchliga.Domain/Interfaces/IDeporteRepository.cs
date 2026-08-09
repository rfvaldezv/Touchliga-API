using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

public interface IDeporteRepository
{
    Task<Deporte?> ObtenerPorIdAsync(long id);

    Task<Deporte?> ObtenerParaActualizarAsync(long id);

    Task<Deporte?> ObtenerPorCodigoAsync(string codigo);

    Task<IReadOnlyList<Deporte>> ObtenerTodosAsync();

    Task<bool> ExisteCodigoAsync(string codigo);

    Task AgregarAsync(Deporte deporte);

    Task ActualizarAsync(Deporte deporte);

    Task<bool> ExisteAsync(long id);
}
