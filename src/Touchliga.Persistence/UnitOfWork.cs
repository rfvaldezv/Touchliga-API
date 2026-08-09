using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TouchligaDbContext _context;

    public UnitOfWork(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
