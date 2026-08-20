using Microsoft.EntityFrameworkCore;
using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

public sealed class CredencialAlternaRepository : ICredencialAlternaRepository
{
    private readonly TouchligaDbContext _context;

    public CredencialAlternaRepository(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task<CredencialAlterna?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default)
    {
        return await _context.CredencialesAlternas
            .FirstOrDefaultAsync(c => c.Correo.Value == correo, cancellationToken);
    }

    public async Task<CredencialAlterna?> ObtenerPorUsuarioIdAsync(long usuarioId, CancellationToken cancellationToken = default)
    {
        return await _context.CredencialesAlternas
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId, cancellationToken);
    }

    public async Task<IReadOnlyList<CredencialAlterna>> ObtenerTodasAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CredencialesAlternas.ToListAsync(cancellationToken);
    }

    public async Task AgregarAsync(CredencialAlterna entidad, CancellationToken cancellationToken = default)
    {
        await _context.CredencialesAlternas.AddAsync(entidad, cancellationToken);
    }

    public void Eliminar(CredencialAlterna entidad)
    {
        _context.CredencialesAlternas.Remove(entidad);
    }

    public async Task<string?> ObtenerNombreVinculadoAsync(long usuarioId, CancellationToken cancellationToken = default)
    {
        var credencial = await _context.CredencialesAlternas
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId, cancellationToken);

        if (credencial == null)
            return null;

        // Solo el nombre de pila -- "Pedro y Ximena", no
        // "Pedro y Ximena Valdez Alvarez".
        var vinculado = await _context.Usuarios
            .Where(u => u.Correo.Value == credencial.Correo.Value && u.EsCuentaVinculada)
            .Select(u => u.Nombre)
            .FirstOrDefaultAsync(cancellationToken);

        return vinculado;
    }
}
