using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Touchliga.Persistence.Repositories;

public sealed class PronosticoRepository
    : GenericRepository<Pronostico>, IPronosticoRepository
{
    public PronosticoRepository(TouchligaDbContext context) : base(context)
    {
    }

    public async Task<Pronostico?> ObtenerPorPartidoYUsuarioAsync(
        long partidoId,
        long usuarioId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(
            p => p.PartidoId == partidoId && p.UsuarioId == usuarioId,
            cancellationToken);
    }

    public async Task<IReadOnlyList<Pronostico>> ObtenerPorPartidoAsync(
        long partidoId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(p => p.PartidoId == partidoId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Pronostico>> ObtenerPorPartidoIdsAsync(
        IReadOnlyCollection<long> partidoIds,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(p => partidoIds.Contains(p.PartidoId))
            .ToListAsync(cancellationToken);
    }

    public async Task<int> ContarPorPartidoIdsYUsuarioAsync(
        IReadOnlyCollection<long> partidoIds,
        long usuarioId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(p => partidoIds.Contains(p.PartidoId) && p.UsuarioId == usuarioId)
            .CountAsync(cancellationToken);
    }
}
