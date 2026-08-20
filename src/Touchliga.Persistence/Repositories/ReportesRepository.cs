using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
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

        var nombresPorEquipo = await _context.Equipos
            .Where(e => equipoIds.Contains(e.Id))
            .ToDictionaryAsync(e => e.Id, e => e.Nombre, cancellationToken);

        var pronosticos = await (
            from pronostico in _context.Pronosticos
            join usuario in _context.Usuarios on pronostico.UsuarioId equals usuario.Id
            where partidoIds.Contains(pronostico.PartidoId)
            select new
            {
                pronostico.UsuarioId,
                usuario.Nombre,
                usuario.Apellidos,
                pronostico.PartidoId,
                pronostico.EquipoGanadorId,
                pronostico.PuntosTotalesPredichos,
                pronostico.DiferenciaPuntosPredicha,
                pronostico.Puntos,
                pronostico.PuntosBono
            }
        ).ToListAsync(cancellationToken);

        var usuarioIdsDelDetalle = pronosticos.Select(p => p.UsuarioId).Distinct().ToList();
        var nombresVinculadosDetalle = await ObtenerNombresVinculadosAsync(usuarioIdsDelDetalle, cancellationToken);

        var resultado = pronosticos
            .GroupBy(p => new { p.UsuarioId, p.Nombre, p.Apellidos })
            .Select(g =>
            {
                var filaPartidos = partidos
                    .Select(partido =>
                    {
                        var pron = g.FirstOrDefault(x => x.PartidoId == partido.Id);

                        escudosPorEquipo.TryGetValue(partido.EquipoLocalId, out var escudoLocal);
                        escudosPorEquipo.TryGetValue(partido.EquipoVisitanteId, out var escudoVisitante);
                        nombresPorEquipo.TryGetValue(partido.EquipoLocalId, out var nombreLocal);
                        nombresPorEquipo.TryGetValue(partido.EquipoVisitanteId, out var nombreVisitante);

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
                            nombreLocal ?? string.Empty,
                            nombreVisitante ?? string.Empty,
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

                var nombreParaMostrar = nombresVinculadosDetalle.TryGetValue(g.Key.UsuarioId, out var vinculado)
                    ? $"{g.Key.Nombre} y {vinculado}"
                    : $"{g.Key.Nombre} {g.Key.Apellidos}";

                return new DetalleJornadaParticipante(g.Key.UsuarioId, nombreParaMostrar, filaPartidos, total);
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

        // Para saber desde cuándo mostrar medallas en cada jornada --
        // basta con que YA HAYA al menos un resultado capturado (se
        // actualiza en vivo conforme se van calificando partidos, no
        // hasta que la jornada esté 100% completa).
        var estatusPartidosPorJornada = await _context.Partidos
            .Where(p => jornadaIds.Contains(p.JornadaId))
            .GroupBy(p => p.JornadaId)
            .Select(g => new
            {
                JornadaId = g.Key,
                ConResultado = g.Count(p => p.GolesLocal != null && p.GolesVisitante != null)
            })
            .ToDictionaryAsync(x => x.JornadaId, x => x.ConResultado > 0, cancellationToken);

        var datos = await (
            from pronostico in _context.Pronosticos
            join partido in _context.Partidos on pronostico.PartidoId equals partido.Id
            join usuario in _context.Usuarios on pronostico.UsuarioId equals usuario.Id
            where jornadaIds.Contains(partido.JornadaId)
            select new
            {
                pronostico.UsuarioId,
                usuario.Nombre,
                usuario.Apellidos,
                TienePareja = usuario.ParejaId != null,
                usuario.NombreEquipo,
                partido.JornadaId,
                pronostico.Puntos,
                pronostico.PuntosBono
            }
        ).ToListAsync(cancellationToken);

        var usuarioIdsDelRanking = datos.Select(d => d.UsuarioId).Distinct().ToList();
        var nombresVinculados = await ObtenerNombresVinculadosAsync(usuarioIdsDelRanking, cancellationToken);

        var resultado = datos
            .GroupBy(d => new { d.UsuarioId, d.Nombre, d.Apellidos, d.TienePareja, d.NombreEquipo })
            .Select(g =>
            {
                var porJornada = jornadas
                    .Select(j => new PuntosPorJornada(
                        j.Id,
                        j.Numero,
                        g.Where(x => x.JornadaId == j.Id).Sum(x => (x.Puntos ?? 0) + x.PuntosBono),
                        g.Count(x => x.JornadaId == j.Id && x.Puntos != null),
                        estatusPartidosPorJornada.TryGetValue(j.Id, out var completa) && completa))
                    .ToList();

                var totalPuntos = porJornada.Sum(p => p.Puntos);
                var calificados = g.Count(x => x.Puntos != null);
                var puntosSinBono = g.Sum(x => x.Puntos ?? 0);
                var porcentaje = calificados > 0
                    ? Math.Round((double)puntosSinBono / calificados * 100, 1)
                    : 0;

                var nombreParaMostrar = nombresVinculados.TryGetValue(g.Key.UsuarioId, out var vinculado)
                    ? $"{g.Key.Nombre} y {vinculado}"
                    : $"{g.Key.Nombre} {g.Key.Apellidos}";

                return new RankingParticipante(
                    g.Key.UsuarioId,
                    nombreParaMostrar,
                    g.Key.TienePareja,
                    g.Key.NombreEquipo,
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

    public async Task<IReadOnlyList<ParticipantePendiente>> ObtenerParticipantesPendientesAsync(
        long jornadaId,
        CancellationToken cancellationToken = default)
    {
        var partidoIds = await _context.Partidos
            .Where(p => p.JornadaId == jornadaId)
            .Select(p => p.Id)
            .ToListAsync(cancellationToken);

        var totalPartidos = partidoIds.Count;

        // Se parte de los USUARIOS (no de los pronósticos), para que
        // quien no ha capturado NADA todavía también aparezca -- con
        // 0 de totalPartidos, no ausente de la lista.
        var conteoPorUsuario = await _context.Pronosticos
            .Where(pr => partidoIds.Contains(pr.PartidoId))
            .GroupBy(pr => pr.UsuarioId)
            .Select(g => new { UsuarioId = g.Key, Capturados = g.Count() })
            .ToDictionaryAsync(x => x.UsuarioId, x => x.Capturados, cancellationToken);

        var participantesActivos = await _context.Usuarios
            .Where(u => u.Activo)
            .Select(u => new { u.Id, Nombre = u.Nombre + " " + u.Apellidos, Correo = u.Correo.Value, u.Telefono })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return participantesActivos
            .Select(u => new ParticipantePendiente(
                u.Id,
                u.Nombre,
                u.Correo,
                u.Telefono,
                conteoPorUsuario.TryGetValue(u.Id, out var capturados) ? capturados : 0,
                totalPartidos))
            .Where(p => p.PartidosCapturados < p.TotalPartidos)
            .OrderBy(p => p.PartidosCapturados)
            .ThenBy(p => p.Nombre)
            .ToList();
    }

    public async Task<DatosReporteAuditoria> ObtenerDatosReporteAuditoriaAsync(
        long jornadaId,
        CancellationToken cancellationToken = default)
    {
        var jornada = await _context.Jornadas.FindAsync([jornadaId], cancellationToken)
            ?? throw new EntityNotFoundException("Jornada");

        var partidos = await (
            from partido in _context.Partidos
            join equipoLocal in _context.Equipos on partido.EquipoLocalId equals equipoLocal.Id
            join equipoVisitante in _context.Equipos on partido.EquipoVisitanteId equals equipoVisitante.Id
            where partido.JornadaId == jornadaId
            orderby partido.Id
            select new PartidoParaAuditoria(partido.Id, equipoLocal.Nombre, equipoVisitante.Nombre, partido.EsDesempate)
        ).ToListAsync(cancellationToken);

        var partidoIds = partidos.Select(p => p.PartidoId).ToList();

        var pronosticos = await (
            from pronostico in _context.Pronosticos
            join equipoGanador in _context.Equipos on pronostico.EquipoGanadorId equals equipoGanador.Id
            where partidoIds.Contains(pronostico.PartidoId)
            select new
            {
                pronostico.PartidoId,
                pronostico.UsuarioId,
                EquipoGanadorNombre = equipoGanador.Nombre,
                pronostico.PuntosTotalesPredichos,
                pronostico.DiferenciaPuntosPredicha
            }
        ).ToListAsync(cancellationToken);

        var participantesActivos = await _context.Usuarios
            .Where(u => u.Activo)
            .OrderBy(u => u.Nombre)
            .Select(u => new { u.Id, Nombre = u.Nombre + " " + u.Apellidos })
            .ToListAsync(cancellationToken);

        var participantes = participantesActivos.Select(participante =>
        {
            var pronosticosDeEstePartido = pronosticos
                .Where(pr => pr.UsuarioId == participante.Id)
                .Select(pr => new PronosticoParaAuditoria(
                    pr.PartidoId,
                    pr.EquipoGanadorNombre,
                    pr.PuntosTotalesPredichos,
                    pr.DiferenciaPuntosPredicha))
                .ToList();

            return new ParticipanteParaAuditoria(participante.Id, participante.Nombre, pronosticosDeEstePartido);
        }).ToList();

        return new DatosReporteAuditoria(jornada.Numero, partidos, participantes);
    }

    /// <summary>Para cada UsuarioId dado que tiene a alguien
    /// vinculado a su cuenta (comparten pronósticos/puntos), regresa
    /// el nombre de pila de esa persona -- para mostrar "Pedro y
    /// Ximena" en vez de solo "Pedro" en Ranking y Detalle de jornada.</summary>
    private async Task<Dictionary<long, string>> ObtenerNombresVinculadosAsync(
        IReadOnlyCollection<long> usuarioIds,
        CancellationToken cancellationToken)
    {
        var credenciales = await _context.CredencialesAlternas
            .Where(c => usuarioIds.Contains(c.UsuarioId))
            .Select(c => new { c.UsuarioId, Correo = c.Correo.Value })
            .ToListAsync(cancellationToken);

        if (credenciales.Count == 0)
            return new Dictionary<long, string>();

        var correos = credenciales.Select(c => c.Correo).ToList();

        var vinculados = await _context.Usuarios
            .Where(u => correos.Contains(u.Correo.Value) && u.EsCuentaVinculada)
            .Select(u => new { Correo = u.Correo.Value, u.Nombre })
            .ToListAsync(cancellationToken);

        var nombrePorCorreo = vinculados.ToDictionary(v => v.Correo, v => v.Nombre);

        return credenciales
            .Where(c => nombrePorCorreo.ContainsKey(c.Correo))
            .ToDictionary(c => c.UsuarioId, c => nombrePorCorreo[c.Correo]);
    }
}
