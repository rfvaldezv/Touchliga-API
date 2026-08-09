using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Touchliga.Persistence.Repositories;

public sealed class RolRepository : IRolRepository
{
    private readonly TouchligaDbContext _context;

    public RolRepository(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task<Rol?> ObtenerPorIdAsync(long id)
    {
        return await _context.Roles.FindAsync(id);
    }

    public async Task<IReadOnlyList<Rol>> ObtenerTodosAsync()
    {
        return await _context.Roles.AsNoTracking().ToListAsync();
    }

    public async Task AgregarAsync(Rol rol)
    {
        await _context.Roles.AddAsync(rol);
    }

    /// <summary>
    /// No forma parte de la interfaz genérica porque busca por nombre,
    /// no por id — se usa solo para el seed de roles en el arranque.
    /// </summary>
    public async Task<Rol?> ObtenerPorNombreAsync(string nombre)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == nombre);
    }
}
