using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Touchliga.Persistence.Repositories;

public sealed class PartidoRepository
    : GenericRepository<Partido>, IPartidoRepository
{
    public PartidoRepository(TouchligaDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Partido>> ObtenerPorJornadaAsync(
        long jornadaId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(p => p.JornadaId == jornadaId)
            .OrderBy(p => p.FechaHora)
            .ToListAsync(cancellationToken);
    }
}
