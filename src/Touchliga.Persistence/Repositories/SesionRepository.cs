using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Touchliga.Persistence.Repositories;

public sealed class SesionRepository : ISesionRepository
{
    private readonly TouchligaDbContext _context;

    public SesionRepository(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task AgregarAsync(Sesion sesion)
    {
        await _context.Sesiones.AddAsync(sesion);
    }

    public async Task<Sesion?> ObtenerPorIdAsync(long id)
    {
        return await _context.Sesiones
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Sesion?> ObtenerActivaPorUsuarioAsync(long usuarioId)
    {
        return await _context.Sesiones
            .Where(x => x.UsuarioId == usuarioId)
            .Where(x => x.Fin == null)
            .OrderByDescending(x => x.Inicio)
            .FirstOrDefaultAsync();
    }

    public Task ActualizarAsync(Sesion sesion)
    {
        _context.Sesiones.Update(sesion);
        return Task.CompletedTask;
    }
}
