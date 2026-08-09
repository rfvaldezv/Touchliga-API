using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Touchliga.Persistence.Repositories;

public sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly TouchligaDbContext _context;

    public RefreshTokenRepository(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task AgregarAsync(RefreshToken token)
    {
        await _context.RefreshTokens.AddAsync(token);
    }

    public async Task<RefreshToken?> ObtenerAsync(string token)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == token);
    }

    public Task ActualizarAsync(RefreshToken token)
    {
        _context.RefreshTokens.Update(token);
        return Task.CompletedTask;
    }
}
