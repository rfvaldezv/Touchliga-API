using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Partido.CapturarResultado;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Partido.CapturarResultado;

/// <summary>
/// Captura (o corrige) el resultado real del partido y, en el mismo
/// momento, califica todos los pronósticos ya capturados para ese
/// partido: marcador exacto = 3 puntos, acierta el resultado sin
/// marcador exacto = 1 punto, no acierta = 0 puntos.
/// </summary>
public sealed class CapturarResultadoCommandHandler : IRequestHandler<CapturarResultadoCommand, Unit>
{
    private readonly IPartidoRepository _partidos;
    private readonly IJornadaRepository _jornadas;
    private readonly IPronosticoRepository _pronosticos;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CapturarResultadoCommandHandler(
        IPartidoRepository partidos,
        IJornadaRepository jornadas,
        IPronosticoRepository pronosticos,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _partidos = partidos;
        _jornadas = jornadas;
        _pronosticos = pronosticos;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(CapturarResultadoCommand request, CancellationToken cancellationToken)
    {
        var partido = await _partidos.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Partido");

        var jornada = await _jornadas.ObtenerPorIdAsync(partido.JornadaId, cancellationToken)
            ?? throw new EntityNotFoundException("Jornada");

        if (jornada.Cerrada)
            throw new BusinessException("No se puede modificar el resultado de un partido cuya jornada ya está cerrada.");

        partido.CapturarResultado(request.GolesLocal, request.GolesVisitante, _currentUser.UserId);
        _partidos.Actualizar(partido);

        var equipoGanadorReal = request.GolesLocal > request.GolesVisitante
            ? partido.EquipoLocalId
            : partido.EquipoVisitanteId;

        var pronosticos = await _pronosticos.ObtenerPorPartidoAsync(partido.Id, cancellationToken);

        foreach (var pronostico in pronosticos)
        {
            pronostico.CalcularPuntos(equipoGanadorReal);
        }

        // Diferencial de Touchliga: en el partido de desempate de la
        // jornada, se combina suma + diferencia de puntos (ambas
        // predichas por el participante, ambas reales una vez
        // jugado el partido) — quien(es) queden más cerca (empates
        // incluidos) de ese combinado ganan 1 punto extra.
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

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
