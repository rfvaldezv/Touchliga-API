using Microsoft.EntityFrameworkCore;
using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

public sealed class PremioOtorgadoRepository : IPremioOtorgadoRepository
{
    private readonly TouchligaDbContext _context;

    public PremioOtorgadoRepository(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task AgregarAsync(PremioOtorgado premio, CancellationToken cancellationToken = default)
    {
        await _context.PremiosOtorgados.AddAsync(premio, cancellationToken);
    }

    public async Task<PremioOtorgado?> ObtenerAsync(
        string ambito,
        long referenciaId,
        long usuarioId,
        CancellationToken cancellationToken = default)
    {
        return await _context.PremiosOtorgados.FirstOrDefaultAsync(
            p => p.Ambito == ambito && p.ReferenciaId == referenciaId && p.UsuarioId == usuarioId,
            cancellationToken);
    }

    public async Task<IReadOnlyList<PremioOtorgado>> ObtenerPorReferenciaAsync(
        string ambito,
        long referenciaId,
        CancellationToken cancellationToken = default)
    {
        return await _context.PremiosOtorgados
            .Where(p => p.Ambito == ambito && p.ReferenciaId == referenciaId)
            .ToListAsync(cancellationToken);
    }

    public void Actualizar(PremioOtorgado premio)
    {
        _context.PremiosOtorgados.Update(premio);
    }
}
