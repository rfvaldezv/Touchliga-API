using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Partido.Update;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Partido.Update;

public sealed class UpdatePartidoCommandHandler : IRequestHandler<UpdatePartidoCommand, Unit>
{
    private readonly IPartidoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public UpdatePartidoCommandHandler(
        IPartidoRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(UpdatePartidoCommand request, CancellationToken cancellationToken)
    {
        var partido = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Partido");

        var partidosDeLaJornada = await _repository.ObtenerPorJornadaAsync(partido.JornadaId, cancellationToken);

        var yaUsado = partidosDeLaJornada.Any(p =>
            p.Id != partido.Id &&
            (p.EquipoLocalId == request.EquipoLocalId || p.EquipoVisitanteId == request.EquipoLocalId ||
             p.EquipoLocalId == request.EquipoVisitanteId || p.EquipoVisitanteId == request.EquipoVisitanteId));

        if (yaUsado)
            throw new BusinessException("Uno de los equipos ya tiene otro partido en esta jornada.");

        partido.Editar(
            request.EquipoLocalId,
            request.EquipoVisitanteId,
            request.FechaHora,
            request.CanchaId,
            _currentUser.UserId);

        _repository.Actualizar(partido);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
