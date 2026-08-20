namespace Touchliga.Domain.Interfaces;

public sealed record DetallePartidoResumen(
    long PartidoId,
    string? EscudoLocalUrl,
    string? EscudoVisitanteUrl,
    string LocalNombre,
    string VisitanteNombre,
    long? EquipoGanadorReal,
    long? EquipoGanadorPronostico,
    bool EsDesempate,
    int? PuntosTotalesPredichos,
    int? DiferenciaPuntosPredicha,
    int? PuntosTotalesReal,
    int? DiferenciaPuntosReal,
    int? Puntos,
    int PuntosBono);

public sealed record DetalleJornadaParticipante(
    long UsuarioId,
    string Nombre,
    IReadOnlyList<DetallePartidoResumen> Partidos,
    int Total);

/// <summary>Puntos ya incluye el PuntosBono de esa jornada sumado.</summary>
public sealed record PuntosPorJornada(long JornadaId, int Numero, int Puntos, int Calificados, bool TodosLosPartidosConResultado);

public sealed record RankingParticipante(
    long UsuarioId,
    string Nombre,
    bool TienePareja,
    string? NombreEquipo,
    IReadOnlyList<PuntosPorJornada> Jornadas,
    int TotalPuntos,
    int PronosticosCalificados,
    double PorcentajeProductividad);

public sealed record PronosticoDetalleUsuario(
    int JornadaNumero,
    string EquipoLocalNombre,
    string EquipoVisitanteNombre,
    int? Puntos);

/// <summary>Un partido ya jugado de un equipo en particular, visto
/// desde la perspectiva de ese equipo (a favor/en contra) — para
/// armar una racha de forma (Gana/Pierde, NFL no tiene empates).</summary>
public sealed record ResultadoEquipo(
    int JornadaNumero,
    string RivalNombre,
    bool EsLocal,
    int GolesFavor,
    int GolesContra);

/// <summary>Un participante activo y su avance de captura en UNA
/// jornada -- incluye a quienes no han capturado NADA todavía (a
/// diferencia de ObtenerDetalleJornadaAsync, que solo lista a
/// quienes ya tienen al menos un pronóstico).</summary>
public sealed record ParticipantePendiente(
    long UsuarioId,
    string Nombre,
    string Correo,
    string? Telefono,
    int PartidosCapturados,
    int TotalPartidos);

/// <summary>
/// Consultas de reporte (no son un repositorio de una entidad en
/// particular, sino vistas agregadas sobre Pronostico + Partido +
/// Jornada + Usuario) — igual que IPosicionesRepository.
/// </summary>
public interface IReportesRepository
{
    /// <summary>Detalle partido por partido de UNA jornada, por participante.</summary>
    Task<IReadOnlyList<DetalleJornadaParticipante>> ObtenerDetalleJornadaAsync(
        long jornadaId,
        CancellationToken cancellationToken = default);

    /// <summary>Ranking de la temporada: puntos por jornada, total y % de productividad.</summary>
    Task<IReadOnlyList<RankingParticipante>> ObtenerRankingAsync(
        long temporadaId,
        CancellationToken cancellationToken = default);

    /// <summary>Todos los pronósticos YA calificados de un usuario en
    /// una temporada — para estadísticas personales (aciertos/fallos).</summary>
    Task<IReadOnlyList<PronosticoDetalleUsuario>> ObtenerPronosticosDetalleUsuarioAsync(
        long temporadaId,
        long usuarioId,
        CancellationToken cancellationToken = default);

    /// <summary>Los últimos N partidos YA JUGADOS de un equipo (en
    /// cualquier temporada/jornada), más recientes primero.</summary>
    Task<IReadOnlyList<ResultadoEquipo>> ObtenerUltimosResultadosEquipoAsync(
        long equipoId,
        int cantidad,
        CancellationToken cancellationToken = default);

    /// <summary>Participantes activos con su avance de captura en UNA
    /// jornada -- para saber a quién recordarle que le falta capturar.
    /// Incluye a quienes no han hecho nada todavía.</summary>
    Task<IReadOnlyList<ParticipantePendiente>> ObtenerParticipantesPendientesAsync(
        long jornadaId,
        CancellationToken cancellationToken = default);

    /// <summary>Todo lo necesario para armar el PDF de auditoría de
    /// una jornada: número de la jornada, sus partidos (con nombres
    /// de equipo), los participantes activos, y sus pronósticos.</summary>
    Task<DatosReporteAuditoria> ObtenerDatosReporteAuditoriaAsync(
        long jornadaId,
        CancellationToken cancellationToken = default);
}

public sealed record DatosReporteAuditoria(
    int JornadaNumero,
    IReadOnlyList<PartidoParaAuditoria> Partidos,
    IReadOnlyList<ParticipanteParaAuditoria> Participantes);

public sealed record PartidoParaAuditoria(long PartidoId, string LocalNombre, string VisitanteNombre, bool EsDesempate);

public sealed record ParticipanteParaAuditoria(long UsuarioId, string Nombre, IReadOnlyList<PronosticoParaAuditoria> Pronosticos);

public sealed record PronosticoParaAuditoria(
    long PartidoId,
    string EquipoGanadorNombre,
    int? PuntosTotalesPredichos,
    int? DiferenciaPuntosPredicha);
