using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorIdAsync(long id);

    Task<Usuario?> ObtenerPorCorreoAsync(string correo);

    Task<IReadOnlyList<Usuario>> ObtenerTodosAsync();

    Task<bool> ExisteCorreoAsync(string correo);

    Task AgregarAsync(Usuario usuario);

    Task ActualizarAsync(Usuario usuario);
}
