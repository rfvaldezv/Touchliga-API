using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Partido.Create;
using Touchliga.Application.Common.Interfaces;
using DomainEntity = Touchliga.Domain.Entities.Partido;

namespace Touchliga.Application.Handlers.Partido.Create;

public sealed class CreatePartidoCommandHandler : IRequestHandler<CreatePartidoCommand, long>
{
    private readonly IPartidoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreatePartidoCommandHandler(
        IPartidoRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(CreatePartidoCommand request, CancellationToken cancellationToken)
    {
        if (request.EquipoLocalId == request.EquipoVisitanteId)
            throw new BusinessException("El equipo local y visitante no pueden ser el mismo.");

        var partidosDeLaJornada = await _repository.ObtenerPorJornadaAsync(request.JornadaId, cancellationToken);

        var yaUsado = partidosDeLaJornada.Any(p =>
            p.EquipoLocalId == request.EquipoLocalId || p.EquipoVisitanteId == request.EquipoLocalId ||
            p.EquipoLocalId == request.EquipoVisitanteId || p.EquipoVisitanteId == request.EquipoVisitanteId);

        if (yaUsado)
            throw new BusinessException("Uno de los equipos ya tiene un partido en esta jornada.");

        var partido = DomainEntity.Crear(
            request.JornadaId,
            request.EquipoLocalId,
            request.EquipoVisitanteId,
            request.FechaHora,
            request.CanchaId,
            _currentUser.UserId);

        await _repository.AgregarAsync(partido, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return partido.Id;
    }
}
