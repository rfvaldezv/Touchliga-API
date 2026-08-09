using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Touchliga.Persistence.Repositories;

public sealed class UsuarioRepository : IUsuarioRepository
{
    private readonly TouchligaDbContext _context;

    public UsuarioRepository(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task AgregarAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public async Task<Usuario?> ObtenerPorIdAsync(long id)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Usuario?> ObtenerPorCorreoAsync(string correo)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(x => x.Correo.Value == correo);
    }

    public async Task<IReadOnlyList<Usuario>> ObtenerTodosAsync()
    {
        return await _context.Usuarios
            .Include(u => u.Roles)
                .ThenInclude(ur => ur.Rol)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<bool> ExisteCorreoAsync(string correo)
    {
        return await _context.Usuarios
            .AnyAsync(x => x.Correo.Value == correo);
    }

    public Task ActualizarAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        return Task.CompletedTask;
    }
}
