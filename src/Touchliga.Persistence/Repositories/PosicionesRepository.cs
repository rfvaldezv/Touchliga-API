using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Touchliga.Persistence.Repositories;

public sealed class PosicionesRepository : IPosicionesRepository
{
    private readonly TouchligaDbContext _context;

    public PosicionesRepository(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PosicionResumen>> ObtenerTablaPosicionesAsync(
        long temporadaId,
        CancellationToken cancellationToken = default)
    {
        var query =
            from pronostico in _context.Pronosticos
            join partido in _context.Partidos on pronostico.PartidoId equals partido.Id
            join jornada in _context.Jornadas on partido.JornadaId equals jornada.Id
            join usuario in _context.Usuarios on pronostico.UsuarioId equals usuario.Id
            where jornada.TemporadaId == temporadaId
            group pronostico by new { pronostico.UsuarioId, usuario.Nombre } into g
            select new
            {
                g.Key.UsuarioId,
                g.Key.Nombre,
                Puntos = g.Sum(p => p.Puntos ?? 0),
                Aciertos = g.Count(p => p.Puntos != null && p.Puntos > 0),
                Pronosticos = g.Count()
            };

        var agregados = await query
            .OrderByDescending(p => p.Puntos)
            .ToListAsync(cancellationToken);

        // El record se construye en memoria: EF Core no siempre puede
        // traducir un constructor posicional dentro de un Select
        // agregado directamente a SQL.
        return agregados
            .Select(a => new PosicionResumen(
                a.UsuarioId, a.Nombre, a.Puntos, a.Aciertos, a.Pronosticos))
            .ToList();
    }
}
