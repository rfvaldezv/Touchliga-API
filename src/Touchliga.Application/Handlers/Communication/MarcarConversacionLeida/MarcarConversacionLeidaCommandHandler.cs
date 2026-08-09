using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Communication.Commands.MarcarConversacionLeida;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Communication.MarcarConversacionLeida;

public sealed class MarcarConversacionLeidaCommandHandler : IRequestHandler<MarcarConversacionLeidaCommand, Unit>
{
    private readonly IMensajeRepository _mensajes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public MarcarConversacionLeidaCommandHandler(
        IMensajeRepository mensajes,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _mensajes = mensajes;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(MarcarConversacionLeidaCommand request, CancellationToken cancellationToken)
    {
        await _mensajes.MarcarConversacionLeidaAsync(_currentUser.UserId, request.OtroUsuarioId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
