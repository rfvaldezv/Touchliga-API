using Microsoft.EntityFrameworkCore;
using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

public sealed class ConfiguracionSmtpRepository : IConfiguracionSmtpRepository
{
    private readonly TouchligaDbContext _context;

    public ConfiguracionSmtpRepository(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task<ConfiguracionSmtp?> ObtenerAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ConfiguracionesSmtp.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AgregarAsync(ConfiguracionSmtp entidad, CancellationToken cancellationToken = default)
    {
        await _context.ConfiguracionesSmtp.AddAsync(entidad, cancellationToken);
    }

    public void Actualizar(ConfiguracionSmtp entidad)
    {
        _context.ConfiguracionesSmtp.Update(entidad);
    }
}
