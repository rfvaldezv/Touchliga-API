using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Touchliga.Persistence.Repositories;

public sealed class PagoRepository : IPagoRepository
{
    private readonly TouchligaDbContext _context;

    public PagoRepository(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task AgregarAsync(Pago pago, CancellationToken cancellationToken = default)
    {
        await _context.Pagos.AddAsync(pago, cancellationToken);
    }

    public async Task<Pago?> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Pagos.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Pago>> ObtenerListaPorUsuarioYTemporadaAsync(
        long usuarioId,
        long temporadaId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Pagos
            .Where(p => p.UsuarioId == usuarioId && p.TemporadaId == temporadaId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Pago?> ObtenerPorReferenciaAsync(
        string referencia,
        CancellationToken cancellationToken = default)
    {
        return await _context.Pagos.FirstOrDefaultAsync(
            p => p.Referencia == referencia, cancellationToken);
    }

    public async Task<IReadOnlyList<Pago>> ObtenerPorTemporadaAsync(
        long temporadaId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Pagos
            .Where(p => p.TemporadaId == temporadaId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Pago>> ObtenerPorUsuarioAsync(
        long usuarioId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Pagos
            .Where(p => p.UsuarioId == usuarioId)
            .ToListAsync(cancellationToken);
    }

    public void Eliminar(Pago pago)
    {
        _context.Pagos.Remove(pago);
    }
}
