using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Pronostico.Guardar;
using Touchliga.Application.Common.Interfaces;
using DomainEntity = Touchliga.Domain.Entities.Pronostico;

namespace Touchliga.Application.Handlers.Pronostico.Guardar;

public sealed class GuardarPronosticoCommandHandler : IRequestHandler<GuardarPronosticoCommand, long>
{
    private readonly IPronosticoRepository _pronosticos;
    private readonly IPartidoRepository _partidos;
    private readonly IJornadaRepository _jornadas;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public GuardarPronosticoCommandHandler(
        IPronosticoRepository pronosticos,
        IPartidoRepository partidos,
        IJornadaRepository jornadas,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _pronosticos = pronosticos;
        _partidos = partidos;
        _jornadas = jornadas;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(GuardarPronosticoCommand request, CancellationToken cancellationToken)
    {
        var partido = await _partidos.ObtenerPorIdAsync(request.PartidoId, cancellationToken)
            ?? throw new EntityNotFoundException("Partido");

        var jornada = await _jornadas.ObtenerPorIdAsync(partido.JornadaId, cancellationToken)
            ?? throw new EntityNotFoundException("Jornada");

        if (jornada.Cerrada)
            throw new BusinessException("Ya no se pueden capturar ni editar pronósticos: la jornada está cerrada.");

        var usuarioId = _currentUser.UserId;

        var existente = await _pronosticos.ObtenerPorPartidoYUsuarioAsync(
            request.PartidoId, usuarioId, cancellationToken);

        long? equipoGanadorReal = partido.TieneResultado
            ? (partido.GolesLocal!.Value > partido.GolesVisitante!.Value ? partido.EquipoLocalId : partido.EquipoVisitanteId)
            : null;

        if (existente != null)
        {
            existente.Actualizar(
                request.EquipoGanadorId, request.PuntosTotalesPredichos, request.DiferenciaPuntosPredicha, usuarioId);

            // Está capturando/corrigiendo un pronóstico para un
            // partido que YA tiene resultado real (captura hacia
            // atrás, o edición posterior al cierre parcial) — se
            // califica en este mismo momento, no solo cuando se
            // captura el resultado.
            if (equipoGanadorReal.HasValue)
                existente.CalcularPuntos(equipoGanadorReal.Value);

            _pronosticos.Actualizar(existente);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return existente.Id;
        }

        var nuevo = DomainEntity.Crear(
            request.PartidoId,
            usuarioId,
            request.EquipoGanadorId,
            request.PuntosTotalesPredichos,
            request.DiferenciaPuntosPredicha);

        if (equipoGanadorReal.HasValue)
            nuevo.CalcularPuntos(equipoGanadorReal.Value);

        await _pronosticos.AgregarAsync(nuevo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return nuevo.Id;
    }
}
