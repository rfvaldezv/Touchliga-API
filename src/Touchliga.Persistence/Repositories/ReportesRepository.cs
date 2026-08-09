using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Touchliga.Persistence.Repositories;

public sealed class ReportesRepository : IReportesRepository
{
    private readonly TouchligaDbContext _context;

    public ReportesRepository(TouchligaDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<DetalleJornadaParticipante>> ObtenerDetalleJornadaAsync(
        long jornadaId,
        CancellationToken cancellationToken = default)
    {
        var partidos = await _context.Partidos
            .Where(p => p.JornadaId == jornadaId)
            .OrderBy(p => p.Id)
            .Select(p => new
            {
                p.Id,
                p.EquipoLocalId,
                p.EquipoVisitanteId,
                p.GolesLocal,
                p.GolesVisitante,
                p.EsDesempate
            })
            .ToListAsync(cancellationToken);

        var partidoIds = partidos.Select(p => p.Id).ToList();
        var equipoIds = partidos.Select(p => p.EquipoLocalId)
            .Concat(partidos.Select(p => p.EquipoVisitanteId))
            .Distinct()
            .ToList();

        var escudosPorEquipo = await _context.Equipos
            .Where(e => equipoIds.Contains(e.Id))
            .ToDictionaryAsync(e => e.Id, e => e.EscudoUrl, cancellationToken);

        var pronosticos = await (
            from pronostico in _context.Pronosticos
            join usuario in _context.Usuarios on pronostico.UsuarioId equals usuario.Id
            where partidoIds.Contains(pronostico.PartidoId)
            select new
            {
                pronostico.UsuarioId,
                Nombre = usuario.Nombre + " " + usuario.Apellidos,
                pronostico.PartidoId,
                pronostico.EquipoGanadorId,
                pronostico.PuntosTotalesPredichos,
                pronostico.DiferenciaPuntosPredicha,
                pronostico.Puntos,
                pronostico.PuntosBono
            }
        ).ToListAsync(cancellationToken);

        var resultado = pronosticos
            .GroupBy(p => new { p.UsuarioId, p.Nombre })
            .Select(g =>
            {
                var filaPartidos = partidos
                    .Select(partido =>
                    {
                        var pron = g.FirstOrDefault(x => x.PartidoId == partido.Id);

                        escudosPorEquipo.TryGetValue(partido.EquipoLocalId, out var escudoLocal);
                        escudosPorEquipo.TryGetValue(partido.EquipoVisitanteId, out var escudoVisitante);

                        long? equipoGanadorReal = partido.GolesLocal.HasValue && partido.GolesVisitante.HasValue
                            ? (partido.GolesLocal.Value > partido.GolesVisitante.Value ? partido.EquipoLocalId : partido.EquipoVisitanteId)
                            : null;

                        int? totalReal = partido.GolesLocal.HasValue && partido.GolesVisitante.HasValue
                            ? partido.GolesLocal.Value + partido.GolesVisitante.Value
                            : null;

                        int? diferenciaReal = partido.GolesLocal.HasValue && partido.GolesVisitante.HasValue
                            ? Math.Abs(partido.GolesLocal.Value - partido.GolesVisitante.Value)
                            : null;

                        return new DetallePartidoResumen(
                            partido.Id,
                            escudoLocal,
                            escudoVisitante,
                            equipoGanadorReal,
                            pron?.EquipoGanadorId,
                            partido.EsDesempate,
                            pron?.PuntosTotalesPredichos,
                            pron?.DiferenciaPuntosPredicha,
                            totalReal,
                            diferenciaReal,
                            pron?.Puntos,
                            pron?.PuntosBono ?? 0);
                    })
                    .ToList();

                var total = filaPartidos.Sum(x => (x.Puntos ?? 0) + x.PuntosBono);

                return new DetalleJornadaParticipante(g.Key.UsuarioId, g.Key.Nombre, filaPartidos, total);
            })
            .OrderByDescending(x => x.Total)
            .ToList();

        return resultado;
    }

    public async Task<IReadOnlyList<RankingParticipante>> ObtenerRankingAsync(
        long temporadaId,
        CancellationToken cancellationToken = default)
    {
        var jornadas = await _context.Jornadas
            .Where(j => j.TemporadaId == temporadaId)
            .OrderBy(j => j.Numero)
            .Select(j => new { j.Id, j.Numero })
            .ToListAsync(cancellationToken);

        var jornadaIds = jornadas.Select(j => j.Id).ToList();

        var datos = await (
            from pronostico in _context.Pronosticos
            join partido in _context.Partidos on pronostico.PartidoId equals partido.Id
            join usuario in _context.Usuarios on pronostico.UsuarioId equals usuario.Id
            where jornadaIds.Contains(partido.JornadaId)
            select new
            {
                pronostico.UsuarioId,
                Nombre = usuario.Nombre + " " + usuario.Apellidos,
                partido.JornadaId,
                pronostico.Puntos,
                pronostico.PuntosBono
            }
        ).ToListAsync(cancellationToken);

        var resultado = datos
            .GroupBy(d => new { d.UsuarioId, d.Nombre })
            .Select(g =>
            {
                var porJornada = jornadas
                    .Select(j => new PuntosPorJornada(
                        j.Id,
                        j.Numero,
                        g.Where(x => x.JornadaId == j.Id).Sum(x => (x.Puntos ?? 0) + x.PuntosBono),
                        g.Count(x => x.JornadaId == j.Id && x.Puntos != null)))
                    .ToList();

                var totalPuntos = porJornada.Sum(p => p.Puntos);
                var calificados = g.Count(x => x.Puntos != null);
                var puntosSinBono = g.Sum(x => x.Puntos ?? 0);
                var porcentaje = calificados > 0
                    ? Math.Round((double)puntosSinBono / calificados * 100, 1)
                    : 0;

                return new RankingParticipante(
                    g.Key.UsuarioId,
                    g.Key.Nombre,
                    porJornada,
                    totalPuntos,
                    calificados,
                    porcentaje);
            })
            .OrderByDescending(x => x.TotalPuntos)
            .ToList();

        return resultado;
    }

    public async Task<IReadOnlyList<PronosticoDetalleUsuario>> ObtenerPronosticosDetalleUsuarioAsync(
        long temporadaId,
        long usuarioId,
        CancellationToken cancellationToken = default)
    {
        var datos = await (
            from pronostico in _context.Pronosticos
            join partido in _context.Partidos on pronostico.PartidoId equals partido.Id
            join jornada in _context.Jornadas on partido.JornadaId equals jornada.Id
            join equipoLocal in _context.Equipos on partido.EquipoLocalId equals equipoLocal.Id
            join equipoVisitante in _context.Equipos on partido.EquipoVisitanteId equals equipoVisitante.Id
            where jornada.TemporadaId == temporadaId
                  && pronostico.UsuarioId == usuarioId
                  && pronostico.Puntos != null
            orderby jornada.Numero
            select new PronosticoDetalleUsuario(
                jornada.Numero,
                equipoLocal.Nombre,
                equipoVisitante.Nombre,
                pronostico.Puntos)
        ).ToListAsync(cancellationToken);

        return datos;
    }

    public async Task<IReadOnlyList<ResultadoEquipo>> ObtenerUltimosResultadosEquipoAsync(
        long equipoId,
        int cantidad,
        CancellationToken cancellationToken = default)
    {
        var comoLocal = await (
            from partido in _context.Partidos
            join jornada in _context.Jornadas on partido.JornadaId equals jornada.Id
            join rival in _context.Equipos on partido.EquipoVisitanteId equals rival.Id
            where partido.EquipoLocalId == equipoId && partido.GolesLocal != null && partido.GolesVisitante != null
            select new
            {
                partido.Id,
                jornada.Numero,
                RivalNombre = rival.Nombre,
                EsLocal = true,
                GolesFavor = partido.GolesLocal!.Value,
                GolesContra = partido.GolesVisitante!.Value,
            }
        ).ToListAsync(cancellationToken);

        var comoVisitante = await (
            from partido in _context.Partidos
            join jornada in _context.Jornadas on partido.JornadaId equals jornada.Id
            join rival in _context.Equipos on partido.EquipoLocalId equals rival.Id
            where partido.EquipoVisitanteId == equipoId && partido.GolesLocal != null && partido.GolesVisitante != null
            select new
            {
                partido.Id,
                jornada.Numero,
                RivalNombre = rival.Nombre,
                EsLocal = false,
                GolesFavor = partido.GolesVisitante!.Value,
                GolesContra = partido.GolesLocal!.Value,
            }
        ).ToListAsync(cancellationToken);

        var todos = comoLocal.Concat(comoVisitante)
            .OrderByDescending(x => x.Id)
            .Take(cantidad)
            .Select(x => new ResultadoEquipo(x.Numero, x.RivalNombre, x.EsLocal, x.GolesFavor, x.GolesContra))
            .ToList();

        return todos;
    }
}
