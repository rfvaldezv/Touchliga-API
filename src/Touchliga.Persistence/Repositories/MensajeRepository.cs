using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Touchliga.Persistence.Repositories;

public sealed class MensajeRepository : IMensajeRepository
{
    private readonly TouchligaDbContext _context;

    public MensajeRepository(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task AgregarAsync(Mensaje mensaje, CancellationToken cancellationToken = default)
    {
        await _context.Mensajes.AddAsync(mensaje, cancellationToken);
    }

    public async Task<Mensaje?> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Mensajes.FindAsync(new object[] { id }, cancellationToken);
    }

    public void Actualizar(Mensaje mensaje)
    {
        _context.Mensajes.Update(mensaje);
    }

    public void Eliminar(Mensaje mensaje)
    {
        _context.Mensajes.Remove(mensaje);
    }

    public async Task<IReadOnlyList<Mensaje>> ObtenerConversacionAsync(
        long usuarioId1,
        long usuarioId2,
        CancellationToken cancellationToken = default)
    {
        return await _context.Mensajes
            .Where(m =>
                (m.RemitenteId == usuarioId1 && m.DestinatarioId == usuarioId2) ||
                (m.RemitenteId == usuarioId2 && m.DestinatarioId == usuarioId1))
            .OrderBy(m => m.FechaEnvio)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Mensaje>> ObtenerUltimosPorContactoAsync(
        long usuarioId,
        CancellationToken cancellationToken = default)
    {
        // Traemos todos los mensajes donde el usuario participa y
        // agrupamos por contacto en memoria: los volúmenes de una
        // quiniela entre amigos son pequeños, y evita problemas de
        // traducción de EF Core con agregaciones + Max por grupo.
        var mensajes = await _context.Mensajes
            .Where(m => m.RemitenteId == usuarioId || m.DestinatarioId == usuarioId)
            .OrderByDescending(m => m.FechaEnvio)
            .ToListAsync(cancellationToken);

        return mensajes
            .GroupBy(m => m.RemitenteId == usuarioId ? m.DestinatarioId : m.RemitenteId)
            .Select(g => g.First())
            .ToList();
    }

    public async Task<int> ContarNoLeidosAsync(long usuarioId, CancellationToken cancellationToken = default)
    {
        return await _context.Mensajes
            .CountAsync(m => m.DestinatarioId == usuarioId && !m.Leido, cancellationToken);
    }

    public async Task MarcarConversacionLeidaAsync(
        long usuarioId,
        long otroUsuarioId,
        CancellationToken cancellationToken = default)
    {
        var noLeidos = await _context.Mensajes
            .Where(m => m.DestinatarioId == usuarioId && m.RemitenteId == otroUsuarioId && !m.Leido)
            .ToListAsync(cancellationToken);

        foreach (var mensaje in noLeidos)
        {
            mensaje.MarcarLeido();
        }
    }
}
