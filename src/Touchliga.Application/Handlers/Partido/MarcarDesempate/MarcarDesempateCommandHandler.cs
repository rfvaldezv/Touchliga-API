using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Partido.MarcarDesempate;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Partido.MarcarDesempate;

public sealed class MarcarDesempateCommandHandler : IRequestHandler<MarcarDesempateCommand, Unit>
{
    private readonly IPartidoRepository _partidos;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public MarcarDesempateCommandHandler(
        IPartidoRepository partidos,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _partidos = partidos;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(MarcarDesempateCommand request, CancellationToken cancellationToken)
    {
        var partido = await _partidos.ObtenerPorIdAsync(request.PartidoId, cancellationToken)
            ?? throw new EntityNotFoundException("Partido");

        if (request.EsDesempate)
        {
            // Solo puede haber un partido de desempate por jornada --
            // si ya hay otro marcado, se desmarca solo (evita que el
            // admin tenga que hacerlo en 2 pasos).
            var partidosDeLaJornada = await _partidos.ObtenerPorJornadaAsync(partido.JornadaId, cancellationToken);
            var otroDesempate = partidosDeLaJornada.FirstOrDefault(p => p.Id != partido.Id && p.EsDesempate);

            if (otroDesempate != null)
            {
                otroDesempate.MarcarComoDesempate(false, _currentUser.UserId);
                _partidos.Actualizar(otroDesempate);
            }
        }

        partido.MarcarComoDesempate(request.EsDesempate, _currentUser.UserId);
        _partidos.Actualizar(partido);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
