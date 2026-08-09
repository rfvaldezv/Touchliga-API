using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

public interface IUsuarioRolRepository
{
    Task AgregarAsync(UsuarioRol usuarioRol);

    Task<List<UsuarioRol>> ObtenerRolesAsync(long usuarioId);

    Task<bool> ExisteAsync(long usuarioId, long rolId);
}
