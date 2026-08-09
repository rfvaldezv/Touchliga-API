using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Touchliga.Persistence.Repositories;

public sealed class UsuarioRolRepository : IUsuarioRolRepository
{
    private readonly TouchligaDbContext _context;

    public UsuarioRolRepository(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task AgregarAsync(UsuarioRol usuarioRol)
    {
        await _context.UsuarioRoles.AddAsync(usuarioRol);
    }

    public async Task<List<UsuarioRol>> ObtenerRolesAsync(long usuarioId)
    {
        return await _context.UsuarioRoles
            .Include(ur => ur.Rol)
            .Where(ur => ur.UsuarioId == usuarioId && ur.Activo)
            .ToListAsync();
    }

    public async Task<bool> ExisteAsync(long usuarioId, long rolId)
    {
        return await _context.UsuarioRoles
            .AnyAsync(ur => ur.UsuarioId == usuarioId && ur.RolId == rolId);
    }
}
