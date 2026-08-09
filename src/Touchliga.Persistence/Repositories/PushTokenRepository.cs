using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Touchliga.Persistence.Repositories;

public sealed class PushTokenRepository : IPushTokenRepository
{
    private readonly TouchligaDbContext _context;

    public PushTokenRepository(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task<PushToken?> ObtenerPorTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _context.PushTokens.FirstOrDefaultAsync(t => t.Token == token, cancellationToken);
    }

    public async Task AgregarAsync(PushToken pushToken, CancellationToken cancellationToken = default)
    {
        await _context.PushTokens.AddAsync(pushToken, cancellationToken);
    }

    public void Eliminar(PushToken pushToken)
    {
        _context.PushTokens.Remove(pushToken);
    }

    public async Task<IReadOnlyList<PushToken>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PushTokens.Where(t => t.Activo).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PushToken>> ObtenerPorUsuarioAsync(
        long usuarioId, CancellationToken cancellationToken = default)
    {
        return await _context.PushTokens
            .Where(t => t.UsuarioId == usuarioId && t.Activo)
            .ToListAsync(cancellationToken);
    }
}
