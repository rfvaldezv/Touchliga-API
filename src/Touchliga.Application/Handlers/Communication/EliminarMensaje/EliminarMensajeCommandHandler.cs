using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Communication.Commands.EliminarMensaje;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Communication.EliminarMensaje;

public sealed class EliminarMensajeCommandHandler : IRequestHandler<EliminarMensajeCommand, Unit>
{
    private readonly IMensajeRepository _mensajes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public EliminarMensajeCommandHandler(
        IMensajeRepository mensajes,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _mensajes = mensajes;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(EliminarMensajeCommand request, CancellationToken cancellationToken)
    {
        var mensaje = await _mensajes.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Mensaje");

        // Se borra para los dos — quien lo mandó tiene la autoridad
        // de quitarlo por completo, no solo ocultarlo de su lado.
        if (mensaje.RemitenteId != _currentUser.UserId)
            throw new BusinessException("Solo puedes eliminar tus propios mensajes.");

        _mensajes.Eliminar(mensaje);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
