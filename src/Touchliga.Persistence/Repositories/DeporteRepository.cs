using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Touchliga.Persistence.Repositories;

public sealed class DeporteRepository : IDeporteRepository
{
    private readonly TouchligaDbContext _context;

    public DeporteRepository(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task AgregarAsync(Deporte deporte)
    {
        await _context.Deportes.AddAsync(deporte);
    }

    public Task ActualizarAsync(Deporte deporte)
    {
        _context.Deportes.Update(deporte);
        return Task.CompletedTask;
    }

    public async Task<Deporte?> ObtenerPorIdAsync(long id)
    {
        return await _context.Deportes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Deporte?> ObtenerPorCodigoAsync(string codigo)
    {
        return await _context.Deportes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Codigo == codigo);
    }

    public async Task<IReadOnlyList<Deporte>> ObtenerTodosAsync()
    {
        return await _context.Deportes
            .AsNoTracking()
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }

    public async Task<bool> ExisteCodigoAsync(string codigo)
    {
        return await _context.Deportes
            .AnyAsync(x => x.Codigo == codigo);
    }

    public async Task<Deporte?> ObtenerParaActualizarAsync(long id)
    {
    return await _context.Deportes
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExisteAsync(long id)
    {
        return await _context.Deportes
            .AnyAsync(x => x.Id == id);
    }
}
