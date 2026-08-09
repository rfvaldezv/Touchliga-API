using Microsoft.EntityFrameworkCore;
using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

public sealed class ConfiguracionPremioRepository : IConfiguracionPremioRepository
{
    private readonly TouchligaDbContext _context;

    public ConfiguracionPremioRepository(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task AgregarAsync(ConfiguracionPremio premio, CancellationToken cancellationToken = default)
    {
        await _context.ConfiguracionesPremio.AddAsync(premio, cancellationToken);
    }

    public async Task<ConfiguracionPremio?> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.ConfiguracionesPremio.FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<ConfiguracionPremio>> ObtenerPorTemporadaYAmbitoAsync(
        long temporadaId,
        string ambito,
        CancellationToken cancellationToken = default)
    {
        return await _context.ConfiguracionesPremio
            .Where(p => p.TemporadaId == temporadaId && p.Ambito == ambito)
            .OrderBy(p => p.Posicion)
            .ToListAsync(cancellationToken);
    }

    public void Actualizar(ConfiguracionPremio premio)
    {
        _context.ConfiguracionesPremio.Update(premio);
    }

    public void Eliminar(ConfiguracionPremio premio)
    {
        _context.ConfiguracionesPremio.Remove(premio);
    }
}
