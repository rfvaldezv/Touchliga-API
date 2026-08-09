using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Common.Interfaces;
using Touchliga.Application.Queries.Estadisticas.GetEstadisticasParticipante;

namespace Touchliga.Application.Handlers.Estadisticas.GetEstadisticasParticipante;

public sealed class GetEstadisticasParticipanteQueryHandler
    : IRequestHandler<GetEstadisticasParticipanteQuery, EstadisticasParticipanteDto>
{
    private readonly IReportesRepository _reportes;
    private readonly ICurrentUserService _currentUser;
    private readonly IUsuarioRepository _usuarios;
    private readonly IEquipoRepository _equipos;

    public GetEstadisticasParticipanteQueryHandler(
        IReportesRepository reportes,
        ICurrentUserService currentUser,
        IUsuarioRepository usuarios,
        IEquipoRepository equipos)
    {
        _reportes = reportes;
        _currentUser = currentUser;
        _usuarios = usuarios;
        _equipos = equipos;
    }

    public async Task<EstadisticasParticipanteDto> Handle(
        GetEstadisticasParticipanteQuery request,
        CancellationToken cancellationToken)
    {
        var usuarioId = _currentUser.UserId;

        var ranking = await _reportes.ObtenerRankingAsync(request.TemporadaId, cancellationToken);
        var detallePronosticos = await _reportes.ObtenerPronosticosDetalleUsuarioAsync(
            request.TemporadaId, usuarioId, cancellationToken);

        var resultado = new EstadisticasParticipanteDto();

        // ---- Aciertos (dona) ----
        resultado.PronosticosAcertados = detallePronosticos.Count(p => p.Puntos == 1);
        resultado.PronosticosFallados = detallePronosticos.Count(p => p.Puntos == 0);

        var miRanking = ranking.FirstOrDefault(r => r.UsuarioId == usuarioId);
        if (miRanking == null)
            return resultado;

        // Solo jornadas que ya tienen al menos un pronostico calificado
        // cuentan como "jugadas" para racha/tendencia/podio/posicion.
        var jornadasConDatos = miRanking.Jornadas
            .Where(j => j.Calificados > 0)
            .OrderBy(j => j.Numero)
            .ToList();

        // ---- Racha actual ----
        var racha = 0;
        for (var i = jornadasConDatos.Count - 1; i >= 0; i--)
        {
            if (jornadasConDatos[i].Puntos > 0) racha++;
            else break;
        }
        resultado.RachaActual = racha;

        // ---- Tendencia: ultima jornada vs la anterior ----
        if (jornadasConDatos.Count >= 2)
        {
            var ultima = jornadasConDatos[^1].Puntos;
            var anterior = jornadasConDatos[^2].Puntos;
            resultado.Tendencia = ultima > anterior ? "Mejorando" : (ultima < anterior ? "Bajando" : "Estable");
        }

        // ---- Posicion actual / anterior / movimiento ----
        // Se recalcula la posicion de cada usuario sumando solo hasta
        // cierta jornada (acumulado parcial), para poder comparar
        // "donde iba" antes de la ultima jornada jugada.
        int? CalcularPosicion(int hastaJornadaNumero)
        {
            var acumulados = ranking
                .Select(r => new
                {
                    r.UsuarioId,
                    Total = r.Jornadas.Where(j => j.Numero <= hastaJornadaNumero).Sum(j => j.Puntos),
                })
                .OrderByDescending(x => x.Total)
                .ToList();

            var posicion = acumulados.FindIndex(x => x.UsuarioId == usuarioId);
            return posicion >= 0 ? posicion + 1 : null;
        }

        if (jornadasConDatos.Count > 0)
        {
            var numeroUltimaJornada = jornadasConDatos[^1].Numero;
            resultado.PosicionActual = CalcularPosicion(numeroUltimaJornada) ?? 0;

            if (jornadasConDatos.Count >= 2)
            {
                var numeroPenultimaJornada = jornadasConDatos[^2].Numero;
                resultado.PosicionAnterior = CalcularPosicion(numeroPenultimaJornada);

                if (resultado.PosicionAnterior != null)
                {
                    resultado.MovimientoPosiciones = resultado.PosicionAnterior.Value - resultado.PosicionActual;
                }
            }
        }

        // ---- Veces en podio (top 3 de una jornada) ----
        var vecesEnPodio = 0;
        foreach (var jornada in jornadasConDatos)
        {
            var posicionesEsaJornada = ranking
                .Select(r => new
                {
                    r.UsuarioId,
                    Puntos = r.Jornadas.FirstOrDefault(j => j.Numero == jornada.Numero)?.Puntos ?? 0,
                })
                .OrderByDescending(x => x.Puntos)
                .ToList();

            var miPosicion = posicionesEsaJornada.FindIndex(x => x.UsuarioId == usuarioId) + 1;
            if (miPosicion >= 1 && miPosicion <= 3) vecesEnPodio++;
        }
        resultado.VecesEnPodio = vecesEnPodio;

        // ---- Forma reciente del equipo favorito (si lo registro en Perfil) ----
        var usuario = await _usuarios.ObtenerPorIdAsync(usuarioId);
        if (usuario?.EquipoFavoritoId != null)
        {
            var equipo = await _equipos.ObtenerPorIdAsync(usuario.EquipoFavoritoId.Value, cancellationToken);
            if (equipo != null)
            {
                resultado.EquipoFavoritoNombre = equipo.Nombre;

                var ultimosPartidos = await _reportes.ObtenerUltimosResultadosEquipoAsync(
                    usuario.EquipoFavoritoId.Value, 5, cancellationToken);

                resultado.FormaEquipoFavorito = ultimosPartidos
                    .Select(p => p.GolesFavor > p.GolesContra ? "G" : "P")
                    .ToList();
            }
        }

        return resultado;
    }
}
