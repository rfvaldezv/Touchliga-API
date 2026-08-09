using Microsoft.EntityFrameworkCore;
using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

public sealed class ReaccionAnuncioRepository : IReaccionAnuncioRepository
{
    private readonly TouchligaDbContext _context;

    public ReaccionAnuncioRepository(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task AgregarAsync(ReaccionAnuncio reaccion, CancellationToken cancellationToken = default)
    {
        await _context.ReaccionesAnuncio.AddAsync(reaccion, cancellationToken);
    }

    public async Task<ReaccionAnuncio?> ObtenerAsync(
        long anuncioId, long usuarioId, CancellationToken cancellationToken = default)
    {
        return await _context.ReaccionesAnuncio.FirstOrDefaultAsync(
            r => r.AnuncioId == anuncioId && r.UsuarioId == usuarioId, cancellationToken);
    }

    public async Task<IReadOnlyList<ReaccionAnuncio>> ObtenerPorAnunciosAsync(
        IReadOnlyList<long> anuncioIds, CancellationToken cancellationToken = default)
    {
        return await _context.ReaccionesAnuncio
            .Where(r => anuncioIds.Contains(r.AnuncioId))
            .ToListAsync(cancellationToken);
    }

    public void Actualizar(ReaccionAnuncio reaccion)
    {
        _context.ReaccionesAnuncio.Update(reaccion);
    }

    public void Eliminar(ReaccionAnuncio reaccion)
    {
        _context.ReaccionesAnuncio.Remove(reaccion);
    }
}
