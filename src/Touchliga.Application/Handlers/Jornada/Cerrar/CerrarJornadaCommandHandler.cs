using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Jornada.Cerrar;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Jornada.Cerrar;

/// <summary>
/// Cierra la jornada: a partir de este momento ya no se pueden
/// capturar ni editar pronósticos, ni corregir resultados de sus
/// partidos. Los puntos normalmente ya se calcularon antes (al
/// capturar cada resultado, o al guardar/editar un pronóstico para
/// un partido que ya tenía resultado) — este recálculo es solo una
/// red de seguridad final para no dejar ningún pronóstico sin
/// calificar antes de bloquear la jornada.
/// </summary>
public sealed class CerrarJornadaCommandHandler : IRequestHandler<CerrarJornadaCommand, Unit>
{
    private readonly IJornadaRepository _jornadas;
    private readonly IPartidoRepository _partidos;
    private readonly IPronosticoRepository _pronosticos;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CerrarJornadaCommandHandler(
        IJornadaRepository jornadas,
        IPartidoRepository partidos,
        IPronosticoRepository pronosticos,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _jornadas = jornadas;
        _partidos = partidos;
        _pronosticos = pronosticos;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(CerrarJornadaCommand request, CancellationToken cancellationToken)
    {
        var jornada = await _jornadas.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Jornada");

        var partidosDeLaJornada = await _partidos.ObtenerPorJornadaAsync(request.Id, cancellationToken);

        foreach (var partido in partidosDeLaJornada.Where(p => p.TieneResultado))
        {
            var pronosticos = await _pronosticos.ObtenerPorPartidoAsync(partido.Id, cancellationToken);

            var equipoGanadorReal = partido.GolesLocal!.Value > partido.GolesVisitante!.Value
                ? partido.EquipoLocalId
                : partido.EquipoVisitanteId;

            foreach (var pronostico in pronosticos)
            {
                pronostico.CalcularPuntos(equipoGanadorReal);
            }

            if (partido.EsDesempate)
            {
                var combinadoReal = partido.TotalPuntosReal!.Value + partido.DiferenciaPuntosReal!.Value;
                var conPrediccion = pronosticos
                    .Where(p => p.PuntosTotalesPredichos.HasValue && p.DiferenciaPuntosPredicha.HasValue)
                    .ToList();

                if (conPrediccion.Count > 0)
                {
                    var distanciaMinima = conPrediccion.Min(p =>
                        Math.Abs((p.PuntosTotalesPredichos!.Value + p.DiferenciaPuntosPredicha!.Value) - combinadoReal));

                    foreach (var pronostico in pronosticos)
                    {
                        var gano = pronostico.PuntosTotalesPredichos.HasValue
                            && pronostico.DiferenciaPuntosPredicha.HasValue
                            && Math.Abs((pronostico.PuntosTotalesPredichos.Value + pronostico.DiferenciaPuntosPredicha.Value) - combinadoReal) == distanciaMinima;

                        pronostico.AsignarPuntoBono(gano);
                    }
                }
            }

            foreach (var pronostico in pronosticos)
            {
                _pronosticos.Actualizar(pronostico);
            }
        }

        jornada.Cerrar(_currentUser.UserId);
        _jornadas.Actualizar(jornada);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
