using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

public sealed class ArchivoRepository : IArchivoRepository
{
    private readonly TouchligaDbContext _context;

    public ArchivoRepository(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task AgregarAsync(Archivo archivo, CancellationToken cancellationToken = default)
    {
        await _context.Archivos.AddAsync(archivo, cancellationToken);
    }

    public async Task<Archivo?> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Archivos.FindAsync(new object[] { id }, cancellationToken);
    }
}
